using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Xml;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings.Core;
using ICD.Connect.Telemetry.Attributes;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
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
		private const int VOIP_DIALER_CONTROL_ID = 1;
		private const int BACKLIGHT_CONTROL_ID = 2;
		protected const int HARD_BUTTON_CONTROL_ID = 3;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_IP_ADDRESS_CHANGED)]
		public event EventHandler<StringEventArgs> OnIpAddressChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_MAC_ADDRESS_CHANGED)]
		public event EventHandler<StringEventArgs> OnMacAddressChanged;

		private IDialingDeviceControl m_DialingControl;
		private readonly IPowerDeviceControl m_BacklightControl;

		// Used with settings
		private bool m_EnableVoip;

		private string m_IpAddress;
		private string m_MacAddress;

		#region Properties

		/// <summary>
		/// Gets the VoIp dialer for this panel.
		/// </summary>
		public IDialingDeviceControl VoipDialingControl { get { return m_DialingControl; } }

		/// <summary>
		/// Gets the backlight control for this panel.
		/// </summary>
		public IPowerDeviceControl BacklightControl { get { return m_BacklightControl; } }

		[DynamicPropertyTelemetry(DeviceTelemetryNames.DEVICE_IP_ADDRESS, DeviceTelemetryNames.DEVICE_IP_ADDRESS_CHANGED)]
		public string IpAddress
		{
			get { return m_IpAddress; }
			private set
			{
				if (m_IpAddress == value)
					return;

				m_IpAddress = value;

				OnIpAddressChanged.Raise(this, new StringEventArgs(value));
			}
		}

		[DynamicPropertyTelemetry(DeviceTelemetryNames.DEVICE_MAC_ADDRESS, DeviceTelemetryNames.DEVICE_MAC_ADDRESS_CHANGED)]
		public string MacAddress
		{
			get { return m_MacAddress; }
			private set
			{
				if (m_MacAddress == value)
					return;

				m_MacAddress = value;

				OnMacAddressChanged.Raise(this, new StringEventArgs(value));
			}
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractTswFt5ButtonAdapter()
		{
			m_BacklightControl = InstantiateBacklightControl(BACKLIGHT_CONTROL_ID);
			Controls.Add(m_BacklightControl);
		}

#if SIMPLSHARP
		#region Panel Callbacks

		/// <summary>
		/// Sets the wrapped panel device.
		/// </summary>
		/// <param name="panel"></param>
		public override void SetPanel(TPanel panel)
		{
			base.SetPanel(panel);

			IpAddress = panel == null ? string.Empty : panel.ExtenderEthernetReservedSigs.IpAddressFeedback.StringValue;
			MacAddress = panel == null ? string.Empty : panel.ExtenderEthernetReservedSigs.MacAddressFeedback.StringValue;
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
							IpAddress = args.Sig.StringValue;
							break;
						case 17309:
							MacAddress = args.Sig.StringValue;
							break;
					}
					break;
				case eSigEvent.StringOutputSigsCleared:
					IpAddress = null;
					MacAddress = null;
					break;
			}
		}

		#endregion
#endif

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			m_EnableVoip = false;

			Controls.Remove(VOIP_DIALER_CONTROL_ID);
			if (m_DialingControl != null)
				m_DialingControl.Dispose();
			m_DialingControl = null;
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.EnableVoip = m_EnableVoip;
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

			// Create the control after the panel has been registered
			if (!m_EnableVoip)
				return;

			m_DialingControl = InstantiateDialingControl(VOIP_DIALER_CONTROL_ID);
			Controls.Add(m_DialingControl);
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
		/// Override to control the type of dialing control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected abstract IDialingDeviceControl InstantiateDialingControl(int id);

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of backlight control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected abstract IPowerDeviceControl InstantiateBacklightControl(int id);

		#endregion
	}

	public abstract class AbstractTswFt5ButtonAdapterSettings : AbstractTriListAdapterSettings,
	                                                            ITswFt5ButtonAdapterSettings
	{
		private const string ENABLE_VOIP_ELEMENT = "EnableVoIP";

		public bool EnableVoip { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(ENABLE_VOIP_ELEMENT, IcdXmlConvert.ToString(EnableVoip));
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			EnableVoip = XmlUtils.TryReadChildElementContentAsBoolean(xml, ENABLE_VOIP_ELEMENT) ?? false;
		}
	}

	public interface ITswFt5ButtonAdapter : ITriListAdapter
	{
		/// <summary>
		/// Gets the VoIp dialer for this panel.
		/// </summary>
		[PublicAPI]
		IDialingDeviceControl VoipDialingControl { get; }
	}

	public interface ITswFt5ButtonAdapterSettings : ITriListAdapterSettings
	{
		bool EnableVoip { get; set; }
	}
}
