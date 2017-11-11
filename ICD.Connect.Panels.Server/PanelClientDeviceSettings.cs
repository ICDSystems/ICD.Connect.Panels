using System;
using ICD.Common.Properties;
using ICD.Common.Utils.Xml;
using ICD.Connect.Devices;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Server
{
	public sealed class PanelClientDeviceSettings : AbstractDeviceSettings
	{
		private const string FACTORY_NAME = "PanelClient";

		private const string ADDRESS_ELEMENT = "Address";
		private const string PORT_ELEMENT = "Port";
		private const string PANEL_ELEMENT = "Panel";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		public override Type OriginatorType { get { return typeof(PanelClientDevice); } }

		public string Address { get; set; }

		public ushort Port { get; set; }

		[SettingsProperty(SettingsProperty.ePropertyType.PanelId)]
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
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlFactoryMethod(FACTORY_NAME)]
		public static PanelClientDeviceSettings FromXml(string xml)
		{
			string address = XmlUtils.TryReadChildElementContentAsString(xml, ADDRESS_ELEMENT);
			ushort port = XmlUtils.TryReadChildElementContentAsUShort(xml, PORT_ELEMENT) ?? 0;
			int? panel = XmlUtils.TryReadChildElementContentAsInt(xml, PANEL_ELEMENT);

			PanelClientDeviceSettings output = new PanelClientDeviceSettings
			{
				Address = address,
				Port = port,
				Panel = panel
			};

			ParseXml(output, xml);
			return output;
		}
	}
}