using System;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Server.PanelServer
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class PanelServerDeviceSettings : AbstractPanelServerDeviceSettings
	{
		private const string FACTORY_NAME = "PanelServer";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		public override Type OriginatorType { get { return typeof(PanelServerDevice); } }
	}
}
