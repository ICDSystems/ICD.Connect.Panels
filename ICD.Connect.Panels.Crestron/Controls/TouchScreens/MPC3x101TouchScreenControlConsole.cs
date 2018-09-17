using System;
using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Panels.Crestron.Controls.TouchScreens
{
	public static class MPC3x101TouchScreenControlConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(IMPC3x101TouchScreenControl instance)
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
		public static void BuildConsoleStatus(IMPC3x101TouchScreenControl instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			addRow("VolumeUp", instance.VolumeUp);
			addRow("VolumeDown", instance.VolumeDown);
			addRow("ButtonPressBeepingEnabled", instance.ButtonPressBeepingEnabled);
			addRow("ProximityDetected", instance.ProximityDetected);
			addRow("ProximityWakeupEnabled", instance.ProximityWakeupEnabled);
			addRow("VolumeDownButtonEnabled", instance.VolumeDownButtonEnabled);
			addRow("VolumeUpButtonEnabled", instance.VolumeUpButtonEnabled);
			addRow("ProximityRange", instance.ProximityRange);
			addRow("ProximityThreshold", instance.ProximityThreshold);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(IMPC3x101TouchScreenControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield return new GenericConsoleCommand<bool>("SetVolumeDownButtonEnabled", "SetVolumeDownButtonEnabled <true/false>", b => instance.SetVolumeDownButtonEnabled(b));
			yield return new GenericConsoleCommand<bool>("SetVolumeUpButtonEnabled", "SetVolumeUpButtonEnabled <true/false>", b => instance.SetVolumeUpButtonEnabled(b));
			yield return new GenericConsoleCommand<bool>("SetButtonPressBeepingEnabled", "SetButtonPressBeepingEnabled <true/false>", b => instance.SetButtonPressBeepingEnabled(b));
			yield return new GenericConsoleCommand<bool>("SetProximityWakeupEnabled", "SetProximityWakeupEnabled <true/false>", b => instance.SetProximityWakeupEnabled(b));
			yield return new GenericConsoleCommand<ushort>("SetProximityThreshold", "SetProximityThreshold <CENTIMETERS>", cm => instance.SetProximityThreshold(cm));
		}
	}
}
