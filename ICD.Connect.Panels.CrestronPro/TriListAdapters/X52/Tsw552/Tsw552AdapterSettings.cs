using System;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52.Tsw552
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class Tsw552AdapterSettings : AbstractTswX52ButtonVoiceControlAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw552";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw552Adapter); } }
	}
}
