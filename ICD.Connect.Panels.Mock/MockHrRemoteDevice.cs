using System;
using System.Collections.Generic;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Commands;
using ICD.Connect.Devices.Mock;
using ICD.Connect.Panels.Crestron.Devices.Hr;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.HardButtons;

namespace ICD.Connect.Panels.Mock
{
	public sealed class MockHrRemoteDevice : AbstractMockDevice<MockHrRemoteDeviceSettings>, IHrRemoteAdapter
	{
		public event EventHandler<ButtonActionEventArgs> OnButtonAction;

		#region Simulate Button Presses

		private void ButtonTap(eHardButton button)
		{
			OnButtonAction.Raise(this, new ButtonActionEventArgs(button, eButtonAction.Tap));
		}

		private void ButtonDoubleTap(eHardButton button)
		{
			OnButtonAction.Raise(this, new ButtonActionEventArgs(button, eButtonAction.DoubleTap));
		}

		private void ButtonHold(eHardButton button)
		{
			OnButtonAction.Raise(this, new ButtonActionEventArgs(button, eButtonAction.Hold));
		}

		private void ButtonHoldRelease(eHardButton button)
		{
			OnButtonAction.Raise(this, new ButtonActionEventArgs(button, eButtonAction.HoldRelease));
		}

		private void ButtonPress(eHardButton button)
		{
			OnButtonAction.Raise(this, new ButtonActionEventArgs(button, eButtonAction.Press));
		}

		private void ButtonRelease(eHardButton button)
		{
			OnButtonAction.Raise(this, new ButtonActionEventArgs(button, eButtonAction.Release));
		}

		#endregion


		#region Console

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			yield return new GenericConsoleCommand<eHardButton>("ButtonPress", "simulates a button press", b => ButtonPress(b));
			yield return new GenericConsoleCommand<eHardButton>("ButtonRelease", "simulates a button release", b => ButtonRelease(b));
			yield return new GenericConsoleCommand<eHardButton>("ButtonTap", "simulates a button tap", b => ButtonTap(b));
			yield return new GenericConsoleCommand<eHardButton>("ButtonDoubleTap", "simulates a button double tap", b => ButtonDoubleTap(b));
			yield return new GenericConsoleCommand<eHardButton>("ButtonHold", "simulates a button hold", b => ButtonHold(b));
			yield return new GenericConsoleCommand<eHardButton>("ButtonHoldRelease", "simulates a button hold release", b => ButtonHoldRelease(b));

		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
