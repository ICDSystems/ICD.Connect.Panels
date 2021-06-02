﻿using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Timers;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Telemetry;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings;
#if SIMPLSHARP
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Panels.Controls.Backlight;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons
{
#if SIMPLSHARP
	public abstract class AbstractTswFt5ButtonAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>,
	                                                                       ITswFt5ButtonAdapter
		where TPanel : TswFt5Button
#else
	public abstract class AbstractTswFt5ButtonAdapter<TSettings> : AbstractTriListAdapter<TSettings>, ITswFt5ButtonAdapter
#endif
		where TSettings : ITswFt5ButtonAdapterSettings, new()
	{
		#region Events

		public event EventHandler<StringEventArgs> OnAppModeChanged;
		public event EventHandler<StringEventArgs> OnDisplayProjectChanged;

		#endregion

		#region Constants

		private const string MONITORED_DEVICE_INFO_MAKE = "Crestron";
		private const int VOIP_DIALER_CONTROL_ID = 1;
		private const int BACKLIGHT_CONTROL_ID = 2;
		protected const int HARD_BUTTON_CONTROL_ID = 3;

		#endregion

		#region Fields

		[UsedImplicitly]
		private SafeTimer m_ProjectInfoUpdateTimer;
		private CrestronProjectInfo m_ProjectInfo;
		private string m_AppMode;
		private string m_DisplayProject;

		// Used with settings
		private bool m_EnableVoip;
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

		public SecureNetworkProperties NetworkProperties { get { return m_NetworkProperties; } }

		#endregion

		#region Constructor.

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractTswFt5ButtonAdapter()
		{
			m_NetworkProperties = new SecureNetworkProperties();
		}

		#endregion

#if SIMPLSHARP

		#region Panel Callbacks

		/// <summary>
		/// Sets the wrapped panel device.
		/// </summary>
		/// <param name="panel"></param>
		public override void SetPanel(TPanel panel)
		{
			base.SetPanel(panel);

			UpdateNetworkInfo();
		}

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		/// <param name="panel"></param>
		protected override void Subscribe(TPanel panel)
		{
			base.Subscribe(panel);

			if (panel == null)
				return;

			panel.ExtenderEthernetReservedSigs.DeviceExtenderSigChange += ExtenderEthernetReservedSigsOnDeviceExtenderSigChange;
		}

		/// <summary>
		/// Subscribe to the TriList events.
		/// </summary>
		/// <param name="panel"></param>
		protected override void Unsubscribe(TPanel panel)
		{
			base.Unsubscribe(panel);

			if (panel == null)
				return;

			panel.ExtenderEthernetReservedSigs.DeviceExtenderSigChange -= ExtenderEthernetReservedSigsOnDeviceExtenderSigChange;
		}

		private void ExtenderEthernetReservedSigsOnDeviceExtenderSigChange(DeviceExtender currentDeviceExtender, SigEventArgs args)
		{
			switch (args.Event)
			{
				case eSigEvent.StringChange:
					switch (args.Sig.Number)
					{
						case 17300:
						case 17309:
							UpdateNetworkInfo();
							break;
					}
					break;
				case eSigEvent.StringOutputSigsCleared:
					MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).Ipv4Address = null;
					MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).MacAddress = null;
					break;
			}
		}

		private void UpdateNetworkInfo()
		{
			string macAddressFeedback =
				Panel == null ? string.Empty : Panel.ExtenderEthernetReservedSigs.MacAddressFeedback.StringValue;

			IcdPhysicalAddress mac;
			IcdPhysicalAddress.TryParse(macAddressFeedback, out mac);

			MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).Ipv4Address = Panel == null ? string.Empty : Panel.ExtenderEthernetReservedSigs.IpAddressFeedback.StringValue;
			MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).MacAddress = mac;
		}

		#endregion
