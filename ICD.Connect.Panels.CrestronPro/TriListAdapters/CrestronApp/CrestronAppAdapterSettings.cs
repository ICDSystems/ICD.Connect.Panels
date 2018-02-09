using System;
using ICD.Common.Utils.Xml;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.CrestronApp
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class CrestronAppAdapterSettings : AbstractTriListAdapterSettings
	{
		private const string PROJECT_NAME_ELEMENT = "ProjectName";

		private const string FACTORY_NAME = "CrestronApp";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(CrestronAppAdapter); } }

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
