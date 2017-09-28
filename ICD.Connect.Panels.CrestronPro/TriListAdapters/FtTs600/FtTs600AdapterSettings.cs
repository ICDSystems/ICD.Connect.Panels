using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.FtTs600
{
	public sealed class FtTs600AdapterSettings : AbstractFt5ButtonAdapterSettings
	{
		private const string FACTORY_NAME = "FtTs600";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(FtTs600Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlFactoryMethod(FACTORY_NAME)]
		public static FtTs600AdapterSettings FromXml(string xml)
		{
			FtTs600AdapterSettings output = new FtTs600AdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}