#endif

		#region ProjectInfo Callbacks

		private void InitializeProjectInfoPolling()
		{
			// Only poll if there is a configured username.
			// Do the first poll immediately, after that poll after every 120 minutes.
			// TODO - see if polling flag can be set here.
			if (!string.IsNullOrEmpty(NetworkProperties.NetworkUsername))
				m_ProjectInfoUpdateTimer = new SafeTimer(() => ProjectInfo.UpdateInfo(), 120 * 60000);

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
			projectInfo.OnAppModeChanged += ProjectInfoOnAppModeChanged;
			projectInfo.OnHostNameChanged += ProjectInfoOnHostNameChanged;
		}

		private void Unsubscribe(ICrestronProjectInfo projectInfo)
		{
			if (projectInfo == null)
				return;

			projectInfo.OnNetworkInfoChanged -= ProjectInfoOnNetworkInfoChanged;
			projectInfo.OnVersionInfoChanged -= ProjectInfoOnVersionInfoChanged;
			projectInfo.OnProjectInfoChanged -= ProjectInfoOnProjectInfoChanged;
			projectInfo.OnAppModeChanged -= ProjectInfoOnAppModeChanged;
			projectInfo.OnHostNameChanged -= ProjectInfoOnHostNameChanged;
		}

		private void ProjectInfoOnNetworkInfoChanged(object sender, GenericEventArgs<CrestronEthernetDeviceAdapterNetworkInfo?> args)
		{
			IcdPhysicalAddress mac = args.Data == null ? null : args.Data.Value.MacAddress;

			// Update device information.
			MonitoredDeviceInfo.NetworkInfo.Dns = args.Data.HasValue ? args.Data.Value.DnsServer : null;
			MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).Ipv4Address = args.Data.HasValue ? args.Data.Value.IpAddress : null;
			MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).Dhcp = args.Data.HasValue && args.Data.Value.Dhcp;
			MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).Ipv4Gateway = args.Data.HasValue ? args.Data.Value.DefaultGateway : null;
			MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).Ipv4SubnetMask = args.Data.HasValue ? args.Data.Value.SubnetMask : null;
			MonitoredDeviceInfo.NetworkInfo.Adapters.GetOrAddAdapter(1).MacAddress = mac == null ? null : mac.Clone();
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

		private void ProjectInfoOnAppModeChanged(object sender, StringEventArgs args)
		{
			AppMode = args.Data;
		}

		private void ProjectInfoOnHostNameChanged(object sender, StringEventArgs args)
		{
			MonitoredDeviceInfo.NetworkInfo.Hostname = args.Data;
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			m_EnableVoip = false;

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

			settings.EnableVoip = m_EnableVoip;
			settings.Copy(m_NetworkProperties);
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			// Set this value before applying the rest of the settings and registering the panel
			m_EnableVoip = settings.EnableVoip;

			base.ApplySettingsFinal(settings, factory);

			m_NetworkProperties.Copy(settings);
		}

#if SIMPLSHARP
		/// <summary>
		/// Override to add controls to the device.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		/// <param name="addControl"></param>
		protected override void AddControls(TSettings settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
			base.AddControls(settings, factory, addControl);

			addControl(InstantiateBacklightControl(BACKLIGHT_CONTROL_ID));

			if (m_EnableVoip)
				addControl(InstantiateDialingControl(VOIP_DIALER_CONTROL_ID));
		}

		/// <summary>
		/// Called before registration.
		/// Override to control which extenders are used with the panel.
		/// </summary>
		/// <param name="panel"></param>
		protected override void RegisterExtenders(TPanel panel)
		{
			base.RegisterExtenders(panel);

			if (panel == null)
				return;

			RegisterSystemExtender(panel);
			RegisterEthernetExtender(panel);

			if (m_EnableVoip)
				RegisterVoIpExtender(panel);
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

		/// <summary>
		/// Registers the system extender for the given panel.
		/// </summary>
		/// <param name="panel"></param>
		protected virtual void RegisterSystemExtender(TPanel panel)
		{
			panel.ExtenderSystemReservedSigs.Use();
		}

		/// <summary>
		/// Registers the VoIP extender for the given panel.
		/// </summary>
		/// <param name="panel"></param>
		protected virtual void RegisterVoIpExtender(TPanel panel)
		{
			panel.ExtenderVoipReservedSigs.Use();
		}

		protected virtual void RegisterEthernetExtender(TPanel panel)
		{
			panel.ExtenderEthernetReservedSigs.Use();
		}

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of conference control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected abstract ITraditionalConferenceDeviceControl InstantiateDialingControl(int id);

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of backlight control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected abstract IBacklightDeviceControl InstantiateBacklightControl(int id);
#endif

		#endregion

		#region Console

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("AppMode", AppMode);
			addRow("DisplayProject", DisplayProject);
		}

		#endregion
	}
}
