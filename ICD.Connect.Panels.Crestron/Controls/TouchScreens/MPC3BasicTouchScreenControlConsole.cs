using System;
using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Panels.Crestron.Controls.TouchScreens
{
	public static class MPC3BasicTouchScreenControlConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(IMPC3BasicTouchScreenControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield break;
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="addRow"></param>
		public static void BuildConsoleStatus(IMPC3BasicTouchScreenControl instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			addRow("AutoBrightnessEnabled", instance.AutoBrightnessEnabled);
			addRow("MuteButtonEnabled", instance.MuteButtonEnabled);
			addRow("PowerButtonEnabled", instance.PowerButtonEnabled);
			addRow("ActiveBrightnessPercent", instance.ActiveBrightnessPercent);
			addRow("StandbyBrightnessPercent", instance.StandbyBrightnessPercent);
			addRow("ActiveTimeoutMinutes", instance.ActiveTimeoutMinutes);
			addRow("StandbyTimeoutMinutes", instance.StandbyTimeoutMinutes);
			addRow("LedBrightnessPercent", instance.LedBrightnessPercent);
			addRow("AmbientLightThresholdForAutoBrightnessAdjustmentLux", instance.AmbientLightThresholdForAutoBrightnessAdjustmentLux);
			addRow("ActiveModeAutoBrightnessLowLevelPercent", instance.ActiveModeAutoBrightnessLowLevelPercent);
			addRow("ActiveModeAutoBrightnessHighLevelPercent", instance.ActiveModeAutoBrightnessHighLevelPercent);
			addRow("StandbyModeAutoBrightnessLowLevelPercent", instance.StandbyModeAutoBrightnessLowLevelPercent);
			addRow("StandbyModeAutoBrightnessHighLevelPercent", instance.StandbyModeAutoBrightnessHighLevelPercent);
			addRow("AmbientLightLevelLux", instance.AmbientLightLevelLux);

			addRow("Mute", instance.Mute);
			addRow("Power", instance.Power);
			addRow("Button1", instance.Button1);
			addRow("Button2", instance.Button2);
			addRow("Button3", instance.Button3);
			addRow("Button4", instance.Button4);
			addRow("Button5", instance.Button5);
			addRow("Button6", instance.Button6);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(IMPC3BasicTouchScreenControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield return new GenericConsoleCommand<bool>("SetMuteButtonEnabled", "SetMuteButtonEnabled <true/false>", b => instance.SetMuteButtonEnabled(b));
			yield return new GenericConsoleCommand<bool>("SetMuteButtonSelected", "SetMuteButtonSelected <true/false>", b => instance.SetMuteButtonSelected(b));
			yield return new GenericConsoleCommand<bool>("SetPowerButtonEnabled", "SetPowerButtonEnabled <true/false>", b => instance.SetPowerButtonEnabled(b));
			yield return new GenericConsoleCommand<bool>("SetPowerButtonSelected", "SetPowerButtonSelected <true/false>", b => instance.SetPowerButtonSelected(b));
			yield return new GenericConsoleCommand<uint, bool>("SetNumericalButtonEnabled", "SetNumericalButtonEnabled <BUTTON> <true/false>", (n, b) => instance.SetNumericalButtonEnabled(n, b));
			yield return new GenericConsoleCommand<uint, bool>("SetNumericalButtonSelected", "SetNumericalButtonSelected <BUTTON> <true/false>", (n, b) => instance.SetNumericalButtonSelected(n, b));
			yield return new GenericConsoleCommand<bool>("SetAutoBrightnessEnabled", "SetAutoBrightnessEnabled <true/false>", b => instance.SetAutoBrightnessEnabled(b));
			yield return new GenericConsoleCommand<ushort>("SetActiveBrightness", "SetActiveBrightness <0 - 65,535>", b => instance.SetActiveBrightness(b));
			yield return new GenericConsoleCommand<ushort>("SetStandbyBrightness", "SetStandbyBrightness <0 - 65,535>", b => instance.SetStandbyBrightness(b));
			yield return new GenericConsoleCommand<ushort>("SetActiveTimeout", "SetActiveTimeout <MINUTES>", b => instance.SetActiveTimeout(b));
			yield return new GenericConsoleCommand<ushort>("SetStandbyTimeout", "SetStandbyTimeout <MINUTES>", b => instance.SetStandbyTimeout(b));
			yield return new GenericConsoleCommand<ushort>("SetLedBrightness", "SetLedBrightness <0 - 65,535>", b => instance.SetLedBrightness(b));
			yield return new GenericConsoleCommand<ushort>("SetLedBrightness", "SetLedBrightness <0 - 65,535>", b => instance.SetLedBrightness(b));

			yield return new GenericConsoleCommand<ushort>("SetAmbientLightThresholdForAutoBrightnessAdjustment",
			                                               "SetAmbientLightThresholdForAutoBrightnessAdjustment <LUX>",
			                                               b => instance.SetAmbientLightThresholdForAutoBrightnessAdjustment(b));

			yield return new GenericConsoleCommand<ushort>("SetActiveModeAutoBrightnessLowLevel", "SetActiveModeAutoBrightnessLowLevel <0 - 65,535>", b => instance.SetActiveModeAutoBrightnessLowLevel(b));
			yield return new GenericConsoleCommand<ushort>("SetActiveModeAutoBrightnessHighLevel", "SetActiveModeAutoBrightnessHighLevel <0 - 65,535>", b => instance.SetActiveModeAutoBrightnessHighLevel(b));
			yield return new GenericConsoleCommand<ushort>("SetStandbyModeAutoBrightnessLowLevel", "SetStandbyModeAutoBrightnessLowLevel <0 - 65,535>", b => instance.SetStandbyModeAutoBrightnessLowLevel(b));
			yield return new GenericConsoleCommand<ushort>("SetStandbyModeAutoBrightnessHighLevel", "SetStandbyModeAutoBrightnessHighLevel <0 - 65,535>", b => instance.SetStandbyModeAutoBrightnessHighLevel(b));
			yield return new GenericConsoleCommand<ushort>("SetVolumeBargraph", "SetVolumeBargraph <0 - 65,535>", b => instance.SetVolumeBargraph(b));
		}
	}
}
