using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Mock
{
	public sealed class MockSmartObject : AbstractSmartObject
	{
		public override event EventHandler<SigInfoEventArgs> OnAnyOutput;

		private readonly SigCallbackManager m_SigCallbacks;

		private readonly IDeviceBooleanInputCollection m_BooleanInput;
		private readonly IDeviceUShortInputCollection m_UShortInput;
		private readonly IDeviceStringInputCollection m_StringInput;

		private readonly IDeviceBooleanOutputCollection m_BooleanOutput;
		private readonly IDeviceUShortOutputCollection m_UShortOutput;
		private readonly IDeviceStringOutputCollection m_StringOutput;

		private readonly uint m_SmartObjectId;

		#region Properties

		/// <summary>
		/// Collection of Boolean Inputs sent to the device.
		/// </summary>
		public IDeviceBooleanInputCollection BooleanInput { get { return m_BooleanInput; } }

		/// <summary>
		/// Collection of Integer Inputs sent to the device.
		/// </summary>
		public IDeviceUShortInputCollection UShortInput { get { return m_UShortInput; } }

		/// <summary>
		/// Collection of String Inputs sent to the device.
		/// </summary>
		public IDeviceStringInputCollection StringInput { get { return m_StringInput; } }

		/// <summary>
		/// 
		/// </summary>
		public override uint SmartObjectId { get { return m_SmartObjectId; } }

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public override DateTime? LastOutput { get { return m_SigCallbacks.LastOutput; } }

		/// <summary>
		/// Gets the created input sigs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<SigInfo> GetInputSigInfo()
		{
			foreach (IBoolInputSig sig in m_BooleanInput)
				yield return new SigInfo(sig, (ushort)SmartObjectId);

			foreach (IStringInputSig sig in m_StringInput)
				yield return new SigInfo(sig, (ushort)SmartObjectId);

			foreach (IUShortInputSig sig in m_UShortInput)
				yield return new SigInfo(sig, (ushort)SmartObjectId);
		}

		/// <summary>
		/// Gets the created output sigs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<SigInfo> GetOutputSigInfo()
		{
			foreach (IBoolOutputSig sig in m_BooleanOutput)
				yield return new SigInfo(sig, (ushort)SmartObjectId);

			foreach (IStringOutputSig sig in m_StringOutput)
				yield return new SigInfo(sig, (ushort)SmartObjectId);

			foreach (IUShortOutputSig sig in m_UShortOutput)
				yield return new SigInfo(sig, (ushort)SmartObjectId);
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id"></param>
		public MockSmartObject(uint id)
		{
			m_SmartObjectId = id;

			m_BooleanInput = new MockBooleanInputCollection();
			m_UShortInput = new MockUShortInputCollection();
			m_StringInput = new MockStringInputCollection();

			m_BooleanOutput = new MockBooleanOutputCollection();
			m_UShortOutput = new MockUShortOutputCollection();
			m_StringOutput = new MockStringOutputCollection();

			m_SigCallbacks = new SigCallbackManager();
		    m_SigCallbacks.OnAnyCallback += SigCallbacksOnAnyCallback;
        }

		#region Methods

		/// <summary>
		/// Clears the assigned input sig values.
		/// </summary>
		public override void Clear()
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
		public override void RegisterOutputSigChangeCallback(uint number, eSigType type,
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
		public override void UnregisterOutputSigChangeCallback(uint number, eSigType type,
		                                                       Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			m_SigCallbacks.UnregisterSigChangeCallback(number, type, callback);
		}

		/// <summary>
		/// Sends the serial data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="text"></param>
		public override void SendInputSerial(uint number, string text)
		{
			try
			{
				SendSerial(StringInput[number], text);
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "Unable to send serial sig {0}", number);
			}
		}

		/// <summary>
		/// Sends the analog data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public override void SendInputAnalog(uint number, ushort value)
		{
			try
			{
				SendAnalog(UShortInput[number], value);
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "Unable to send analog sig {0}", number);
			}
		}

		/// <summary>
		/// Sends the digital data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public override void SendInputDigital(uint number, bool value)
		{
			try
			{
				SendDigital(BooleanInput[number], value);
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "Unable to send digital sig {0}", number);
			}
		}

		/// <summary>
		/// Raises the sig change callbacks.
		/// </summary>
		/// <param name="sigInfo"></param>
		[PublicAPI]
		public void RaiseOutputSigChange(SigInfo sigInfo)
		{
			m_SigCallbacks.RaiseSigChangeCallback(sigInfo);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Sends the serial data to the panel.
		/// </summary>
		/// <param name="sig"></param>
		/// <param name="text"></param>
		private static void SendSerial(IStringInputSig sig, string text)
		{
			sig.SetStringValue(text);
		}

		/// <summary>
		/// Sends the analog data to the panel.
		/// </summary>
		/// <param name="sig"></param>
		/// <param name="value"></param>
		private static void SendAnalog(IUShortInputSig sig, ushort value)
		{
			sig.SetUShortValue(value);
		}

		/// <summary>
		/// Sends the digital data to the panel.
		/// </summary>
		/// <param name="sig"></param>
		/// <param name="value"></param>
		private static void SendDigital(IBoolInputSig sig, bool value)
		{
			sig.SetBoolValue(value);
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
	    private void RaiseOnAnyOutput(SigInfo sigInfo)
	    {
	        OnAnyOutput.Raise(this, new SigInfoEventArgs(sigInfo));
	    }

		#endregion
	}
}
