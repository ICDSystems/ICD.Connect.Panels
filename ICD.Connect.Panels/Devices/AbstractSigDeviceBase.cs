using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.Settings;

namespace ICD.Connect.Panels.Devices
{
	/// <summary>
	/// AbstractPanelBase represents shared functionality between the PanelDevice and the SmartObject.
	/// </summary>
	public abstract class AbstractSigDeviceBase<TSettings> : AbstractDeviceBase<TSettings>, ISigDevice
		where TSettings : ISettings, new()
	{
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
		protected AbstractSigDeviceBase()
		{
			m_SigCallbacks = new SigCallbackManager();
			m_SigCallbacks.OnAnyCallback += SigCallbacksOnAnyCallback;
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
				Log(eSeverity.Error, "Unable to send input serial {0} - {1}", number, e.Message);
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
				Log(eSeverity.Error, "Unable to send input analog {0} - {1}", number, e.Message);
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
				Log(eSeverity.Error, "Unable to send input digital {0} - {1}", number, e.Message);
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

		#region Console

		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("Last Output", LastOutput);
		}

		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			yield return new ConsoleCommand("PrintSigs", "Prints sigs that have a value assigned", () => PrintSigs());

			yield return new GenericConsoleCommand<uint, ushort>("SendInputAnalog", "SendInputAnalog <Number> <Value>", (n, v) => SendInputAnalog(n, v));
			yield return new GenericConsoleCommand<uint, bool>("SendInputDigital", "SendInputDigital <Number> <Value>", (n, v) => SendInputDigital(n, v));
			yield return new GenericConsoleCommand<uint, string>("SendInputSerial", "SendInputSerial <Number> <Value>", (n, v) => SendInputSerial(n, v));
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		private string PrintSigs()
		{
			TableBuilder builder = new TableBuilder("Number", "Name", "Type", "Value");

			IEnumerable<ISig> sigs = BooleanInput.Cast<ISig>()
			                                     .Concat(UShortInput.Cast<ISig>())
			                                     .Concat(StringInput.Cast<ISig>()
			                                                        .Where(s => !string.IsNullOrEmpty(s.GetStringValue())))
			                                     .Where(s => s.HasValue())
												 .OrderBy(s => s.Type)
												 .ThenBy(s => s.Number)
												 .ThenBy(s => s.Name);

			foreach (ISig item in sigs)
				builder.AddRow(item.Number, item.Name, item.Type, item.GetValue());

			return builder.ToString();
		}

		#endregion
	}
}
