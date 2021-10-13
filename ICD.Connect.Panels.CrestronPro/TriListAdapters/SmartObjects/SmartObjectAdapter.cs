#if !NETSTANDARD
using System.Collections.Generic;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Panels.EventArguments;
using System;
using Crestron.SimplSharpPro;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Misc.CrestronPro.Sigs;
using ICD.Connect.Misc.CrestronPro.Extensions;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;
using eSigType = ICD.Connect.Protocol.Sigs.eSigType;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.SmartObjects
{
	public sealed class SmartObjectAdapter : AbstractSmartObject, IDisposable
	{
		public override event EventHandler<SigInfoEventArgs> OnAnyOutput;

		private readonly SmartObject m_SmartObject;

		private readonly SigCallbackManager m_SigCallbacks;

		private readonly DeviceBooleanInputCollectionAdapter m_BooleanInput;
		private readonly DeviceUShortInputCollectionAdapter m_UShortInput;
		private readonly DeviceStringInputCollectionAdapter m_StringInput;

		private readonly DeviceBooleanOutputCollectionAdapter m_BooleanOutput;
		private readonly DeviceUShortOutputCollectionAdapter m_UShortOutput;
		private readonly DeviceStringOutputCollectionAdapter m_StringOutput;

		#region Properties

		/// <summary>
		/// Gets the ID of the Smart Object.
		/// </summary>
		public override uint SmartObjectId { get { return m_SmartObject.ID; } }

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public override DateTime? LastOutput { get { return m_SigCallbacks.LastOutput; } }

		/// <summary>
		/// Collection of Boolean Inputs sent to the device.
		/// </summary>
		private IDeviceBooleanInputCollection BooleanInput { get { return m_BooleanInput; } }

		/// <summary>
		/// Collection of Integer Inputs sent to the device.
		/// </summary>
		private IDeviceUShortInputCollection UShortInput { get { return m_UShortInput; } }

		/// <summary>
		/// Collection of String Inputs sent to the device.
		/// </summary>
		private IDeviceStringInputCollection StringInput { get { return m_StringInput; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObject"></param>
		public SmartObjectAdapter(SmartObject smartObject)
		{
			m_BooleanInput = new DeviceBooleanInputCollectionAdapter();
			m_UShortInput = new DeviceUShortInputCollectionAdapter();
			m_StringInput = new DeviceStringInputCollectionAdapter();

			m_BooleanOutput = new DeviceBooleanOutputCollectionAdapter();
			m_UShortOutput = new DeviceUShortOutputCollectionAdapter();
			m_StringOutput = new DeviceStringOutputCollectionAdapter();

			m_SigCallbacks = new SigCallbackManager();
			m_SigCallbacks.OnAnyCallback += SigCallbacksOnAnyCallback;

			m_SmartObject = smartObject;

			m_BooleanInput.SetCollection(m_SmartObject.BooleanInput);
			m_UShortInput.SetCollection(m_SmartObject.UShortInput);
			m_StringInput.SetCollection(m_SmartObject.StringInput);

			m_BooleanOutput.SetCollection(m_SmartObject.BooleanOutput);
			m_UShortOutput.SetCollection(m_SmartObject.UShortOutput);
			m_StringOutput.SetCollection(m_SmartObject.StringOutput);

			Subscribe(m_SmartObject);
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public void Dispose()
		{
			Unsubscribe(m_SmartObject);

			m_SigCallbacks.Clear();
		}

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
				StringInput[number].SetStringValue(text);
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "Unable to send input serial {0} - {1}", number, e.Message);
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
				UShortInput[number].SetUShortValue(value);
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "Unable to send input analog {0} - {1}", number, e.Message);
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
				BooleanInput[number].SetBoolValue(value);
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "Unable to send input digital {0} - {1}", number, e.Message);
			}
		}

		#endregion

		#region Private Methods

		private void SigCallbacksOnAnyCallback(object sender, SigInfoEventArgs eventArgs)
		{
			OnAnyOutput.Raise(this, new SigInfoEventArgs(eventArgs.Data));
		}

		#endregion

		#region SmartObject Callbacks

		/// <summary>
		/// Subscribe to the TriList events.
		/// </summary>
		/// <param name="smartObject"></param>
		private void Subscribe(SmartObject smartObject)
		{
			if (smartObject == null)
				return;

			smartObject.SigChange += SmartObjectOnSigChange;
		}

		/// <summary>
		/// Subscribe to the TriList events.
		/// </summary>
		/// <param name="smartObject"></param>
		private void Unsubscribe(SmartObject smartObject)
		{
			if (smartObject == null)
				return;

			smartObject.SigChange -= SmartObjectOnSigChange;
		}

		/// <summary>
		/// Called when a sig change comes from the TriList.
		/// </summary>
		/// <param name="currentDevice"></param>
		/// <param name="smartObjectEventArgs"></param>
		private void SmartObjectOnSigChange(GenericBase currentDevice, SmartObjectEventArgs smartObjectEventArgs)
		{
			SigInfo sigInfo = smartObjectEventArgs.Sig.ToSigInfo((ushort)SmartObjectId);

			m_SigCallbacks.RaiseSigChangeCallback(sigInfo);
		}

		#endregion
	}
}

#endif
