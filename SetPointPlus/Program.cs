using System;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
/*
SetPointPlus: Released under the MIT License

Copyright (c) 2010-2011 wwwcfe

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

/*
This program modify SetPoint configration files.
You can select all commands hidden by default.
Binary Download
URL: http://d.hatena.ne.jp/wwwcfe/20090901/SetPointPlus
*/
namespace SetPointPlus
{
	class Program
	{
		static void Main(string[] args)
		{
			PrintNotes();
			if (args.Length == 0)
			{
				PrintUsage();
				return;
			}

			int ver = 0;
			bool restore = false;
			bool list = false;
			bool advanced = false;
			var targets = new List<string>();
			foreach (var a in args)
			{
				switch (a)
				{
					case "-v6": ver = 6; break;
					case "-v4": ver = 4; break;
					case "--1": ver = -1; break; // for test
					case "-r": restore = true; break;
					case "-l": list = true; break;
					case "-a": advanced = true; break;
					default:
						if (!a.StartsWith("-"))
							targets.Add(a.ToLower());
						break;
				}
			}

			if (ver == 0)
			{
				PrintUsage();
				return;
			}

			var info = GetInfo(ver);
			if (list)
			{
				PrintDeviceIds(info);
				return;
			}

			if (restore) Restore(info);
			else Apply(info, targets.ToArray(), advanced);

			RestartSetPoint();
		}

		static void PrintUsage()
		{
			Console.WriteLine("Usage...");
			Console.WriteLine("  -v6: for SetPoint 6.x");
			Console.WriteLine("  -v4: for SetPoint 4.x");
			Console.WriteLine("  -a : show all commands for mouse left/right button");
			Console.WriteLine("       but some problem occurs");
			Console.WriteLine("  -r : restore SetPoint and all installed devices");
			Console.WriteLine("  -l : list installed devices");
			Console.WriteLine();
			Console.WriteLine("Example...");
			Console.WriteLine("  SetPoint 6.x   : SetPointPlus.exe -v6");
			Console.WriteLine("  SetPoint 4.x   : SetPointPlus.exe -v4");
			Console.WriteLine("  Restore  6.x   : SetPointPlus.exe -v6 -r");
			Console.WriteLine("  List devices   : SetPointPlus.exe -v6 -l");
			Console.WriteLine("  Specify devices: SetPointPlus.exe -v6 100009f 2000068");
			Console.WriteLine();
		}

		static void PrintNotes()
		{
			Console.WriteLine("SetPointPlus version 1.0");
			Console.WriteLine("Copyright (c) 2011 wwwcfe");
			Console.WriteLine("The MIT License (see License.txt)");
			Console.WriteLine();
			Console.WriteLine("Note...");
			Console.WriteLine("  * You should run this application as Administrator");
			Console.WriteLine("  * Exit SetPoint before run this application");
			Console.WriteLine("  * Your SetPoint setting will be reset");
			Console.WriteLine("  * Before updating SetPoint, you should uninstall previous version ");
			Console.WriteLine("    and delete .bak files (run SPP with -r option)");
			Console.WriteLine();
		}

		static void PrintDeviceIds(string[] info)
		{
			Console.WriteLine("Installed Devices...");
			var ids = info[2] != null ? info[2].Split(',') : new string[] { };
			if (ids.Length == 0)
			{
				Console.WriteLine("  Not found");
				return;
			}
			foreach (var id in ids)
			{
				var doc = new XmlDocument();
				var files = Directory.GetFiles(info[1], id + ".xml", SearchOption.AllDirectories);
				doc.Load(files[0]);
				var node = doc.SelectSingleNode("//Device");
				Console.WriteLine(string.Format("  {0}: {1}", id, node.Attributes["DisplayName"].Value));
			}
			
		}

		static void RestartSetPoint()
		{
			string filename = null;
			foreach (var p in Process.GetProcesses())
			{
				// kill setpoint.exe and setpoint32.exe
				if (p.ProcessName.Equals("setpoint", StringComparison.OrdinalIgnoreCase) ||
					p.ProcessName.Equals("setpoint32", StringComparison.OrdinalIgnoreCase))
				{
					if (p.MainModule.FileName.EndsWith("setpoint.exe", StringComparison.OrdinalIgnoreCase))
						filename = p.MainModule.FileName;
					p.Kill();
				}
			}
			if (filename != null)
				Process.Start(filename);
		}

