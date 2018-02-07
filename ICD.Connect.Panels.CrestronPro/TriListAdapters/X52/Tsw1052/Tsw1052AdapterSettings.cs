using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52.Tsw1052
{
	public sealed class Tsw1052AdapterSettings : AbstractTswX52ButtonVoiceControlAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw1052";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw1052Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlFactoryMethod(FACTORY_NAME)]
		public static Tsw1052AdapterSettings FromXml(string xml)
		{
			Tsw1052AdapterSettings output = new Tsw1052AdapterSettings();
			output.ParseXml(xml);
			return output;
		}
	}
}
