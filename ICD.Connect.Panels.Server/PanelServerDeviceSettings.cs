using ICD.Common.Properties;
using ICD.Common.Utils.Xml;
using ICD.Connect.Settings;
using ICD.Connect.Settings.Attributes.Factories;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Panels.Server
{
	public sealed class PanelServerDeviceSettings : AbstractPanelDeviceSettings
	{
		private const string FACTORY_NAME = "PanelServer";

		private const string PORT_ELEMENT = "Port";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

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
		/// Creates a new originator instance from the settings.
		/// </summary>
		/// <param name="factory"></param>
		/// <returns></returns>
		public override IOriginator ToOriginator(IDeviceFactory factory)
		{
			PanelServerDevice output = new PanelServerDevice();
			output.ApplySettings(this, factory);
			return output;
		}

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static PanelServerDeviceSettings FromXml(string xml)
		{
			ushort port = XmlUtils.ReadChildElementContentAsUShort(xml, PORT_ELEMENT);

			PanelServerDeviceSettings output = new PanelServerDeviceSettings
			{
				Port = port
			};

			ParseXml(output, xml);
			return output;
		}
	}
}
