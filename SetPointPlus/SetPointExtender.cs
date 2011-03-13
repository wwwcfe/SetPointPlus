using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Win32;

namespace SetPointPlus
{
	static class SetPointExtender
	{
		static readonly string SetPointDirectory = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "SetPoint Directory", null) as string;
		static readonly string InstalledDevices = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Logitech\Info", "SP5Devices", null) as string;
		static readonly string DevicesFolder = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "Devices Folder", null) as string;

		private static void BackupFile(string fileName)
		{
			// 元ファイルが無い場合、例外をだすべき？
			string backupName = fileName + ".bak";
			// 元ファイルがあり、かつ、バックアップがまだ作成されていない
			if (File.Exists(fileName) && !File.Exists(backupName))
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
				File.Delete(userFile);
		}

		public static void RestartSetPoint()
		{
			string setPointFileName = null;
			foreach (var proc in Process.GetProcesses())
			{
				if (proc.ProcessName.Equals("SetPoint", StringComparison.OrdinalIgnoreCase) ||
					proc.ProcessName.Equals("SetPoint32", StringComparison.OrdinalIgnoreCase))
				{
					if (proc.MainModule.FileName.EndsWith("SetPoint.exe", StringComparison.OrdinalIgnoreCase))
					{
						// SetPoint.exe のパスを記憶しておく
						// (SetPoint32.exe は SetPoint.exe を起動すれば同時に起動する)
						setPointFileName = proc.MainModule.FileName;
					}
					proc.Kill();
				}
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
						if (Path.GetFileNameWithoutExtension(file).Equals(id, StringComparison.OrdinalIgnoreCase))
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

		public static bool IsValidHandlerSet(XElement handlerSet)
		{
			string name = handlerSet.Attribute("Name").Value;
			// 除外するコマンド
			if (string.IsNullOrEmpty(name) ||
				name.StartsWith("AppOverride_", StringComparison.OrdinalIgnoreCase) ||
				name.Equals("ButtonUsage", StringComparison.OrdinalIgnoreCase) ||
				name.Equals("FirstUsageSet", StringComparison.OrdinalIgnoreCase))
				return false;

			XAttribute helpString = handlerSet.Attribute("HelpString");
			if (helpString == null || string.IsNullOrEmpty(helpString.Value))
				return false;

			return true;
		}

		public static void ApplyToDefaultXml()
		{
			// SetPointDirectory が null なら SetPoint はインストールされていない
			// 処理しなかったことを通知しなくてもいいか？
			if (string.IsNullOrEmpty(SetPointDirectory)) return;

			string defaultXml = Path.Combine(SetPointDirectory, "default.xml");
			BackupFile(defaultXml);

			// default.xml を読み込む
			XDocument document = XDocument.Load(defaultXml);

			// 検証して HelpString が無い場合、また、重複していると思われるハンドラセットを除外
		    var handlerSets =
		        from h in document.Root.Element("HandlerSets").Elements()
		        where IsValidHandlerSet(h)
                group h by h.Attribute("HelpString").Value into helpStringGroup
                select helpStringGroup.First().Attribute("Name").Value;
                //select h.Attribute("Name").Value;

			#region Linq を使わない方法(XmlDocument を使う)はこんな感じになると思う
			//List<string> nameCache = new List<string>();
			//StringBuilder sb = new StringBuilder();
			//foreach (var item in document.Root.Element("HandlerSets").Elements())
			//{
			//    if (IsValidHandlerSet(item))
			//    {
			//        string helpString = item.Attribute("HelpString").Value;
			//        if (!nameCache.Contains(helpString))
			//        {
			//            nameCache.Add(helpString);
			//            sb.Append(item.Attribute("Name").Value).Append(",");
			//        }
			//    }
			//}
			#endregion

			// カンマ区切りにする
			StringBuilder sb = new StringBuilder();
			foreach (var item in handlerSets)
			{
				sb.Append(item);
				sb.Append(",");
			}

			XElement setPointPlusElement = null;
			var handlerSetGroups = document.Root.Element("HandlerSetGroups");
			foreach (var group in handlerSetGroups.Elements("HandlerSetGroup"))
			{
				// 以前追加されてるならそれを使う
				if (group.Attribute("Name").Value.Equals("SetPointPlus", StringComparison.OrdinalIgnoreCase))
				{
					setPointPlusElement = group;
					break;
				}
			}

			// 以前追加されてなければ作って追加
			if (setPointPlusElement == null)
			{
				setPointPlusElement = new XElement("HandlerSetGroup", new XAttribute("Name", "SetPointPlus"));
				handlerSetGroups.AddFirst(setPointPlusElement);
			}
			setPointPlusElement.SetAttributeValue("HandlerSetNames", sb.ToString());

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
			if (device.Attribute("Class").Value.Equals("PointingDevice", StringComparison.OrdinalIgnoreCase))
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
				// 汎用ボタンに設定すれば回避できるがプログラム毎の設定ができない
				XElement trigger = button.Element("Trigger");
				XElement triggerParam = trigger.Element("PARAM");
				if (triggerParam != null)
				{
					XAttribute silent = triggerParam.Attribute("Silent");
					if (silent != null)
						silent.Remove(); // Silent があったら削除
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

        /// <summary>
        /// Strings.xml に SetPointPlus の設定を書き込みます。
        /// 設定のテキストが重複した場合、重複したテキストに番号を割り当てます。
        /// </summary>
        public static void ApplyToHelpString()
        {
            var stringsXml = Path.Combine(SetPointDirectory, "Strings.xml");

            // 先に復元してもいいけど、バージョンアップで上書きされた場合、
            // 古い Strings.xml を復元してしまう可能性があるのでやめる。
            //RestoreHelpString();

            BackupFile(stringsXml);

            XDocument document = XDocument.Load(stringsXml);
            var elements = from e in document.Root.Element("StringSet").Elements("String")
                           where e.Attribute("ALIAS").Value.StartsWith("HELP_")
                           select e;

            // 設定のテキスト, 番号
            var dict = new Dictionary<string, int>();
            foreach (var element in elements)
            {
                if (dict.ContainsKey(element.Value))
                {
                    // かぶった場合
                    dict[element.Value] = dict[element.Value] + 1;
                    element.Value = element.Value + string.Format(" ({0})", dict[element.Value]);
                }
                else
                {
                    // 重複しなかった
                    // Key は 要素 (String 要素の値)
                    dict.Add(element.Value, 1);
                }
            }

            document.Save(stringsXml);
        }

		private static void RestoreBackupFile(string backupFileName)
		{
			// original.xml.bak に決めうち
			string originalFileName = backupFileName.Substring(0, backupFileName.Length - 4);
			//string originalFileName =
			//    Path.Combine(Path.GetDirectoryName(backupFileName), Path.GetFileNameWithoutExtension(backupFileName));
			if (File.Exists(backupFileName))
			{
				if (File.Exists(originalFileName)) // 先に元ファイルを削除
					File.Delete(originalFileName);
				File.Move(backupFileName, originalFileName); // 元ファイルにリネーム
			}
		}

		public static void RestoreDefaultXml()
		{
			// インストールされていない
			if (SetPointDirectory == null) return;

			string backupFile = Path.Combine(SetPointDirectory, "default.xml.bak");
			RestoreBackupFile(backupFile);
		}

		public static void RestoreDeviceXml()
		{
			// インストールされてない
			if (DevicesFolder == null) return;

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

        public static void RestoreHelpString()
        {
            if (SetPointDirectory == null) return;

            string backupFile = Path.Combine(SetPointDirectory, "Strings.xml.bak");
            RestoreBackupFile(backupFile);
        }
	}
}
