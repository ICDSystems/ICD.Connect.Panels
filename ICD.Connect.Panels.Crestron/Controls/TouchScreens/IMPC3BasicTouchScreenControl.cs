using System;
using ICD.Connect.Misc.Keypads;

namespace ICD.Connect.Panels.Crestron.Controls.TouchScreens
{
	public interface IMPC3BasicTouchScreenControl : IThreeSeriesTouchScreenControl
	{
		/// <summary>
		/// Raised when a button state changes.
		/// </summary>
		event EventHandler<KeypadButtonPressedEventArgs> OnButtonStateChange;

		#region Properties

		/// <summary>
		/// When true, indicates automatic LED brightness adjustment based ambient light is enabled on this device.
		/// </summary>
		bool AutoBrightnessEnabled { get; }

		/// <summary>
		/// Property that returns true when the mute button is enabled on this device, false otherwise.
		/// </summary>
		bool MuteButtonEnabled { get; }

		/// <summary>
		/// Property that returns true when the power button is enabled on this device, false otherwise.
		/// </summary>
		bool PowerButtonEnabled { get; }

		/// <summary>
		/// Indicates the LED brightness level in Active State,
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		ushort ActiveBrightnessPercent { get; }

		/// <summary>
		/// Indicates the LED brightness level in Standby State.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		ushort StandbyBrightnessPercent { get; }

		/// <summary>
		/// Indicates Active State timeout value in minutes.
		/// </summary>
		ushort ActiveTimeoutMinutes { get; }

		/// <summary>
		/// Indicates Standby State timeout value in minutes.
		/// </summary>
		ushort StandbyTimeoutMinutes { get; }

		/// <summary>
		/// Indicates the button LED brightness level.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		ushort LedBrightnessPercent { get; }

		/// <summary>
		/// Reports the current ambient light threshold level in lux unit.
		/// This property is only valid when the AutoBrightnessEnabled property is set to true.
		/// </summary>
		ushort AmbientLightThresholdForAutoBrightnessAdjustmentLux { get; }

		/// <summary>
		/// Reports the current active mode auto brightness low level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		ushort ActiveModeAutoBrightnessLowLevelPercent { get; }

		/// <summary>
		/// Reports the current active mode auto brightness high level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		ushort ActiveModeAutoBrightnessHighLevelPercent { get; }

		/// <summary>
		/// Reports the current standby mode auto brightness low level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		ushort StandbyModeAutoBrightnessLowLevelPercent { get; }

		/// <summary>
		/// Reports the current standby mode auto brightness high level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		ushort StandbyModeAutoBrightnessHighLevelPercent { get; }

		/// <summary>
		/// Reports the current ambient light level in lux unit for selecting high LED level vs low LED level.
		/// 100-400: normal office, 600: bright lab, 10000+: direct sunlight.
		/// </summary>
		ushort AmbientLightLevelLux { get; }

		/// <summary>
		/// Get the Button object corresponding to the Mute button for this device.
		/// </summary>
		eButtonState Mute { get; }

		/// <summary>
		/// Get the Button object corresponding to the Power button for this device.
		/// </summary>
		eButtonState Power { get; }

		/// <summary>
		/// Get the Button object corresponding to the Button1 button for this device.
		/// </summary>
		eButtonState Button1 { get; }

		/// <summary>
		/// Get the Button object corresponding to the Button2 button for this device.
		/// </summary>
		eButtonState Button2 { get; }

		/// <summary>
		/// Get the Button object corresponding to the Button3 buttonm for this device.
		/// </summary>
		eButtonState Button3 { get; }

		/// <summary>
		/// Get the Button object corresponding to the Button4 button for this device.
		/// </summary>
		eButtonState Button4 { get; }

		/// <summary>
		/// Get the Button object corresponding to the Button5 button for this device.
		/// </summary>
		eButtonState Button5 { get; }

