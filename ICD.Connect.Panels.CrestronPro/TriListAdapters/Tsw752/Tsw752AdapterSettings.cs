using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes.Factories;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Tsw752
{
	public sealed class Tsw752AdapterSettings : AbstractTswFt5ButtonAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw752";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw752Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static Tsw752AdapterSettings FromXml(string xml)
		{
			Tsw752AdapterSettings output = new Tsw752AdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}