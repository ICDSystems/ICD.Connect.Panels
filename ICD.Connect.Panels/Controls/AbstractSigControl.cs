using System;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Controls
{
	public abstract class AbstractSigControl<TParent> : AbstractDeviceControl<TParent>, ISigControl
		where TParent : IDeviceBase
	{
		/// <summary>
		/// Raised when the user interacts with the panel.
		/// </summary>
		public abstract event EventHandler<SigInfoEventArgs> OnAnyOutput;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractSigControl(TParent parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public abstract DateTime? LastOutput { get; }

		/// <summary>
		/// Clears the assigned input sig values.
		/// </summary>
		public abstract void Clear();

		/// <summary>
		/// Registers the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public abstract void RegisterOutputSigChangeCallback(uint number, eSigType type,
		                                                     Action<SigCallbackManager, SigInfoEventArgs> callback);

		/// <summary>
		/// Unregisters the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public abstract void UnregisterOutputSigChangeCallback(uint number, eSigType type,
		                                                       Action<SigCallbackManager, SigInfoEventArgs> callback);

		/// <summary>
		/// Sends the serial data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="text"></param>
		public abstract void SendInputSerial(uint number, string text);

		/// <summary>
		/// Sends the analog data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public abstract void SendInputAnalog(uint number, ushort value);

		/// <summary>
		/// Sends the digital data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public abstract void SendInputDigital(uint number, bool value);

		#region Console

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			SigControlConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in SigControlConsole.GetConsoleCommands(this))
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

			foreach (IConsoleNodeBase node in SigControlConsole.GetConsoleNodes(this))
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
