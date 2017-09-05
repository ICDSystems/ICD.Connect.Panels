using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public abstract class AbstractTswFt5ButtonAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, ITswFt5ButtonAdapter
		where TSettings : ITswFt5ButtonAdapterSettings, new()
		where TPanel : TswFt5Button
	{
		// Used with settings
		private bool m_EnableVoIp;

		protected AbstractTswFt5ButtonAdapter()
		{
			Controls.Add(new TswFt5ButtonDialingControl(this, 1));
		}

		/// <summary>
		/// Called before registration.
		/// Override to control which extenders are used with the panel.
		/// </summary>
		/// <param name="panel"></param>
		protected override void RegisterExtenders(TPanel panel)
		{
			base.RegisterExtenders(panel);

			if (m_EnableVoIp)
				panel.ExtenderVoipReservedSigs.Use();
		}

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
}
