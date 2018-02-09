using System;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw1060
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class Tsw1060AdapterSettings : AbstractTswX60BaseClassAdapterSettings
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
	}
}
