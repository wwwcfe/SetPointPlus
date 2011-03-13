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
			this.Id = id;
			this.Name = GetDeviceName(document, id);
			this.FilePath = path;
			this.Document = document;
		}

		/// <summary>
		/// デバイス名を document から取得します。
		/// </summary>
		/// <param name="document">分析する XDocument。</param>
		/// <param name="id">デバッグ用の ID 文字列。</param>
		/// <returns></returns>
		private static string GetDeviceName(XDocument document, string id)
		{
			if (document == null)
				throw new NullReferenceException("document, DevID: " + id);
			if (document.Root == null)
				throw new NullReferenceException("document.Root, DevID: " + id);
			var devices = document.Root.Element("Devices");
			if (devices == null)
				throw new NullReferenceException("devices, DevID: " + id);

			var device = devices.Element("Device");
			if (device == null)
				throw new NullReferenceException("device, DevID: " + id);

			var attribute = device.Attribute("DisplayName");
			if (attribute == null)
				throw new NullReferenceException("attribute, DevID: " + id);

			var value = attribute.Value;
			if (string.IsNullOrEmpty(value))
				throw new NullReferenceException("value, DevID: " + id);

			return value;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Id);
		}
	}
}
