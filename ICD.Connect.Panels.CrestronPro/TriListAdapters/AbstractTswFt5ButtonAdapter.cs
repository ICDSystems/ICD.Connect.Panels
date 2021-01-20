using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Timers;
using ICD.Common.Utils.Xml;
using ICD.Connect.API.Nodes;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Misc.CrestronPro.Utils;
using ICD.Connect.Panels.Controls.Backlight;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	#region Abstract Adapter

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

		public event EventHandler<StringEventArgs> OnTsidChanged;

		#endregion

		#region Constants

		private const int VOIP_DIALER_CONTROL_ID = 1;
		private const int BACKLIGHT_CONTROL_ID = 2;
		protected const int HARD_BUTTON_CONTROL_ID = 3;

		#endregion

		#region Fields

		[UsedImplicitly]
		private SafeTimer m_ProjectInfoUpdateTimer;
		private CrestronProjectInfo m_ProjectInfo;
		private string m_Tsid;

		// Used with settings
		private bool m_EnableVoip;
		private readonly SecureNetworkProperties m_NetworkProperties;

		#endregion

		#region Properties

		public CrestronProjectInfo ProjectInfo
		{
			get { return m_ProjectInfo; }
			private set
			{
				if (m_ProjectInfo == value)
					return;

				Unsubscribe(m_ProjectInfo);
				m_ProjectInfo = value;
				Subscribe(m_ProjectInfo);
			}
		}

		public string Tsid
		{
			get { return m_Tsid; }
			private set
			{
				if (m_Tsid == value)
					return;

				m_Tsid = value;
				OnTsidChanged.Raise(this, m_Tsid);
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

			MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).Ipv4Address = panel == null ? string.Empty : panel.ExtenderEthernetReservedSigs.IpAddressFeedback.StringValue;
			MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).MacAddress = panel == null ? string.Empty : panel.ExtenderEthernetReservedSigs.MacAddressFeedback.StringValue;
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
							MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).Ipv4Address = args.Sig.StringValue;
							break;
						case 17309:
							MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).MacAddress = args.Sig.StringValue;
							break;
					}
					break;
				case eSigEvent.StringOutputSigsCleared:
					MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).Ipv4Address = null;
					MonitoredDeviceInfo.NetworkInfo.GetOrAddAdapter(1).MacAddress = null;
					break;
			}
		}

		#endregion
#endif

		#region ProjectInfo Callbacks

		private void InitializeProjectInfoPolling()
		{
			// Do the first poll in 1 minute, after that poll after every 10 minutes.
			m_ProjectInfoUpdateTimer = new SafeTimer(() => m_ProjectInfo.UpdateAllInfo(), 10 * 60000);
		}

		private void Subscribe(ICrestronProjectInfo projectInfo)
		{
			if (projectInfo == null)
				return;

			projectInfo.OnNetworkInfoChanged += ProjectInfoOnNetworkInfoChanged;
			projectInfo.OnVersionInfoChanged += ProjectInfoOnVersionInfoChanged;
		}

		private void Unsubscribe(ICrestronProjectInfo projectInfo)
		{
			if (projectInfo == null)
				return;

			projectInfo.OnNetworkInfoChanged -= ProjectInfoOnNetworkInfoChanged;
			projectInfo.OnVersionInfoChanged -= ProjectInfoOnVersionInfoChanged;
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
			Tsid = args.Data.HasValue ? args.Data.Value.Tsid : null;
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

#if SIMPLSHARP
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
#endif

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

		#endregion
	}

	#endregion

	#region Settings Abstract

	public abstract class AbstractTswFt5ButtonAdapterSettings : AbstractTriListAdapterSettings,
	                                                            ITswFt5ButtonAdapterSettings
	{
		#region Constants

		private const string ENABLE_VOIP_ELEMENT = "EnableVoIP";

		#endregion

		#region Fields

		private readonly SecureNetworkProperties m_NetworkProperties;

		#endregion

		#region Properties

		public bool EnableVoip { get; set; }

		#endregion

		#region Network

		/// <summary>
		/// Gets/sets the configurable network username.
		/// </summary>
		public string NetworkUsername
		{
			get { return m_NetworkProperties.NetworkUsername; }
			set { m_NetworkProperties.NetworkUsername = value; }
		}

		/// <summary>
		/// Gets/sets the configurable network password.
		/// </summary>
		public string NetworkPassword
		{
			get { return m_NetworkProperties.NetworkPassword; }
			set { m_NetworkProperties.NetworkPassword = value; }
		}

		/// <summary>
		/// Gets/sets the configurable network address.
		/// </summary>
		public string NetworkAddress
		{
			get { return m_NetworkProperties.NetworkAddress; }
			set { m_NetworkProperties.NetworkAddress = value; }
		}

		/// <summary>
		/// Gets/sets the configurable network port.
		/// </summary>
		public ushort? NetworkPort
		{
			get { return m_NetworkProperties.NetworkPort; }
			set { m_NetworkProperties.NetworkPort = value; }
		}

		/// <summary>
		/// Clears the configured values.
		/// </summary>
		void INetworkProperties.ClearNetworkProperties()
		{
			m_NetworkProperties.ClearNetworkProperties();
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractTswFt5ButtonAdapterSettings()
		{
			m_NetworkProperties = new SecureNetworkProperties();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(ENABLE_VOIP_ELEMENT, IcdXmlConvert.ToString(EnableVoip));

			m_NetworkProperties.WriteElements(writer);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			EnableVoip = XmlUtils.TryReadChildElementContentAsBoolean(xml, ENABLE_VOIP_ELEMENT) ?? false;

			m_NetworkProperties.ParseXml(xml);
		}

		#endregion
	}

	#endregion

	#region Interfaces

	public interface ITswFt5ButtonAdapter : ITriListAdapter, ICrestronEthernetDeviceAdapter
	{
		[EventTelemetry(TswFt5ButtonAdapterTelemetryNames.TSID_EVENT)]
		event EventHandler<StringEventArgs> OnTsidChanged;

		[NodeTelemetry("ProjectInfo")]
		CrestronProjectInfo ProjectInfo { get; }

		[PropertyTelemetry(TswFt5ButtonAdapterTelemetryNames.TSID_PROPERTY, null, TswFt5ButtonAdapterTelemetryNames.TSID_EVENT)]
		string Tsid { get; }
	}

	public interface ITswFt5ButtonAdapterSettings : ITriListAdapterSettings, ICrestronEthernetDeviceAdapterSettings
	{
		bool EnableVoip { get; set; }
	}

	#endregion

	#region Telemetry

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

	public static class TswFt5ButtonAdapterTelemetryNames
	{
		public const string TSID_PROPERTY = "Tsid";
		public const string TSID_EVENT = "OnTsidChanged";

		public const string NETWORK_INFO_PROPERTY = "NetworkInfo";
		public const string NETWORK_INFO_EVENT = "OnNetworkInfoChanged";

		public const string VERSION_INFO_PROPERTY = "VersionInfo";
		public const string VERSION_INFO_EVENT = "OnVersionInfoChanged";

		public const string PROJECT_INFO_PROPERTY = "ProjectInfo";
		public const string PROJECT_INFO_EVENT = "OnProjectInfoChanged";

		public const string APP_MODE_PROPERTY = "AppMode";
		public const string APP_MODE_EVENT = "OnAppModeChanged";
	}

	#endregion
}
