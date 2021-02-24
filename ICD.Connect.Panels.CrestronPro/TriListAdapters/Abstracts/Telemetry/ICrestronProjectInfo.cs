using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Telemetry
{
	public interface ICrestronProjectInfo
	{
		#region Events

		event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterNetworkInfo?>> OnNetworkInfoChanged;

		event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterVersionInfo?>> OnVersionInfoChanged;

		event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterProjectInfo?>> OnProjectInfoChanged;

		event EventHandler<StringEventArgs> OnAppModeChanged;

		#endregion

		#region Properties

		[CanBeNull]
		CrestronEthernetDeviceAdapterNetworkInfo? NetworkInfo { get; }

		[CanBeNull]
		CrestronEthernetDeviceAdapterVersionInfo? VersionInfo { get; }

		[CanBeNull]
		CrestronEthernetDeviceAdapterProjectInfo? ProjectInfo { get; }

		[CanBeNull]
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