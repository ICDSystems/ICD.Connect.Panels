using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw550
{
	/// <summary>
	/// Settings for the Tsw550Adapter panel device.
	/// </summary>
	public sealed class Tsw550AdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw550";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw550Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlFactoryMethod(FACTORY_NAME)]
		public static Tsw550AdapterSettings FromXml(string xml)
		{
			Tsw550AdapterSettings output = new Tsw550AdapterSettings();
			output.ParseXml(xml);
			return output;
		}
	}
}
