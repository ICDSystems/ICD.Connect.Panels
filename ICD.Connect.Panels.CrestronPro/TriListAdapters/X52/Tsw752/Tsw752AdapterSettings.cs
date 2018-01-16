using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52.Tsw752
{
	public sealed class Tsw752AdapterSettings : AbstractTswX52ButtonVoiceControlAdapterSettings
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
		[PublicAPI, XmlFactoryMethod(FACTORY_NAME)]
		public static Tsw752AdapterSettings FromXml(string xml)
		{
			Tsw752AdapterSettings output = new Tsw752AdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}
