using ICD.Connect.Panels.Server;
using ICD.Connect.Protocol.NetworkPro.Ports.WebSockets;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.ServerPro.Devices
{
	public sealed class WebPanelDevice : AbstractPanelServerDevice<IcdWebSocketServer, WebPanelDeviceSettings>
	{
	}

	[KrangSettings("WebPanelDevice", typeof(WebPanelDevice))]
	public sealed class WebPanelDeviceSettings : AbstractPanelServerDeviceSettings
	{
	}
}
