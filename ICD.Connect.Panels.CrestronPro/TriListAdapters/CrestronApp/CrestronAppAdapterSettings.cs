using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes.Factories;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.CrestronApp
{
	public sealed class CrestronAppAdapterSettings : AbstractTriListAdapterSettings
	{
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
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static CrestronAppAdapterSettings FromXml(string xml)
		{
			CrestronAppAdapterSettings output = new CrestronAppAdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}
