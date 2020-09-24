using System;
using System.Collections.Generic;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Panels.Controls.Sigs;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.Settings;

namespace ICD.Connect.Panels.Devices
{
	public abstract class AbstractSigDevice<TSettings> : AbstractDevice<TSettings>, ISigDevice
		where TSettings : IDeviceSettings, new()
	{
		private const int PANEL_CONTROL_ID = 10;

		/// <summary>
		/// Raised when the user interacts with the panel.
		/// </summary>
		public event EventHandler<SigInfoEventArgs> OnAnyOutput;

		private readonly SigCallbackManager m_SigCallbacks;

		#region Properties

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public virtual DateTime? LastOutput { get { return m_SigCallbacks.LastOutput; } }

		/// <summary>
		/// Collection of Boolean Inputs sent to the device.
		/// </summary>
		public abstract IDeviceBooleanInputCollection BooleanInput { get; }

		/// <summary>
		/// Collection of Integer Inputs sent to the device.
		/// </summary>
		public abstract IDeviceUShortInputCollection UShortInput { get; }

		/// <summary>
		/// Collection of String Inputs sent to the device.
		/// </summary>
		public abstract IDeviceStringInputCollection StringInput { get; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractSigDevice()
		{
			m_SigCallbacks = new SigCallbackManager();
			m_SigCallbacks.OnAnyCallback += SigCallbacksOnAnyCallback;
		}

		/// <summary>
		/// Override to determine the type of sig control to use with this device.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected virtual ISigControl InstantiateSigControl(int id)
		{
			return new SigControl(this, id);
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnAnyOutput = null;

			base.DisposeFinal(disposing);

			m_SigCallbacks.Clear();
		}

		/// <summary>
		/// Clears the assigned input sig values.
		/// </summary>
		public virtual void Clear()
		{
			foreach (IBoolInputSig item in BooleanInput)
				SendInputDigital(item.Number, false);

			foreach (IUShortInputSig item in UShortInput)
				SendInputAnalog(item.Number, 0);

			foreach (IStringInputSig item in StringInput)
				SendInputSerial(item.Number, null);
		}

		/// <summary>
		/// Registers the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public void RegisterOutputSigChangeCallback(uint number, eSigType type,
		                                            Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			m_SigCallbacks.RegisterSigChangeCallback(number, type, callback);
		}

		/// <summary>
		/// Unregisters the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public void UnregisterOutputSigChangeCallback(uint number, eSigType type,
		                                              Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			m_SigCallbacks.UnregisterSigChangeCallback(number, type, callback);
		}

		/// <summary>
		/// Sends the serial data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="text"></param>
		public void SendInputSerial(uint number, string text)
		{
			try
			{
				StringInput[number].SetStringValue(text);
			}
			catch (Exception e)
			{
				Logger.Log(eSeverity.Error, "Unable to send input serial {0} - {1}", number, e.Message);
			}
		}

		/// <summary>
		/// Sends the analog data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public void SendInputAnalog(uint number, ushort value)
		{
			try
			{
				UShortInput[number].SetUShortValue(value);
			}
			catch (Exception e)
			{
				Logger.Log(eSeverity.Error, "Unable to send input analog {0} - {1}", number, e.Message);
			}
		}

		/// <summary>
		/// Sends the digital data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public void SendInputDigital(uint number, bool value)
		{
			try
			{
				BooleanInput[number].SetBoolValue(value);
			}
			catch (Exception e)
			{
				Logger.Log(eSeverity.Error, "Unable to send input digital {0} - {1}", number, e.Message);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Raises the callbacks registered with the signature.
		/// </summary>
		/// <param name="sigInfo"></param>
		protected void RaiseOutputSigChangeCallback(SigInfo sigInfo)
		{
			m_SigCallbacks.RaiseSigChangeCallback(sigInfo);
		}

		/// <summary>
		/// Called when a sig changes state.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="sigInfoEventArgs"></param>
		private void SigCallbacksOnAnyCallback(object sender, SigInfoEventArgs sigInfoEventArgs)
		{
			RaiseOnAnyOutput(sigInfoEventArgs.Data);
		}

		/// <summary>
		/// Raises the OnAnyOutput event.
		/// </summary>
		protected void RaiseOnAnyOutput(SigInfo sigInfo)
		{
			OnAnyOutput.Raise(this, new SigInfoEventArgs(sigInfo));
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to add controls to the device.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		/// <param name="addControl"></param>
		protected override void AddControls(TSettings settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
			base.AddControls(settings, factory, addControl);

			ISigControl sigControl = InstantiateSigControl(PANEL_CONTROL_ID);
			addControl(sigControl);
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

			SigDeviceConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in SigDeviceConsole.GetConsoleCommands(this))
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

			foreach (IConsoleNodeBase node in SigDeviceConsole.GetConsoleNodes(this))
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
