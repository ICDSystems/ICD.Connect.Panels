using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Panels.Controls.HardButtons
{
	public abstract class AbstractHardButtonBacklightControl<TParent> : AbstractDeviceControl<TParent>,
	                                                                    IHardButtonBacklightControl
		where TParent : IDevice
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractHardButtonBacklightControl(TParent parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Sets the enabled state of the backlight for the button at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="enabled"></param>
		public abstract void SetBacklightEnabled(int address, bool enabled);

		#region Console

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

		#endregion
	}
}
