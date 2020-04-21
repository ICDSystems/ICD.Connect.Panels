using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Panels.Controls;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.Panels.Devices
{
	public abstract class AbstractPanelDevice<T> : AbstractSigDevice<T>, IPanelDevice
		where T : IPanelDeviceSettings, new()
	{
		#region Properties

		/// <summary>
		/// Collection containing the loaded SmartObjects of this device.
		/// </summary>
		public abstract ISmartObjectCollection SmartObjects { get; }

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public override DateTime? LastOutput
		{
			get
			{
				// Return the most recent output time.
				DateTime? output = base.LastOutput;
				DateTime? smartOutput = SmartObjects.Select(p => p.Value)
				                                    .Select(s => s.LastOutput)
				                                    .Where(d => d != null)
				                                    .Max();

				if (output == null)
					return smartOutput;
				if (smartOutput == null)
					return output;

				return output > smartOutput ? output : smartOutput;
			}
		}

		#endregion

		/// <summary>
		/// Override to determine the type of sig control to use with this device.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override ISigControl InstantiateSigControl(int id)
		{
			return new PanelControl(this, id);
		}

		#region Methods

		/// <summary>
		/// Clears the assigned input sig values.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			foreach (ISmartObject so in SmartObjects.Select(kvp => kvp.Value))
				so.Clear();
		}

		#endregion

		#region Console

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			PanelDeviceConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in PanelDeviceConsole.GetConsoleCommands(this))
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

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			foreach (IConsoleNodeBase node in GetBaseConsoleNodes())
				yield return node;

			foreach (IConsoleNodeBase node in PanelDeviceConsole.GetConsoleNodes(this))
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

		#endregion
	}
}
