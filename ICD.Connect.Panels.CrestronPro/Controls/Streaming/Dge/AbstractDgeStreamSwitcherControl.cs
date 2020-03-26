using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge;
using ICD.Connect.Routing;
using ICD.Connect.Routing.Connections;
using ICD.Connect.Routing.Controls;
using ICD.Connect.Routing.Controls.Streaming;
using ICD.Connect.Routing.EventArguments;

namespace ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge
{
	public abstract class AbstractDgeStreamSwitcherControl<TParent> : AbstractRouteSwitcherControl<TParent>, IStreamRouteDestinationControl 
		where TParent : IDgeX00Adapter
	{
		private const int STREAM_INPUT_ADDRESS = 1;
		private const int HDMI_INPUT_ADDRESS = 2;

		private const int OUTPUT_ADDRESS = 1;

		#region Events

		public event EventHandler<StreamUriEventArgs> OnInputStreamUriChanged;
		public override event EventHandler<SourceDetectionStateChangeEventArgs> OnSourceDetectionStateChange;
		public override event EventHandler<ActiveInputStateChangeEventArgs> OnActiveInputsChanged;
		public override event EventHandler<RouteChangeEventArgs> OnRouteChange;
		public override event EventHandler<TransmissionStateEventArgs> OnActiveTransmissionStateChanged;

		#endregion

		#region Delegates

		public delegate bool RouteDelegate(RouteOperation info);

		public delegate bool ClearOutputDelegate();

		public delegate bool SetStreamDelegate(Uri stream);

		public RouteDelegate UiRoute { get; set; }

		public ClearOutputDelegate UiClearOutput { get; set; }

		public SetStreamDelegate UiSetStream { get; set; }

		#endregion

		private Uri m_StreamUri;
		private int? m_ActiveInput;

		public Uri StreamUri
		{
			get { return m_StreamUri; }
			private set
			{
				if (m_StreamUri == value)
					return;

				m_StreamUri = value;
				Log(eSeverity.Debug, "Stream Uri changed to {0}", m_StreamUri);
				OnInputStreamUriChanged.Raise(this,
				                              new StreamUriEventArgs(eConnectionType.Audio | eConnectionType.Video,
				                                                     STREAM_INPUT_ADDRESS, m_StreamUri));
			}
		}

		public int? ActiveInput
		{
			get { return m_ActiveInput; }
			set
			{
				if (m_ActiveInput == value)
					return;

				if (m_ActiveInput != null)
					OnActiveInputsChanged.Raise(this,
					                            new ActiveInputStateChangeEventArgs(m_ActiveInput.Value,
					                                                                eConnectionType.Audio | eConnectionType.Video,
					                                                                false));

				m_ActiveInput = value;
				Log(eSeverity.Debug, "Active Input changed to {0}", m_ActiveInput);

				if (m_ActiveInput != null)
					OnActiveInputsChanged.Raise(this,
					                            new ActiveInputStateChangeEventArgs(m_ActiveInput.Value,
					                                                                eConnectionType.Audio | eConnectionType.Video, true));
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractDgeStreamSwitcherControl(TParent parent, int id) 
			: base(parent, id)
		{
		}

		#region Methods

		/// <summary>
		/// Returns true if a signal is detected at the given input.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public override bool GetSignalDetectedState(int input, eConnectionType type)
		{
			if (EnumUtils.HasMultipleFlags(type))
			{
				return EnumUtils.GetFlagsExceptNone(type)
								.Select(f => GetSignalDetectedState(input, f))
								.Unanimous(false);
			}

			if (ContainsInput(input))
			{
				string message = string.Format("{0} has no {1} input at address {2}", this, type, input);
				throw new ArgumentOutOfRangeException("input", message);
			}

			switch (type)
			{
				case eConnectionType.Audio:
				case eConnectionType.Video:
					return true;

				default:
					throw new ArgumentOutOfRangeException("type");
			}
		}

		/// <summary>
		/// Gets the input at the given address.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override ConnectorInfo GetInput(int input)
		{
			if (!ContainsInput(input))
				throw new ArgumentOutOfRangeException("No inputs with address " + input);

			return new ConnectorInfo(input, eConnectionType.Audio | eConnectionType.Video);
		}

		/// <summary>
		/// Returns true if the destination contains an input at the given address.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override bool ContainsInput(int input)
		{
			return input == STREAM_INPUT_ADDRESS || input == HDMI_INPUT_ADDRESS;
		}

		/// <summary>
		/// Returns the inputs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetInputs()
		{
			yield return GetInput(STREAM_INPUT_ADDRESS);
			yield return GetInput(HDMI_INPUT_ADDRESS);
		}

		/// <summary>
		/// Gets the output at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public override ConnectorInfo GetOutput(int address)
		{
			if (!ContainsOutput(address))
				throw new ArgumentOutOfRangeException("No outputs with address " + address);

			return new ConnectorInfo(address, eConnectionType.Audio | eConnectionType.Video);
		}

		/// <summary>
		/// Returns true if the source contains an output at the given address.
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public override bool ContainsOutput(int output)
		{
			return output == OUTPUT_ADDRESS;
		}

		/// <summary>
		/// Returns the outputs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetOutputs()
		{
			yield return GetOutput(OUTPUT_ADDRESS);
		}

		/// <summary>
		/// Gets the outputs for the given input.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetOutputs(int input, eConnectionType type)
		{
			if (!ContainsInput(input))
				throw new ArgumentOutOfRangeException("No inputs with address " + input);

			if (m_ActiveInput.HasValue && m_ActiveInput.Value == input)
				yield return GetOutput(OUTPUT_ADDRESS);
		}

		/// <summary>
		/// Gets the input routed to the given output matching the given type.
		/// </summary>
		/// <param name="output"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Type has multiple flags.</exception>
		public override ConnectorInfo? GetInput(int output, eConnectionType type)
		{
			if (!ContainsOutput(output))
				throw new ArgumentOutOfRangeException("No outputs with address " + output);

			if (m_ActiveInput == null)
				return null;

			return GetInput(m_ActiveInput.Value);
		}

		/// <summary>
		/// Performs the given route operation.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public override bool Route(RouteOperation info)
		{
			var callback = UiRoute;

			return callback != null && callback(info);
		}

		/// <summary>
		/// Stops routing to the given output.
		/// </summary>
		/// <param name="output"></param>
		/// <param name="type"></param>
		/// <returns>True if successfully cleared.</returns>
		public override bool ClearOutput(int output, eConnectionType type)
		{
			if (!ContainsOutput(output))
				throw new ArgumentOutOfRangeException("No outputs with address " + output);

			var callback = UiClearOutput;
			return callback != null && callback();
		}

		public bool SetStreamForInput(int input, Uri stream)
		{
			if (input != STREAM_INPUT_ADDRESS)
				throw new ArgumentOutOfRangeException("No stream input with address " + input);

			var callback = UiSetStream;

			if (callback != null && callback(stream))
			{
				StreamUri = stream;
				return true;
			}

			return false;
		}

		public Uri GetStreamForInput(int input)
		{
			if (input != STREAM_INPUT_ADDRESS)
				throw new ArgumentOutOfRangeException("No stream for input with address " + input);

			return StreamUri;
		}

		#endregion
	}
}