using System;
using System.Xml.Linq;

namespace SetPointPlus
{
	class DeviceInfo
	{
		public string Name { get; private set; }
		public string Id { get; private set; }
		public string FilePath { get; private set; }
		public XDocument Document { get; private set; }

		public DeviceInfo(XDocument document, string id, string path)
		{
			//this.Name = name;
			this.Name = GetDeviceName(document);
			this.Id = id;
			this.FilePath = path;
			this.Document = document;
		}

		private static string GetDeviceName(XDocument document)
		{
			var device = document.Root.Element("Devices").Element("Device");
			return device.Attribute("DisplayName").Value;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Id);
		}
	}
}
