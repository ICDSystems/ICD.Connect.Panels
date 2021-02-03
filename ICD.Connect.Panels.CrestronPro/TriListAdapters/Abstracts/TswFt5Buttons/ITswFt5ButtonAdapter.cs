using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons.Telemetry;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons
{
	public interface ITswFt5ButtonAdapter : ITriListAdapter, ICrestronEthernetDeviceAdapter
	{
		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.TSID_EVENT)]
		event EventHandler<StringEventArgs> OnTsidChanged;

		[NodeTelemetry("ProjectInfo")]
		CrestronProjectInfo ProjectInfo { get; }

		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.TSID_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.TSID_EVENT)]
		string Tsid { get; }
	}
}