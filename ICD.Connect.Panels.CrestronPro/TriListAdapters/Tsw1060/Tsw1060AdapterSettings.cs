using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes.Factories;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Tsw1060
{
	public sealed class Tsw1060AdapterSettings : AbstractTswFt5ButtonAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw1060";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw1060Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static Tsw1060AdapterSettings FromXml(string xml)
		{
			Tsw1060AdapterSettings output = new Tsw1060AdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}