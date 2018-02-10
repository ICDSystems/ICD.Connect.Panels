using System;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Server.Osd
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class OsdPanelDeviceSettings : AbstractPanelServerDeviceSettings
	{
		private const string FACTORY_NAME = "OsdPanel";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		public override Type OriginatorType { get { return typeof(OsdPanelDevice); } }
	}
}
