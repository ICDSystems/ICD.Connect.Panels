using System;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw760Nc
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class Tsw760NcAdapterSettings : AbstractTswX60BaseClassAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw760Nc";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw760NcAdapter); } }
	}
}