		/// <summary>
		/// Get the Button object corresponding to the Button6 button for this device.
		/// </summary>
		eButtonState Button6 { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Enable the mute button on this device.
		/// </summary>
		void SetMuteButtonEnabled(bool enable);

		/// <summary>
		/// Select the mute button on this device.
		/// </summary>
		void SetMuteButtonSelected(bool select);

		/// <summary>
		/// Enable the power button on this device.
		/// </summary>
		/// <param name="enable"></param>
		void SetPowerButtonEnabled(bool enable);

		/// <summary>
		/// Select the power button on this device.
		/// </summary>
		/// <param name="select"></param>
		void SetPowerButtonSelected(bool select);

		/// <summary>
		/// Enable a given numerical button on this device.
		/// </summary>
		/// <param name="buttonNumber">1-6 on MPC3-201 Touchscreen panel.</param>
		/// <param name="enabled"></param>
		/// <exception cref="T:System.IndexOutOfRangeException">Invalid Button Number specified.</exception>
		void SetNumericalButtonEnabled(uint buttonNumber, bool enabled);

		/// <summary>
		/// Select a given numerical button on this device.
		/// </summary>
		/// <param name="buttonNumber">1-6 on MPC3-201 Touchscreen panel.</param>
		/// <param name="selected"></param>
		/// <exception cref="T:System.IndexOutOfRangeException">Invalid Button Number specified.</exception>
		void SetNumericalButtonSelected(uint buttonNumber, bool selected);

		/// <summary>
		/// Enable automatic LED brightness adjustment based ambient light while the property is true.
		/// Setting the property to false will disable it.
		/// </summary>
		void SetAutoBrightnessEnabled(bool enable);

		/// <summary>
		/// Property to indicate if a numerical button is enabled for a button number on this device.
		/// </summary>
		bool GetNumericalButtonEnabled(uint buttonNumber);

		/// <summary>
		/// Specifies LED brightness level in active state.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// </summary>
		void SetActiveBrightness(ushort percent);

		/// <summary>
		/// Specifies LED brightness level in standby state.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// </summary>
		void SetStandbyBrightness(ushort percent);

		/// <summary>
		/// Specifies the Active State timeout value in minutes.
		/// Value 0: the timeout is disabled.
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// </summary>
		void SetActiveTimeout(ushort minutes);

		/// <summary>
		/// Specifies the Standby State timeout value in minutes.
		/// Value 0: the timeout is disabled.
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// </summary>
		void SetStandbyTimeout(ushort minutes);

		/// <summary>
		/// Specifies the button LED brightness level.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// </summary>
		void SetLedBrightness(ushort percent);

		/// <summary>
		/// Specifies ambient light level in lux unit for selecting high LED level vs low LED level.
		/// This property is only valid when the <see cref="P:Crestron.SimplSharpPro.MPC3Basic.AutoBrightnessEnabled"/> property is set to true.
		/// </summary>
		void SetAmbientLightThresholdForAutoBrightnessAdjustment(ushort lux);

		/// <summary>
		/// Specifies active mode auto brightness low level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// </summary>
		void SetActiveModeAutoBrightnessLowLevel(ushort percent);

		/// <summary>
		/// Specifies active mode auto brightness high level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// 
		/// </summary>
		void SetActiveModeAutoBrightnessHighLevel(ushort percent);

		/// <summary>
		/// Specifies standby mode auto brightness low level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// 
		/// </summary>
		void SetStandbyModeAutoBrightnessLowLevel(ushort percent);

		/// <summary>
		/// Specifies standby mode auto brightness high level in lux unit.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// </summary>
		void SetStandbyModeAutoBrightnessHighLevel(ushort percent);

		/// <summary>
		/// Property to set volume bargraph level on this device.
		/// Valid values range from 0 (0%) to 65535 (100%).
		/// This property is not supported by <see cref="T:Crestron.SimplSharpPro.MPC3x30xTouchscreen"/>.
		/// </summary>
		void SetVolumeBargraph(ushort percent);

		#endregion
	}
}
