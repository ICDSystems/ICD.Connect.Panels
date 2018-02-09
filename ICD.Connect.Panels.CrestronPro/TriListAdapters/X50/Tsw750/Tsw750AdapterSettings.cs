using System;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw750
{
	/// <summary>
	/// Settings for the Tsw750Adapter panel device.
	/// </summary>
	[KrangSettings(FACTORY_NAME)]
	public sealed class Tsw750AdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw750";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw750Adapter); } }
	}
}
