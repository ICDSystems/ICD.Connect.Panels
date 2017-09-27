using System;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes.Factories;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Ts1542
{
#if SIMPLSHARP
	public sealed class Ts1542CAdapter : AbstractTs1542Adapter<Crestron.SimplSharpPro.UI.Ts1542C, Ts1542CAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Ts1542C InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Ts1542C(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts1542CAdapter : AbstractTs1542Adapter<Ts1542CAdapterSettings>
	{
	}
#endif

	public sealed class Ts1542CAdapterSettings : AbstractTs1542AdapterSettings
	{
		private const string FACTORY_NAME = "Ts1542C";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Ts1542CAdapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static Ts1542CAdapterSettings FromXml(string xml)
		{
			Ts1542CAdapterSettings output = new Ts1542CAdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}
