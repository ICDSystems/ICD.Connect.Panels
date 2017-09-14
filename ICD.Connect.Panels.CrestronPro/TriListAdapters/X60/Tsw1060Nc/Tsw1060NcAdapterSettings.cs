using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes.Factories;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw1060Nc
{
	public sealed class Tsw1060NcAdapterSettings : AbstractTswX60BaseClassAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw1060Nc";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw1060NcAdapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static Tsw1060NcAdapterSettings FromXml(string xml)
		{
			Tsw1060NcAdapterSettings output = new Tsw1060NcAdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}