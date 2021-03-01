using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Timers;
using ICD.Connect.API.Nodes;
using ICD.Connect.Misc.CrestronPro.Devices.Ethernet;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Telemetry;
using ICD.Connect.Protocol.Network.Settings;
using ICD.Connect.Settings;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DM;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Misc.CrestronPro.Devices;
#else
using ICD.Connect.Panels.Crestron.Devices.Dge;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge
{
#if SIMPLSHARP
	public abstract class AbstractDgeX00Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, IDgeX00Adapter<TPanel>
		where TPanel : Dge100
#else
	public abstract class AbstractDgeX00Adapter<TSettings> : AbstractTriListAdapter<TSettings>, IDgeX00Adapter
#endif
		where TSettings : IDgeX00AdapterSettings, new()
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
#if SIMPLSHARP
		public TPanel Dge { get { return Panel; } }
#endif

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
		protected AbstractDgeX00Adapter()
		{
			m_NetworkProperties = new SecureNetworkProperties();
		}

		#endregion

		#region Port Parent


#if SIMPLSHARP
		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public ComPort GetComPort(int address)
		{
			if (Dge == null)
				throw new InvalidOperationException("No device instantiated");

			if (address >= 1 && address <= Dge.NumberOfComPorts)
				return Dge.ComPorts[(uint)address];

			string message = string.Format("No {0} at address {1}", typeof(ComPort).Name, address);
			throw new InvalidOperationException(message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public IROutputPort GetIrOutputPort(int address)
		{
			if (Dge == null)
				throw new InvalidOperationException("No device instantiated");

			if (address >= 1 && address <= Dge.NumberOfIROutputPorts)
				return Dge.IROutputPorts[(uint)address];

			string message = string.Format("No {0} at address {1}", typeof(IROutputPort).Name, address);
			throw new InvalidOperationException(message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Relay GetRelayPort(int address)
		{
			string message = string.Format("{0} has no {1}", this, typeof(Relay).Name);
			throw new ArgumentOutOfRangeException("address", message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Versiport GetIoPort(int address)
		{
			string message = string.Format("{0} has no {1}", this, typeof(Versiport).Name);
			throw new ArgumentOutOfRangeException("address", message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public DigitalInput GetDigitalInputPort(int address)
		{
			string message = string.Format("{0} has no {1}", this, typeof(DigitalInput).Name);
			throw new ArgumentOutOfRangeException("address", message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="io"></param>
		/// <param name="address"></param>
		/// <returns></returns>
		public virtual Cec GetCecPort(eInputOuptut io, int address)
		{

			if (Dge == null)
				throw new InvalidOperationException("No device instantiated");

			if (io == eInputOuptut.Output && address == 1)
				return Dge.HdmiOut.StreamCec;
			if (io == eInputOuptut.Input)
			{
				switch (address)
				{
					case 4:
						return Dge.HdmiIn.StreamCec;
				}
			}

			string message = string.Format("No CecPort at address {1}:{2} for device {0}", this, io, address);
			throw new InvalidOperationException(message);
		}

#endif

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
