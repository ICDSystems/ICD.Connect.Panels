using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw1050
{
	public sealed class Tsw1050AdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw1050";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw1050Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlFactoryMethod(FACTORY_NAME)]
		public static Tsw1050AdapterSettings FromXml(string xml)
		{
			Tsw1050AdapterSettings output = new Tsw1050AdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}
