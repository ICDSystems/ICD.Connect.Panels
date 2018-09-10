using ICD.Connect.Panels.Devices;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Mock
{
	[KrangSettings("MockPanel", typeof(MockPanelDevice))]
	public sealed class MockPanelDeviceSettings : AbstractPanelDeviceSettings
	{
	}
}
