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
	}
}
