using System;
using ICD.Connect.Settings;
#if SIMPLSHARP
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.HardButtons;
#endif
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X52;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60
{
#if SIMPLSHARP
	public abstract class AbstractTswX60BaseClassAdapter<TPanel, TSettings> :
		AbstractTswX52ButtonVoiceControlAdapter<TPanel, TSettings>, ITswX60BaseClassAdapter
		where TPanel : TswX60BaseClass
#else
	public abstract class AbstractTswX60BaseClassAdapter<TSettings> :
		AbstractTswX52ButtonVoiceControlAdapter<TSettings>, ITswX60BaseClassAdapter
#endif
		where TSettings : ITswX60BaseClassAdapterSettings, new()
	{
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

			panel.ExtenderHardButtonReservedSigs.Use();
		}

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of backlight control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IPowerDeviceControl InstantiateBacklightControl(int id)
		{
			return new TswX60BaseBacklightControl(this, id);
		}

		/// <summary>
		/// Registers the system extender for the given panel.
		/// </summary>
		/// <param name="panel"></param>
		protected override void RegisterSystemExtender(TPanel panel)
		{
			panel.ExtenderSystemReservedSigs.Use();
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

			addControl(new TswX60HardButtonBacklightControl(this, HARD_BUTTON_CONTROL_ID));
		}
#endif
	}

	public abstract class AbstractTswX60BaseClassAdapterSettings : AbstractTswX52ButtonVoiceControlAdapterSettings,
	                                                               ITswX60BaseClassAdapterSettings
	{
	}

	public interface ITswX60BaseClassAdapterSettings : ITswX52ButtonVoiceControlAdapterSettings
	{
	}

	public interface ITswX60BaseClassAdapter : ITswX52ButtonVoiceControlAdapter
	{
	}
}