		static string GetRegValue(string key, string name)
		{
			return Registry.GetValue(key, name, null) as string;
		}

		static string[] GetInfo(int ver)
		{
			switch (ver)
			{
				case 6:
					return new[]
					       	{
					       		GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\EvtMgr6", "InstallLocation"),
					       		GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\EvtMgr6", "DevicesFilePath"),
					       		GetRegValue(@"HKEY_CURRENT_USER\Software\Logitech\Info", "SP5Devices")
					       	};
				case 4:
					return new[]
					       	{
					       		GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "SetPoint Directory"),
					       		GetRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Logitech\SetPoint\CurrentVersion\Setup", "Devices Folder"),
					       		GetRegValue(@"HKEY_CURRENT_USER\Software\Logitech\Info", "SP5Devices")
					       	};
				case -1:
					return new[]
					       	{
					       		"testdata",
					       		"testdata",
					       		"100009F"
					       	};
				default:
					throw new NotImplementedException();
			}
		}

		static bool CanProcess(string[] info)
		{
			foreach (var s in info)
			{
				if (string.IsNullOrEmpty(s)) return false;
			}
			return true;
		}

		static void Restore(string[] info)
		{
			if (info[0] != null)
			{
				RestoreFile(info[0] + @"\Strings.xml");
				RestoreFile(info[0] + @"\default.xml");
			}
			if (info[1] == null)
				return;
			var ids = info[2] != null ? info[2].Split(',') : new string[] { };
			foreach (var id in ids)
			{
				var files = Directory.GetFiles(info[1], id + ".xml", SearchOption.AllDirectories);
				Debug.Assert(files.Length == 1);
				foreach (var f in files)
				{
					RestoreFile(f);
				}
			}
			RestoreFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
				@"\Logitech\SetPoint\user.xml");
		}

		static void RestoreFile(string filename)
		{
			var backupName = filename + ".bak";
			if (File.Exists(backupName))
			{
				Console.WriteLine("Restoring {0}...", filename);
				File.Copy(backupName, filename, true);
				File.Delete(backupName);
			}
		}

		static void BackupFile(string fileName)
		{
			var backupName = fileName + ".bak";
			if (File.Exists(fileName) && !File.Exists(backupName))
			{
				File.Copy(fileName, backupName);
			}
		}

		static void Apply(string[] info, IList<string> targets, bool advanced)
		{
			if (!CanProcess(info))
			{
				Console.WriteLine("SetPoint is not installed. Or installed devices are not found.");
				return;
			}
			var stringsxml = info[0] + @"\Strings.xml";
			var defaultxml = info[0] + @"\default.xml";
			BackupFile(stringsxml);
			BackupFile(defaultxml);
			ApplyToStrings(stringsxml);
			ApplyToDefault(defaultxml);
			var ids = info[2].Split(',');
			foreach (var id in ids)
			{
				if (targets.Count > 0 && !targets.Contains(id.ToLower()))
					continue;
				var files = Directory.GetFiles(info[1], id + ".xml", SearchOption.AllDirectories);
				Debug.Assert(files.Length == 1);
				foreach (var f in files)
				{
					BackupFile(f);
					ApplyToDevice(f, advanced);
				}
			}

			var userxml = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				@"Logitech\SetPoint\user.xml");
			BackupFile(userxml);
			if (File.Exists(userxml))
				File.Delete(userxml);
		}

		static void ApplyToStrings(string filename)
		{
			Console.WriteLine("Modifying {0}...", filename);
			var doc = new XmlDocument();
			doc.Load(filename);
			var nodes = doc.SelectNodes("//String[starts-with(@ALIAS, 'HELP_')]");
			var dict = new Dictionary<string, int>();
			foreach (var item in nodes)
			{
				var n = (XmlNode)item;
				var text = n.InnerText;
				if (dict.ContainsKey(text))
				{
					dict[text]++;
					n.InnerText += string.Format(" ({0})", dict[text]);
				}
				else
				{
					dict.Add(text, 1);
				}
			}
			doc.Save(filename);
		}

		static void ApplyToDevice(string filename, bool advanced)
		{
			var doc = new XmlDocument();
			doc.Load(filename);
			var device = (XmlElement)doc.SelectSingleNode("//Device");
			Console.WriteLine("Modifying {0}...", device.GetAttribute("DisplayName"));
			Console.WriteLine("  File: {0}", filename);
			device.SetAttribute("AppTier", "2");
			if (advanced && device.GetAttribute("Class") == "PointingDevice")
			{
				((XmlElement)device.SelectSingleNode("PARAM")).SetAttribute("Swapable", "0");
			}

			foreach (var node in doc.SelectNodes("//Buttons/Button"))
			{
				var button = (XmlElement)node;
				var name = button.GetAttribute("Name");
				// ignore mouse left/right button if not advanced
				if (!advanced && (name == "1" || name == "2"))
					continue;

				// show app specific setting
				var param = (XmlElement)button.SelectSingleNode("PARAM");
				if (param == null)
				{
					param = doc.CreateElement("PARAM");
					button.PrependChild(param);
				}
				param.SetAttribute("AppSpecificSettingHidden", "0");

				// for app specific setting
				// if advanced, all commands for l/r buttons become selectable.
				// but mouse left/right buttons become ineffective some situation
				// (UAC window, SetPoint main window close button etc...)
				// you should use generic button if you dont have any reason
				var triggerParam = (XmlElement)button.SelectSingleNode("Trigger/PARAM");
				if (triggerParam != null && triggerParam.HasAttribute("Silent"))
					triggerParam.RemoveAttribute("Silent");

				// modify default group to SetPointPlus group
				foreach (var state in button.SelectNodes("Trigger/TriggerState"))
				{
					((XmlElement)state).SetAttribute("HandlerSetGroup", "SetPointPlus");
				}
			}
			doc.Save(filename);
		}

		static void ApplyToDefault(string filename)
		{
			Console.WriteLine("Modifying {0}...", filename);
			var doc = new XmlDocument();
			doc.Load(filename);
			var list = new List<string>();
			var nodes = doc.SelectNodes(
				"//HandlerSet[string-length(@HelpString) > 0]" +
				"[not(@HelpString = preceding-sibling::HandlerSet/@HelpString)]" +
				"[not(starts-with(@Name,'AppOverride_') or starts-with(@Name, 'ButtonUsage') or " +
				"starts-with(@Name, 'FirstUsageSet'))]");
			Console.WriteLine("{0} valid handler sets found", nodes.Count);
			foreach (var item in nodes)
			{
				var n = (XmlNode)item;
				list.Add(n.Attributes["Name"].Value);
			}
			var handlerSets = doc.SelectSingleNode("//HandlerSets");
			
			if (!list.Contains("DoubleKeystroke"))
			{
				handlerSets.AppendChild(CreateKeystrokeHandler(doc, "DoubleKeystroke", 2));
				list.Add("DoubleKeystroke");
			}
			if (!list.Contains("TripleKeystroke"))
			{
				handlerSets.AppendChild(CreateKeystrokeHandler(doc, "TripleKeystroke", 3));
				list.Add("TripleKeystroke");
			}

			// add SetPointPlus
			var spp = (XmlElement)doc.SelectSingleNode("//HandlerSetGroup[@Name = 'SetPointPlus']");
			var handlerSetNames = string.Join(",", list.ToArray());
			if (spp == null)
			{
				spp = doc.CreateElement("HandlerSetGroup");
				spp.SetAttribute("Name", "SetPointPlus");
			}
			spp.SetAttribute("HandlerSetNames", handlerSetNames);

			var groups = doc.SelectSingleNode("//HandlerSetGroups");
			groups.PrependChild(spp);

			// add SetPointPlus to all "HandlerSetGroup" (for all devices)
			//foreach (var node in groups.SelectNodes("HandlerSetGroup[not(@Name = 'SetPointPlus')]"))
			//{
			//    var el = (XmlElement)node;
			//    el.SetAttribute("HandlerSetNames", el.GetAttribute("HandlerSetNames") + ",SetPointPlus");
			//}

			doc.Save(filename);
		}

		static XmlElement CreateKeystrokeHandler(XmlDocument doc, string name, int count)
		{
			var el = doc.CreateElement("HandlerSet");
			el.SetAttribute("Name", name);
			el.SetAttribute("HelpString", name);
			var handler = doc.CreateElement("Handler");
			handler.SetAttribute("Class", "KeystrokeAssignment");
			var param = doc.CreateElement("Param");
			param.SetAttribute("VirtualKey", "0");
			param.SetAttribute("LParam", "0");
			param.SetAttribute("Modifier", "0");
			param.SetAttribute("DisplayName", "");
			handler.AppendChild(param);
			for (int i = 0; i < count; i++)
			{
				el.AppendChild(handler.Clone());
			}
			return el;
		}
	}
}
