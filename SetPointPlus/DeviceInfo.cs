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
            this.Id = id;
			this.Name = GetDeviceName(document);
			this.FilePath = path;
			this.Document = document;
		}

		private /* static */ string GetDeviceName(XDocument document)
		{
            if (document == null)
                throw new NullReferenceException("document, DevID: " + Id.ToString());
            if (document.Root == null)
                throw new NullReferenceException("document.Root, DevID: " + Id.ToString());
            var devices = document.Root.Element("Devices");
            if (devices == null)
                throw new NullReferenceException("devices, DevID: " + Id.ToString());

			var device = document.Root.Element("Devices").Element("Device");
            if (device == null)
                throw new NullReferenceException("device, DevID: " + Id.ToString());

            var attribute = device.Attribute("DisplayName");
            if (attribute == null)
                throw new NullReferenceException("attribute, DevID: " + Id.ToString());

            //if (attribute == null)
            //{
            //    attribute = device.Attribute("Class");
            //}

            var value = attribute.Value;
            if (string.IsNullOrEmpty(value))
                throw new NullReferenceException("value, DevID: " + Id.ToString());

			return device.Attribute("DisplayName").Value;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Id);
		}
	}
}
