using System;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Protocol.Network.Ports.Tcp;
using ICD.Connect.Routing.Connections;
using ICD.Connect.Routing.Mock.Source;
using ICD.Connect.Settings;

namespace ICD.Connect.Panels.Server.Osd
{
    public sealed class OsdPanelDevice : AbstractPanelServerDevice<IcdTcpServer, OsdPanelDeviceSettings>
    {
	    /// <summary>
	    /// Override to add controls to the device.
	    /// </summary>
	    /// <param name="settings"></param>
	    /// <param name="factory"></param>
	    /// <param name="addControl"></param>
	    protected override void AddControls(OsdPanelDeviceSettings settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
			base.AddControls(settings, factory, addControl);

			MockRouteSourceControl routingControl = new MockRouteSourceControl(this, 0);
			routingControl.SetActiveTransmissionState(1, eConnectionType.Video, true);

			addControl(routingControl);
		}
	}
}
