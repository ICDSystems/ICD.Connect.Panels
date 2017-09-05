using ICD.Common.Utils.Xml;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public abstract class AbstractTswFt5ButtonAdapterSettings : AbstractTriListAdapterSettings,
	                                                            ITswFt5ButtonAdapterSettings
	{
		private const string ENABLE_VOIP_ELEMENT = "EnableVoIP";

		public bool EnableVoIp { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(ENABLE_VOIP_ELEMENT, IcdXmlConvert.ToString(EnableVoIp));
		}

		protected static void ParseXml(AbstractTswFt5ButtonAdapterSettings instance, string xml)
		{
			instance.EnableVoIp = XmlUtils.TryReadChildElementContentAsBoolean(xml, ENABLE_VOIP_ELEMENT) ?? false;

			AbstractTriListAdapterSettings.ParseXml(instance, xml);
		}
	}
}
