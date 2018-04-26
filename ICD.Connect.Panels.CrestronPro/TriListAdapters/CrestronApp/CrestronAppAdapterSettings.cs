using ICD.Common.Utils.Xml;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.CrestronApp
{
	[KrangSettings("CrestronApp", typeof(CrestronAppAdapter))]
	public sealed class CrestronAppAdapterSettings : AbstractTriListAdapterSettings
	{
		private const string PROJECT_NAME_ELEMENT = "ProjectName";

		/// <summary>
		/// The name of the VTPro file without extension.
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(PROJECT_NAME_ELEMENT, ProjectName);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			ProjectName = XmlUtils.TryReadChildElementContentAsString(xml, PROJECT_NAME_ELEMENT);
		}
	}
}
