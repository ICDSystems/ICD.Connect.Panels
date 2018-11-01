using ICD.Common.Properties;
using ICD.Common.Utils.Xml;
using ICD.Connect.Conferencing.Conferences;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings.Core;
#if SIMPLSHARP
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

		private ITraditionalConferenceDeviceControl m_ConferenceControl;
		private readonly IPowerDeviceControl m_BacklightControl;

		// Used with settings
		private bool m_EnableVoip;

		#region Properties

		/// <summary>
		/// Gets the VoIp dialer for this panel.
		/// </summary>
		public ITraditionalConferenceDeviceControl VoipConferenceControl { get { return m_ConferenceControl; } }

		/// <summary>
		/// Gets the backlight control for this panel.
		/// </summary>
		public IPowerDeviceControl BacklightControl { get { return m_BacklightControl; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractTswFt5ButtonAdapter()
		{
			m_BacklightControl = InstantiateBacklightControl(BACKLIGHT_CONTROL_ID);
			Controls.Add(m_BacklightControl);
		}

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			m_EnableVoip = false;

			Controls.Remove(VOIP_DIALER_CONTROL_ID);
			if (m_ConferenceControl != null)
				m_ConferenceControl.Dispose();
			m_ConferenceControl = null;
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

			m_ConferenceControl = InstantiateDialingControl(VOIP_DIALER_CONTROL_ID);
			Controls.Add(m_ConferenceControl);
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
		ITraditionalConferenceDeviceControl VoipConferenceControl { get; }
	}

	public interface ITswFt5ButtonAdapterSettings : ITriListAdapterSettings
	{
		bool EnableVoip { get; set; }
	}
}
