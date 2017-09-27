using System;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes.Factories;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public sealed class Dge1Adapter : AbstractDge1BaseAdapter<Dge1, Dge1AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Dge1 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Dge1(ipid, controlSystem);
		}
	}
#else
	public sealed class Dge1Adapter : AbstractDge1BaseAdapter<Dge1AdapterSettings>
	{
	}
#endif

	public sealed class Dge1AdapterSettings : AbstractDge1BaseAdapterSettings
	{
		private const string FACTORY_NAME = "Dge1";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Dge1Adapter); } }

		/// <summary>
		/// Loads the settings from XML.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		[PublicAPI, XmlPanelSettingsFactoryMethod(FACTORY_NAME)]
		public static Dge1AdapterSettings FromXml(string xml)
		{
			Dge1AdapterSettings output = new Dge1AdapterSettings();
			ParseXml(output, xml);
			return output;
		}
	}
}
