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

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Ipid = XmlUtils.ReadChildElementContentAsByte(xml, IPID_ELEMENT);
		}
	}
}
