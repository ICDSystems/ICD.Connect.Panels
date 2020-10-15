using System;
using System.Collections.Generic;
using ICD.Common.Utils.Services;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.SmartObjects
{
	public abstract class AbstractSmartObject : ISmartObject
	{
		public abstract event EventHandler<SigInfoEventArgs> OnAnyOutput;

		#region Properties

		public abstract uint SmartObjectId { get; }

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public abstract DateTime? LastOutput { get; }

		protected ILoggerService Logger { get { return ServiceProvider.TryGetService<ILoggerService>(); } }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the created input sigs.
		/// </summary>
		/// <returns></returns>
		public abstract IEnumerable<SigInfo> GetInputSigInfo();

		/// <summary>
		/// Gets the created output sigs.
		/// </summary>
		/// <returns></returns>
		public abstract IEnumerable<SigInfo> GetOutputSigInfo();

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

		#endregion
	}
}
