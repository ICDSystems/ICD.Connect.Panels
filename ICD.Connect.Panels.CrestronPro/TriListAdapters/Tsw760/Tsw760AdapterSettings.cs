using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes.Factories;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Tsw760
{
	public sealed class Tsw760AdapterSettings : AbstractTswFt5ButtonAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw760";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw760Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static Tsw760AdapterSettings FromXml(string xml)
		{
			Tsw760AdapterSettings output = new Tsw760AdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}