using ICD.Common.Utils.Xml;

namespace ICD.Connect.Panels.Server
{
	public abstract class AbstractPanelServerDeviceSettings : AbstractPanelDeviceSettings, IPanelServerDeviceSettings
	{
		private const string PORT_ELEMENT = "Port";

		public ushort Port { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(PORT_ELEMENT, IcdXmlConvert.ToString(Port));
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Port = XmlUtils.TryReadChildElementContentAsUShort(xml, PORT_ELEMENT) ?? 0;
		}
	}
}
