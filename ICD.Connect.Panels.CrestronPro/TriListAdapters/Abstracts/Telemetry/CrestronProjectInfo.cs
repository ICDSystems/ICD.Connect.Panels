using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.Ethernet;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Misc.CrestronPro.Utils;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Telemetry
{
	public sealed class CrestronProjectInfo : ICrestronProjectInfo
	{
		#region Events

		public event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterNetworkInfo?>> OnNetworkInfoChanged;
		public event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterVersionInfo?>> OnVersionInfoChanged;
		public event EventHandler<GenericEventArgs<CrestronEthernetDeviceAdapterProjectInfo?>> OnProjectInfoChanged;
		public event EventHandler<StringEventArgs> OnAppModeChanged;

		#endregion

		#region Fields

		private CrestronEthernetDeviceAdapterNetworkInfo? m_NetworkInfo;
		private CrestronEthernetDeviceAdapterVersionInfo? m_VersionInfo;
		private CrestronEthernetDeviceAdapterProjectInfo? m_ProjectInfo;
		private string m_AppMode;

		private readonly ICrestronEthernetDeviceAdapter m_ParentAdapter;

		#endregion

		#region Properties

		public CrestronEthernetDeviceAdapterNetworkInfo? NetworkInfo
		{
			get { return m_NetworkInfo; }
			private set
			{
				if (m_NetworkInfo == value)
					return;

				m_NetworkInfo = value;
				OnNetworkInfoChanged.Raise(this, m_NetworkInfo);
			}
		}

		public CrestronEthernetDeviceAdapterVersionInfo? VersionInfo
		{
			get { return m_VersionInfo; }
			private set
			{
				if (m_VersionInfo == value)
					return;

				m_VersionInfo = value;
				OnVersionInfoChanged.Raise(this, m_VersionInfo);
			}
		}

		public CrestronEthernetDeviceAdapterProjectInfo? ProjectInfo
		{
			get { return m_ProjectInfo; }
			private set
			{
				if (m_ProjectInfo == value)
					return;

				m_ProjectInfo = value;
				OnProjectInfoChanged.Raise(this, m_ProjectInfo);
			}
		}

		public string AppMode
		{
			get { return m_AppMode; }
			private set
			{
				if (m_AppMode == value)
					return;

				m_AppMode = value;
				OnAppModeChanged.Raise(this, m_AppMode);
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parentAdapter"></param>
		public CrestronProjectInfo(ICrestronEthernetDeviceAdapter parentAdapter)
		{
			m_ParentAdapter = parentAdapter;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Queries the parent adapter and updates this instance with the adapter's information.
		/// </summary>
		public void UpdateAllInfo()
		{
			CrestronEthernetDeviceUtils.UpdateNetworkInfo(m_ParentAdapter, n => NetworkInfo = n);
			CrestronEthernetDeviceUtils.UpdateVersionInfo(m_ParentAdapter, v => VersionInfo = v);
			CrestronEthernetDeviceUtils.UpdateProjectInfo(m_ParentAdapter, p => ProjectInfo = p);
			CrestronEthernetDeviceUtils.UpdateAppMode(m_ParentAdapter, a => AppMode = a);
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public void InitializeTelemetry()
		{
		}

		#endregion
	}
}