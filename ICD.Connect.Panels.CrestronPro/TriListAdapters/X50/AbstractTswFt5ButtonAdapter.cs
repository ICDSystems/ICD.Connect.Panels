﻿using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Common.Properties;
using ICD.Common.Utils.Xml;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50
{
	public abstract class AbstractTswFt5ButtonAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, ITswFt5ButtonAdapter
		where TSettings : ITswFt5ButtonAdapterSettings, new()
		where TPanel : TswFt5Button
	{
		// Used with settings
		private bool m_EnableVoIp;

		/// <summary>
		/// Gets the VoIp dialer for this panel.
		/// </summary>
		public abstract IDialingDeviceControl VoIpDialingControl { get; }

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
		protected abstract void RegisterVoIpExtender(TPanel panel);

		#region New region

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			m_EnableVoIp = false;
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
			// Set this value before applying the rest of the settings and instantiating the panel
			m_EnableVoIp = settings.EnableVoIp;

			base.ApplySettingsFinal(settings, factory);
		}

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
