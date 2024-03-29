﻿using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Server
{
	public sealed class PanelServerSmartObject : AbstractSmartObject
	{
		public override event EventHandler<SigInfoEventArgs> OnAnyOutput;

		private readonly ushort m_SmartObjectId;
		private readonly IPanelServerDevice m_Device;
		private readonly SigCallbackManager m_SigCallbacks;

		#region Properties

		/// <summary>
		/// Gets the ID of the Smart Object.
		/// </summary>
		public override uint SmartObjectId { get { return m_SmartObjectId; } }

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public override DateTime? LastOutput { get { return m_SigCallbacks.LastOutput; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="device"></param>
		/// <param name="smartObjectId"></param>
		public PanelServerSmartObject(IPanelServerDevice device, ushort smartObjectId)
		{
			m_Device = device;
			m_SmartObjectId = smartObjectId;

			m_SigCallbacks = new SigCallbackManager();
			m_SigCallbacks.OnAnyCallback += SigCallbacksOnAnyOutput;
		}

		/// <summary>
		/// Called when any output sig on this SmartObject changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void SigCallbacksOnAnyOutput(object sender, SigInfoEventArgs eventArgs)
		{
			OnAnyOutput.Raise(this, new SigInfoEventArgs(eventArgs.Data));
		}

		#region Methods

		/// <summary>
		/// Gets the created input sigs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<SigInfo> GetInputSigInfo()
		{
			return m_Device.GetCachedInputSigs().Where(s => s.SmartObject == m_SmartObjectId);
		}

		/// <summary>
		/// Gets the created output sigs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<SigInfo> GetOutputSigInfo()
		{
			return m_SigCallbacks.GetOutputSigs();
		}

		/// <summary>
		/// Clears the assigned input sig values.
		/// </summary>
		public override void Clear()
		{
			// Clearing is handled by the parent device.
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
			m_Device.SendSig(new SigInfo(number, m_SmartObjectId, text));
		}

		/// <summary>
		/// Sends the analog data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public override void SendInputAnalog(uint number, ushort value)
		{
			m_Device.SendSig(new SigInfo(number, m_SmartObjectId, value));
		}

		/// <summary>
		/// Sends the digital data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public override void SendInputDigital(uint number, bool value)
		{
			m_Device.SendSig(new SigInfo(number, m_SmartObjectId, value));
		}

		#endregion

		/// <summary>
		/// The parent PanelServerDevices hands any output sigs with a SmartObject ID to the child
		/// SmartObjectAdapter for handling.
		/// </summary>
		/// <param name="sigInfo"></param>
		internal void HandleOutputSig(SigInfo sigInfo)
		{
			if (sigInfo.SmartObject != m_SmartObjectId)
				throw new InvalidOperationException();
			m_SigCallbacks.RaiseSigChangeCallback(sigInfo);
		}
	}
}
