using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons.Telemetry
{
	public interface ICrestronProjectInfo : ITelemetryProvider
	{
		#region Events

		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.NETWORK_INFO_EVENT)]
		event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterNetworkInfo?>> OnNetworkInfoChanged;

		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.VERSION_INFO_EVENT)]
		event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterVersionInfo?>> OnVersionInfoChanged;

		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.PROJECT_INFO_EVENT)]
		event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterProjectInfo?>> OnProjectInfoChanged;

		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.APP_MODE_EVENT)]
		event EventHandler<StringEventArgs> OnAppModeChanged;

		#endregion

		#region Properties

		[CanBeNull]
		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.NETWORK_INFO_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.NETWORK_INFO_EVENT)]
		CrestronEthernetDeviceAdapterNetworkInfo? NetworkInfo { get; }

		[CanBeNull]
		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.VERSION_INFO_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.VERSION_INFO_EVENT)]
		CrestronEthernetDeviceAdapterVersionInfo? VersionInfo { get; }

		[CanBeNull]
		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.PROJECT_INFO_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.PROJECT_INFO_EVENT)]
		CrestronEthernetDeviceAdapterProjectInfo? ProjectInfo { get; }

		[CanBeNull]
		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.APP_MODE_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.APP_MODE_EVENT)]
		string AppMode { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Queries the parent adapter and updates this instance with the adapter's information.
		/// </summary>
		void UpdateAllInfo();

		#endregion
	}
}