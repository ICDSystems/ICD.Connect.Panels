using ICD.Common.Utils.Services.Logging;
#if SIMPLSHARP
using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.API.Commands;
using ICD.Connect.Panels.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X60;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.HardButtons
{
	public sealed class TswX60HardButtonBacklightControl : AbstractHardButtonBacklightControl<ITswX60BaseClassAdapter>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswX60HardButtonBacklightControl(ITswX60BaseClassAdapter parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Sets the enabled state of the backlight for the button at the given address.
		/// First button is at address 1.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="enabled"></param>
		public override void SetBacklightEnabled(int address, bool enabled)
		{
			TswX60BaseClass panel = Parent.Panel as TswX60BaseClass;
			if (panel == null)
			{
				Logger.AddEntry(eSeverity.Error, "{0} unable to set button backlight state - internal panel is null");
				return;
			}

			TswX60HardButtonReservedSigs sigs = panel.ExtenderHardButtonReservedSigs;

			switch (address)
			{
				case 1:
					if (enabled)
						sigs.TurnButton1BackLightOn();
					else
						sigs.TurnButton1BackLightOff();
					break;

				case 2:
					if (enabled)
						sigs.TurnButton2BackLightOn();
					else
						sigs.TurnButton2BackLightOff();
					break;

				case 3:
					if (enabled)
						sigs.TurnButton3BackLightOn();
					else
						sigs.TurnButton3BackLightOff();
					break;

				case 4:
					if (enabled)
						sigs.TurnButton4BackLightOn();
					else
						sigs.TurnButton4BackLightOff();
					break;

				case 5:
					if (enabled)
						sigs.TurnButton5BackLightOn();
					else
						sigs.TurnButton5BackLightOff();
					break;

				default:
					throw new ArgumentOutOfRangeException("address", "No button at address " + address);
			}
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			yield return
				new GenericConsoleCommand<int, bool>("SetBacklightEnabled", "SetBacklightEnabled <ADDRESS> <true/false>",
				                                     (i, s) => SetBacklightEnabled(i, s));
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}
	}
}
#endif
