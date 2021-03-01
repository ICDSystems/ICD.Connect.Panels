using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.Ethernet;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.Telemetry;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Panels.Crestron.Devices.Dge
{
	public interface IDgeX00Adapter : IPanelDevice, ICrestronEthernetDeviceAdapter
	{
		[EventTelemetry(CrestronPanelTelemetryNames.DISPLAY_PROJECT_EVENT)]
		event EventHandler<StringEventArgs> OnDisplayProjectChanged;

		[PropertyTelemetry(CrestronPanelTelemetryNames.DISPLAY_PROJECT_PROPERTY, null, CrestronPanelTelemetryNames.DISPLAY_PROJECT_EVENT)]
		string DisplayProject { get; }
	}
}