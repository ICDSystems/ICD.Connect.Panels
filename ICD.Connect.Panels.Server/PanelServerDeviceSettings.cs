using System;
using ICD.Common.Utils.Xml;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Server
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class PanelServerDeviceSettings : AbstractPanelDeviceSettings
	{
		private const string FACTORY_NAME = "PanelServer";

		private const string PORT_ELEMENT = "Port";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		public override Type OriginatorType { get { return typeof(PanelServerDevice); } }

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
