using ICD.Connect.Protocol.Network.Ports.Tcp;
using ICD.Connect.Routing.Connections;
using ICD.Connect.Routing.Mock.Source;

namespace ICD.Connect.Panels.Server.Osd
{
    public sealed class OsdPanelDevice : AbstractPanelServerDevice<IcdTcpServer, OsdPanelDeviceSettings>
    {
		/// <summary>
		/// Constructor.
		/// </summary>
		public OsdPanelDevice()
		{
			MockRouteSourceControl routingControl = new MockRouteSourceControl(this, 0);
			routingControl.SetActiveTransmissionState(1, eConnectionType.Video, true);

			Controls.Add(routingControl);
		}
	}
}
