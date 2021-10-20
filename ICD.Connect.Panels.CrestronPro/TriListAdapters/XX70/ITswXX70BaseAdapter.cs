using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.Ethernet;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;
using ICD.Connect.Panels.Telemetry;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70
{
	public interface ITswXX70BaseAdapter : ITriListAdapter, ICrestronEthernetDeviceAdapter
	{
		[EventTelemetry(CrestronPanelTelemetryNames.APP_MODE_EVENT)]
		event EventHandler<StringEventArgs> OnAppModeChanged;

		[EventTelemetry(CrestronPanelTelemetryNames.DISPLAY_PROJECT_EVENT)]
		event EventHandler<StringEventArgs> OnDisplayProjectChanged;

		[PropertyTelemetry(CrestronPanelTelemetryNames.APP_MODE_PROPERTY, null, CrestronPanelTelemetryNames.APP_MODE_EVENT)]
		string AppMode { get; }

		[PropertyTelemetry(CrestronPanelTelemetryNames.DISPLAY_PROJECT_PROPERTY, null, CrestronPanelTelemetryNames.DISPLAY_PROJECT_EVENT)]
		string DisplayProject { get; }
	}
}