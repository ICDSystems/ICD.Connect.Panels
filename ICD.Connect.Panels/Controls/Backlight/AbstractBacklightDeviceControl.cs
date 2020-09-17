using System;
using System.Collections.Generic;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Panels.EventArguments;

namespace ICD.Connect.Panels.Controls.Backlight
{
	public abstract class AbstractBacklightDeviceControl<TDevice> : AbstractDeviceControl<TDevice>, IBacklightDeviceControl
		where TDevice : IDevice
	{
		/// <summary>
		/// Raised when the backlight state changes.
		/// </summary>
		public event EventHandler<BacklightDeviceControlBacklightStateApiEventArgs> OnBacklightStateChanged;

		private eBacklightState m_BacklightState;

		/// <summary>
		/// Gets the state of the backlight.
		/// </summary>
		public eBacklightState BacklightState
		{
			get { return m_BacklightState; }
			protected set
			{
				try
				{
					if (value == m_BacklightState)
						return;

					m_BacklightState = value;

					Logger.LogSetTo(eSeverity.Informational, "BacklightState", m_BacklightState);

					OnBacklightStateChanged.Raise(this, new BacklightDeviceControlBacklightStateApiEventArgs(m_BacklightState));
				}
				finally
				{
					Activities.LogActivity(BacklightDeviceControlActivities.GetBacklightActivity(m_BacklightState));
				}
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractBacklightDeviceControl(TDevice parent, int id)
			: base(parent, id)
		{
			// Initialize activities
			BacklightState = eBacklightState.Unknown;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		/// <param name="uuid"></param>
		protected AbstractBacklightDeviceControl(TDevice parent, int id, Guid uuid)
			: base(parent, id, uuid)
		{
			// Initialize activities
			BacklightState = eBacklightState.Unknown;
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnBacklightStateChanged = null;

			base.DisposeFinal(disposing);
		}

		#region Methods

		/// <summary>
		/// Turns on the backlight.
		/// </summary>
		[PublicAPI]
		public abstract void BacklightOn();

		/// <summary>
		/// Turns off the backlight.
		/// </summary>
		[PublicAPI]
		public abstract void BacklightOff();

		#endregion

		#region Console

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			foreach (IConsoleNodeBase node in GetBaseConsoleNodes())
				yield return node;

			foreach (IConsoleNodeBase node in BacklightDeviceControlConsole.GetConsoleNodes(this))
				yield return node;
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleNodeBase> GetBaseConsoleNodes()
		{
			return base.GetConsoleNodes();
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			BacklightDeviceControlConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in BacklightDeviceControlConsole.GetConsoleCommands(this))
				yield return command;
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
