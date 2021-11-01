using ICD.Connect.Devices.Mock;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Mock
{
	[KrangSettings("MockHrRemoteDevice", typeof(MockHrRemoteDevice))]
	public sealed class MockHrRemoteDeviceSettings : AbstractMockDeviceSettings
	{
	}
}
