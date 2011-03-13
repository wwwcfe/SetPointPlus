using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace SetPointPlus
{
	class DeviceInfo
	{
		public string Name { get; private set; }
		public string Id { get; private set; }
		public string FilePath { get; private set; }
		public XDocument Document { get; private set; }

		public DeviceInfo(string name, string id, string path, XDocument document)
		{
			this.Name = name;
			this.Id = id;
			this.FilePath = path;
			this.Document = document;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Id);
		}

		//public static DeviceInfo LoadFromXml(string filename)
		//{
		//    // TODO: XML 情報を保持するようにする
		//    XDocument doc = XDocument.Load(filename);
		//    XElement device = doc.Root.Element("Device");
		//    string deviceName = device.Attribute("DisplayName").Value;

		//    //XElement el = XElement.Load(filename).Element("Devices").Element("Device");
		//    //string deviceName = el.Attribute("DisplayName").Value;
		//}
	}
}
