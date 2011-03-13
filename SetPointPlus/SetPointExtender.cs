using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Win32;

namespace SetPointPlus
{
	// すること
	// オーバーヘッドを無くしていく
	// GetDeviceName 時点で一度読んだ XML を保持しておく

	static class SetPointExtender
	{
		static readonly string SetPointDirectory;
		static readonly string InstalledDevices;
		static readonly string DevicesFolder;

		static SetPointExtender()
		{
			SetPointDirectory = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "SetPoint Directory", null) as string;
			InstalledDevices = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Logitech\Info", "SP5Devices", null) as string;
			DevicesFolder = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "Devices Folder", null) as string;
		}

		private static void BackupFile(string fileName)
		{
			// 元ファイルが無い場合、例外をだすべき？
			string backupName = fileName + ".bak";
			if (File.Exists(fileName) && !File.Exists(backupName)) // fileName があり、かつ、バックアップがまだ作成されていない
			{
				File.Copy(fileName, backupName);
			}
		}

		public static void DeleteUserSettingFile(bool createBackup)
		{
			string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string userFile = Path.Combine(appData, @"Logitech\SetPoint\user.xml");

			if (createBackup)
				BackupFile(userFile);

			if (File.Exists(userFile))
			{
				File.Delete(userFile);
			}
		}

		public static void RestartSetPoint()
		{
			// TODO: LINQ 使わなくてもいいよね・・・
			var processes =
				from p in Process.GetProcesses()
				where p.ProcessName.Equals("SetPoint", StringComparison.InvariantCultureIgnoreCase) || p.ProcessName.Equals("SetPoint32", StringComparison.InvariantCultureIgnoreCase)
				select p;

			string setPointFileName = null;
			foreach (var proc in processes)
			{
				if (proc.MainModule.FileName.EndsWith("SetPoint.exe", StringComparison.InvariantCultureIgnoreCase))
				{
					// SetPoint.exe のパスを記憶しておく (SetPoint32.exe は SetPoint.exe を起動すれば同時に起動する)
					setPointFileName = proc.MainModule.FileName;
				}
				proc.Kill();
			}

			if (!string.IsNullOrEmpty(setPointFileName))
				Process.Start(setPointFileName);
		}

		public static DeviceInfo[] GetInstalledDevices()
		{
			List<DeviceInfo> devices = new List<DeviceInfo>();

			if (!string.IsNullOrEmpty(InstalledDevices) && !string.IsNullOrEmpty(DevicesFolder))
			{
				string[] ids = InstalledDevices.Split(',');
				string[] deviceFiles = Directory.GetFiles(DevicesFolder, "*.xml", SearchOption.AllDirectories);
				foreach (var id in ids)
				{
					foreach (var file in deviceFiles)
					{
						if (Path.GetFileNameWithoutExtension(file).Equals(id, StringComparison.InvariantCultureIgnoreCase))
						{
							XDocument document = XDocument.Load(file);
							devices.Add(new DeviceInfo(GetDeviceName(document), id, file, document));
							break;
						}
						// continue
					}
				}
			}

			return devices.ToArray();
		}

		private static string GetDeviceName(XDocument document)
		{
			XElement device = document.Root.Element("Devices").Element("Device");
			return device.Attribute("DisplayName").Value;
		}

		public static bool IsValidHandlerSet(string name)
		{
			// 除外するコマンド
			if (name.StartsWith("AppOverride_") || name == "ButtonUsage" || name == "FirstUsageSet")
				return false;
			return true;
		}

		public static void ApplyToDefaultXml()
		{
			// SetPointDirectory が null なら SetPoint はインストールされていない
			//if (string.IsNullOrEmpty(SetPointDirectory)) return;

			string defaultXml = Path.Combine(SetPointDirectory, "default.xml");
			BackupFile(defaultXml);

			// default.xml を読み込む
			XDocument document = XDocument.Load(defaultXml);

			// 検証して HelpString が無い場合、また、重複していると思われるハンドラセットを除外
			var handlerSets =
				from h in document.Root.Element("HandlerSets").Elements()
				where IsValidHandlerSet(h.Attribute("Name").Value) && h.Attribute("HelpString") != null && !string.IsNullOrEmpty(h.Attribute("HelpString").Value)
				group h by h.Attribute("HelpString").Value into helpStringGroup
				select helpStringGroup.ElementAt(0).Attribute("Name").Value;

			// カンマ区切りにする
			StringBuilder sb = new StringBuilder();
			foreach (var item in handlerSets)
			{
				sb.Append(item);
				sb.Append(",");
			}

			XElement setPointPlusElement =
				new XElement("HandlerSetGroup", new XAttribute("Name", "SetPointPlus"), new XAttribute("HandlerSetNames", sb.ToString()));

			var handlerSetGroups = document.Root.Element("HandlerSetGroups");

			// 既に改変してあっても作成し直している。
			// 先に改変チェックして処理しないようにした方がいいか
			XElement firstElement = handlerSetGroups.Element("HandlerSetGroup");
			if (firstElement.Attribute("Name").Value == "SetPointPlus")
			{
				firstElement.Remove();
			}
			handlerSetGroups.AddFirst(setPointPlusElement);
			//document.Save(defaultXml, SaveOptions.DisableFormatting);
			document.Save(defaultXml);
		}

		public static void ApplyToDeviceXml(DeviceInfo info)
		{
			// まずバックアップを作成
			BackupFile(info.FilePath);

			// デバイス構成ファイルを読み込む
			//XDocument document = XDocument.Load(info.FilePath);
			XDocument document = info.Document;

			XElement device = document.Root.Element("Devices").Element("Device");

			// マウスなら左右入れ替え OFF
			if (device.Attribute("Class").Value == "PointingDevice")
				device.Element("PARAM").SetAttributeValue("Swapable", 0);

			// ボタンにオプションを適用
			var buttons = device.Element("Buttons").Elements("Button");
			foreach (var button in buttons)
			{
				// アプリケーション毎の設定
				XElement param = button.Element("PARAM");
				if (param == null)
				{
					param = new XElement("PARAM"); // null だったら追加
					button.AddFirst(param);
				}
				param.SetAttributeValue("AppSpecificSettingHidden", 0);

				// Silent="0" なら、左右クリックに他のコマンドを適用できる。
				// だけど UAC ダイアログでマウスが効かない
				// 汎用ボタンに設定すれば回避できるがプログラム毎の設定ができない等問題がある。
				XElement trigger = button.Element("Trigger");
				XElement triggerParam = trigger.Element("PARAM");
				if (triggerParam != null)
				{
					XAttribute silent = triggerParam.Attribute("Silent");
					if (silent != null)
						silent.Remove(); // あったら削除
				}

				// SetPointPlus を各ボタンに適用
				var triggerStates = trigger.Elements("TriggerState");
				foreach (var triggerState in triggerStates)
				{
					triggerState.SetAttributeValue("HandlerSetGroup", "SetPointPlus");
				}
			}

			document.Save(info.FilePath);
		}

		private static void RestoreBackupFile(string backupFileName)
		{
			// 拡張子 .bak に決めうち
			string originalFileName = backupFileName.Substring(0, backupFileName.Length - 4);
			//string originalFileName = Path.Combine(Path.GetDirectoryName(backupFileName), Path.GetFileNameWithoutExtension(backupFileName));
			if (File.Exists(backupFileName))
			{
				if (File.Exists(originalFileName)) // 先に元ファイルを削除
					File.Delete(originalFileName);
				File.Move(backupFileName, originalFileName); // 元ファイルにリネーム
			}
		}

		public static void RestoreDefaultXml()
		{
			string backupFile = Path.Combine(SetPointDirectory, "default.xml.bak");
			RestoreBackupFile(backupFile);
		}

		public static void RestoreDeviceXml()
		{
			string[] backupFiles = Directory.GetFiles(DevicesFolder, "*.bak", SearchOption.AllDirectories);
			foreach (var file in backupFiles)
			{
				RestoreBackupFile(file);
			}
		}

		public static void RestoreUserSettingFile()
		{
			string backupFile = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Logitech\SetPoint\user.xml.bak");

			RestoreBackupFile(backupFile);
		}
	}
}
