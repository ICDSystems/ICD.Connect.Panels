using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Timers;
using ICD.Connect.API.Nodes;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Telemetry;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Ts1542
{
#if SIMPLSHARP
	public abstract class AbstractTs1542Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>,
																	 ITs1542Adapter
		where TPanel : global::Crestron.SimplSharpPro.UI.Ts1542
#else
	public abstract class AbstractTs1542Adapter<TSettings> : AbstractTriListAdapter<TSettings>, ITs1542Adapter
#endif
		where TSettings : ITs1542AdapterSettings, new()
	{
		#region Events

		public event EventHandler<StringEventArgs> OnDisplayProjectChanged;

		#endregion

		#region Constants

		private const string MONITORED_DEVICE_INFO_MAKE = "Crestron";

		#endregion

		#region Fields

		[UsedImplicitly]
		private SafeTimer m_ProjectInfoUpdateTimer;
		private CrestronProjectInfo m_ProjectInfo;
		private string m_DisplayProject;
		private readonly SecureNetworkProperties m_NetworkProperties;

		#endregion

		#region Properties

		private CrestronProjectInfo ProjectInfo
		{
			get { return m_ProjectInfo; }
			set
			{
				if (m_ProjectInfo == value)
					return;

				Unsubscribe(m_ProjectInfo);
				m_ProjectInfo = value;
				Subscribe(m_ProjectInfo);
			}
		}

		public SecureNetworkProperties NetworkProperties { get { return m_NetworkProperties; } }

		public string DisplayProject
		{
			get { return m_DisplayProject; }
			private set
			{
				if (m_DisplayProject == value)
					return;

				m_DisplayProject = value;
				OnDisplayProjectChanged.Raise(this, m_DisplayProject);
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractTs1542Adapter()
		{
			m_NetworkProperties = new SecureNetworkProperties();
		}

		#endregion

		#region ProjectInfo Callbacks

		private void InitializeProjectInfoPolling()
		{
			// Do the first poll immediately, after that poll after every 10 minutes.
			// TODO - change polling frequency
			m_ProjectInfoUpdateTimer =
				new SafeTimer(() => ProjectInfo.UpdateInfo(eCrestronProjectInfoUpdateComponents.NetworkInfo |
				                                           eCrestronProjectInfoUpdateComponents.VersionInfo |
				                                           eCrestronProjectInfoUpdateComponents.ProjectInfo), 10 * 60000);

			// These are all Crestron panels
			MonitoredDeviceInfo.Make = MONITORED_DEVICE_INFO_MAKE;
		}

		private void Subscribe(ICrestronProjectInfo projectInfo)
		{
			if (projectInfo == null)
				return;

			projectInfo.OnNetworkInfoChanged += ProjectInfoOnNetworkInfoChanged;
			projectInfo.OnVersionInfoChanged += ProjectInfoOnVersionInfoChanged;
			projectInfo.OnProjectInfoChanged += ProjectInfoOnProjectInfoChanged;
		}

		private void Unsubscribe(ICrestronProjectInfo projectInfo)
		{
			if (projectInfo == null)
				return;

			projectInfo.OnNetworkInfoChanged -= ProjectInfoOnNetworkInfoChanged;
			projectInfo.OnVersionInfoChanged -= ProjectInfoOnVersionInfoChanged;
			projectInfo.OnProjectInfoChanged -= ProjectInfoOnProjectInfoChanged;
		}

		private void ProjectInfoOnNetworkInfoChanged(object sender, GenericEventArgs<CrestronEthernetDeviceAdapterNetworkInfo?> args)
		{
			// Update device information.
			MonitoredDeviceInfo.NetworkInfo.Dns = args.Data.HasValue ? args.Data.Value.DnsServer : null;
			MonitoredDeviceInfo.NetworkInfo.Hostname = args.Data.HasValue ? args.Data.Value.IpAddress : null;
			MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).Ipv4Address = args.Data.HasValue ? args.Data.Value.IpAddress : null;
			MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).Dhcp = args.Data.HasValue && args.Data.Value.Dhcp;
			MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).Ipv4Gateway = args.Data.HasValue ? args.Data.Value.DefaultGateway : null;
			MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).Ipv4SubnetMask = args.Data.HasValue ? args.Data.Value.SubnetMask : null;
			MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).MacAddress = args.Data.HasValue ? args.Data.Value.MacAddress : null;
		}

		private void ProjectInfoOnVersionInfoChanged(object sender, GenericEventArgs<CrestronEthernetDeviceAdapterVersionInfo?> args)
		{
			// Update device information.
			MonitoredDeviceInfo.Model = args.Data.HasValue ? args.Data.Value.Model : null;
			MonitoredDeviceInfo.SerialNumber = args.Data.HasValue ? args.Data.Value.SerialNumber : null;
			MonitoredDeviceInfo.FirmwareVersion = args.Data.HasValue ? args.Data.Value.FirmwareVersion : null;
			MonitoredDeviceInfo.FirmwareDate = args.Data.HasValue ? args.Data.Value.FirmwareDate : null;
		}

		private void ProjectInfoOnProjectInfoChanged(object sender, GenericEventArgs<CrestronEthernetDeviceAdapterProjectInfo?> args)
		{
			DisplayProject = args.Data.HasValue ? args.Data.Value.Vtz : null;
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			if (m_NetworkProperties != null)
				m_NetworkProperties.ClearNetworkProperties();
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.Copy(m_NetworkProperties);
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			m_NetworkProperties.Copy(settings);
		}

		/// <summary>
		/// Override to add actions on StartSettings
		/// This should be used to start communications with devices and perform initial actions
		/// </summary>
		protected override void StartSettingsFinal()
		{
			base.StartSettingsFinal();

			ProjectInfo = new CrestronProjectInfo(this);
			InitializeProjectInfoPolling();
		}

		#endregion

		#region Console

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("DisplayProject", DisplayProject);
		}

		#endregion
	}
}