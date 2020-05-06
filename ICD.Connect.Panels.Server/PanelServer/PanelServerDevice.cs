using ICD.Connect.Protocol.Network.Ports.Tcp;

namespace ICD.Connect.Panels.Server.PanelServer
{
	/// <summary>
	/// The PanelServerDevice wraps a TCPServer to emulate how existing Crestron panels work.
	/// </summary>
	public sealed class PanelServerDevice : AbstractPanelServerDevice<IcdTcpServer, PanelServerDeviceSettings>
	{
	}
}
