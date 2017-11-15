using ICD.Common.Utils;
using ICD.Common.Utils.Xml;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public abstract class AbstractTriListAdapterSettings : AbstractPanelDeviceSettings, ITriListAdapterSettings
	{
		private const string IPID_ELEMENT = "IPID";

		[IpIdSettingsProperty]
		public byte Ipid { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(IPID_ELEMENT, StringUtils.ToIpIdString(Ipid));
		}

		protected static void ParseXml(AbstractTriListAdapterSettings instance, string xml)
		{
			instance.Ipid = XmlUtils.ReadChildElementContentAsByte(xml, IPID_ELEMENT);

			ParseXml((AbstractPanelDeviceSettings)instance, xml);
		}
	}
}
