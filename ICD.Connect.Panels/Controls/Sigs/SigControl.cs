using System;
using System.Collections.Generic;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Controls.Sigs
{
	public sealed class SigControl : AbstractSigControl<ISigDevice>
	{
		/// <summary>
		/// Raised when the user interacts with the panel.
		/// </summary>
		public override event EventHandler<SigInfoEventArgs> OnAnyOutput;

		#region Properties

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public override DateTime? LastOutput { get { return Parent.LastOutput; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public SigControl(ISigDevice parent, int id)
			: base(parent, id)
		{
			Parent.OnAnyOutput += ParentOnAnyOutput;
		}

		#region Methods

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnAnyOutput = null;

			base.DisposeFinal(disposing);

			Parent.OnAnyOutput -= OnAnyOutput;
		}

		/// <summary>
		/// Gets the created input sigs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<SigInfo> GetInputSigInfo()
		{
			return Parent.GetInputSigInfo();
		}

		/// <summary>
		/// Gets the created output sigs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<SigInfo> GetOutputSigInfo()
		{
			return Parent.GetOutputSigInfo();
		}

		/// <summary>
		/// Clears the assigned input sig values.
		/// </summary>
		public override void Clear()
		{
			Parent.Clear();
		}

		/// <summary>
		/// Registers the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public override void RegisterOutputSigChangeCallback(uint number, eSigType type, Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			Parent.RegisterOutputSigChangeCallback(number, type, callback);
		}

		/// <summary>
		/// Unregisters the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public override void UnregisterOutputSigChangeCallback(uint number, eSigType type, Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			Parent.UnregisterOutputSigChangeCallback(number, type, callback);
		}

		/// <summary>
		/// Sends the serial data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="text"></param>
		public override void SendInputSerial(uint number, string text)
		{
			Parent.SendInputSerial(number, text);
		}

		/// <summary>
		/// Sends the analog data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public override void SendInputAnalog(uint number, ushort value)
		{
			Parent.SendInputAnalog(number, value);
		}

		/// <summary>
		/// Sends the digital data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public override void SendInputDigital(uint number, bool value)
		{
			Parent.SendInputDigital(number, value);
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Called when the parent device raises any output.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="sigInfoEventArgs"></param>
		private void ParentOnAnyOutput(object sender, SigInfoEventArgs sigInfoEventArgs)
		{
			OnAnyOutput.Raise(this, new SigInfoEventArgs(sigInfoEventArgs));
		}

		#endregion
	}
}
