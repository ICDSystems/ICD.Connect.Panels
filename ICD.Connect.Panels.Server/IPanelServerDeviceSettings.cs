using ICD.Connect.Panels.Devices;

namespace ICD.Connect.Panels.Server
{
	public interface IPanelServerDeviceSettings : IPanelDeviceSettings
	{
		ushort Port { get; set; }
	}
}
