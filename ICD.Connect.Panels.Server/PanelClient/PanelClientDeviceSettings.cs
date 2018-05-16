using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Settings.Attributes;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Panels.Server.PanelClient
{
	[KrangSettings("PanelClient", typeof(PanelClientDevice))]
	public sealed class PanelClientDeviceSettings : AbstractDeviceSettings
	{
		private const string ADDRESS_ELEMENT = "Address";
		private const string PORT_ELEMENT = "Port";
		private const string PANEL_ELEMENT = "Panel";

		public string Address { get; set; }

		public ushort Port { get; set; }

		[OriginatorIdSettingsProperty(typeof(IPanelDevice))]
		public int? Panel { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(ADDRESS_ELEMENT, Address);
			writer.WriteElementString(PORT_ELEMENT, IcdXmlConvert.ToString(Port));
			writer.WriteElementString(PANEL_ELEMENT, IcdXmlConvert.ToString(Panel));
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Address = XmlUtils.TryReadChildElementContentAsString(xml, ADDRESS_ELEMENT);
			Port = XmlUtils.TryReadChildElementContentAsUShort(xml, PORT_ELEMENT) ?? 0;
			Panel = XmlUtils.TryReadChildElementContentAsInt(xml, PANEL_ELEMENT);
		}
	}
}
