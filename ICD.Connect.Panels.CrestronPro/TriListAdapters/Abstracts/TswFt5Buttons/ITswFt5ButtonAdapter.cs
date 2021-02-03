using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons.Telemetry;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons
{
	public interface ITswFt5ButtonAdapter : ITriListAdapter, ICrestronEthernetDeviceAdapter
	{
		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.APP_MODE_EVENT)]
		event EventHandler<StringEventArgs> OnAppModeChanged;

		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.DISPLAY_PROJECT_EVENT)]
		event EventHandler<StringEventArgs> OnDisplayProjectChanged;

		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.APP_MODE_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.APP_MODE_EVENT)]
		string AppMode { get; }

		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.DISPLAY_PROJECT_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.DISPLAY_PROJECT_EVENT)]
		string DisplayProject { get; }
	}
}