#if SIMPLSHARP
using Crestron.SimplSharpPro.DeviceSupport;
#endif
using ICD.Common.Properties;
using ICD.Common.Utils.Xml;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
#if SIMPLSHARP
	public abstract class AbstractTswFt5ButtonAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, ITswFt5ButtonAdapter
		where TPanel : TswFt5Button
#else
	public abstract class AbstractTswFt5ButtonAdapter<TSettings> : AbstractTriListAdapter<TSettings>, ITswFt5ButtonAdapter
#endif
		where TSettings : ITswFt5ButtonAdapterSettings, new()
	{
		private const int VOIP_DIALER_CONTROL_ID = 1;

		private IDialingDeviceControl m_DialingControl;

		// Used with settings
		private bool m_EnableVoIp;

		/// <summary>
		/// Gets the VoIp dialer for this panel.
		/// </summary>
		public IDialingDeviceControl VoIpDialingControl { get { return m_DialingControl; }}

#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			m_EnableVoIp = false;

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

			settings.EnableVoIp = m_EnableVoIp;
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			// Set this value before applying the rest of the settings and registering the panel
			m_EnableVoIp = settings.EnableVoIp;

			base.ApplySettingsFinal(settings, factory);

			// Create the control after the panel has been registered
			if (!m_EnableVoIp)
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

			if (m_EnableVoIp)
				RegisterVoIpExtender(panel);
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
		/// Override to control the type of dialing control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected abstract IDialingDeviceControl InstantiateDialingControl(int id);

#endregion
	}

	public abstract class AbstractTswFt5ButtonAdapterSettings : AbstractTriListAdapterSettings,
															ITswFt5ButtonAdapterSettings
	{
		private const string ENABLE_VOIP_ELEMENT = "EnableVoIP";

		public bool EnableVoIp { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(ENABLE_VOIP_ELEMENT, IcdXmlConvert.ToString(EnableVoIp));
		}

		protected static void ParseXml(AbstractTswFt5ButtonAdapterSettings instance, string xml)
		{
			instance.EnableVoIp = XmlUtils.TryReadChildElementContentAsBoolean(xml, ENABLE_VOIP_ELEMENT) ?? false;

			AbstractTriListAdapterSettings.ParseXml(instance, xml);
		}
	}

	public interface ITswFt5ButtonAdapter : ITriListAdapter
	{
		/// <summary>
		/// Gets the VoIp dialer for this panel.
		/// </summary>
		[PublicAPI]
		IDialingDeviceControl VoIpDialingControl { get; }
	}

	public interface ITswFt5ButtonAdapterSettings : ITriListAdapterSettings
	{
		bool EnableVoIp { get; set; }
	}
}
