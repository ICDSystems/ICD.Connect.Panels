using System;
using ICD.Common.Properties;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Mock
{
	[KrangSettings(FACTORY_NAME)]
	public sealed class MockPanelDeviceSettings : AbstractPanelDeviceSettings
	{
		private const string FACTORY_NAME = "MockPanel";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		public override Type OriginatorType
		{
			get { return typeof(MockPanelDevice); }
		}
	}
}
