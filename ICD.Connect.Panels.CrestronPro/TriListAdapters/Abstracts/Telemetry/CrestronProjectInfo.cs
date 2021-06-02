using System;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
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
		public event EventHandler<StringEventArgs> OnHostNameChanged;

		#endregion

		#region Fields

		private CrestronEthernetDeviceAdapterNetworkInfo? m_NetworkInfo;
		private CrestronEthernetDeviceAdapterVersionInfo? m_VersionInfo;
		private CrestronEthernetDeviceAdapterProjectInfo? m_ProjectInfo;
		private string m_AppMode;
		private string m_HostName;

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

		public string HostName
		{
			get { return m_HostName; }
			private set
			{
				if (m_HostName == value)
					return;

				m_HostName = value;
				OnHostNameChanged.Raise(this, m_HostName);
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
		public void UpdateInfo()
		{
			UpdateInfo(EnumUtils.GetFlagsExceptNone<eCrestronProjectInfoUpdateComponents>().Aggregate((c, n) => c | n));
		}

		/// <summary>
		/// Queries the parent adapter based on the set flag.
		/// </summary>
		/// <param name="flag"></param>
		public void UpdateInfo(eCrestronProjectInfoUpdateComponents flag)
		{
			if (flag.HasFlag(eCrestronProjectInfoUpdateComponents.NetworkInfo))
				CrestronEthernetDeviceUtils.UpdateNetworkInfo(m_ParentAdapter, n => NetworkInfo = n);

			if (flag.HasFlag(eCrestronProjectInfoUpdateComponents.VersionInfo))
				CrestronEthernetDeviceUtils.UpdateVersionInfo(m_ParentAdapter, v => VersionInfo = v);

			if (flag.HasFlag(eCrestronProjectInfoUpdateComponents.ProjectInfo))
				CrestronEthernetDeviceUtils.UpdateProjectInfo(m_ParentAdapter, p => ProjectInfo = p);

			if (flag.HasFlag(eCrestronProjectInfoUpdateComponents.AppMode))
				CrestronEthernetDeviceUtils.UpdateAppMode(m_ParentAdapter, a => AppMode = a);

			if (flag.HasFlag(eCrestronProjectInfoUpdateComponents.HostName))
				CrestronEthernetDeviceUtils.UpdateHostName(m_ParentAdapter, h => HostName = h);
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public void InitializeTelemetry()
		{
		}

		#endregion
	}

	[Flags]
	public enum eCrestronProjectInfoUpdateComponents
	{
		None = 0,
		NetworkInfo = 1,
		VersionInfo = 2,
		ProjectInfo = 4,
		AppMode = 8,
		HostName = 16
	}
}