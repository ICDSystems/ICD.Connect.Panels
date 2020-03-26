using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Routing;
using ICD.Connect.Routing.Connections;
using ICD.Connect.Routing.Controls.Streaming;
using ICD.Connect.Routing.EventArguments;

namespace ICD.Connect.Panels.Controls
{
	public sealed class PanelStreamRouteDestinationControl : AbstractStreamRouteDestinationControl<IPanelDevice>
	{
		private const int INPUT_ADDRESS = 1;

		public override event EventHandler<SourceDetectionStateChangeEventArgs> OnSourceDetectionStateChange;
		public override event EventHandler<ActiveInputStateChangeEventArgs> OnActiveInputsChanged;
		public override event EventHandler<StreamUriEventArgs> OnInputStreamUriChanged;
		
		private bool m_InputActive;
		private Uri m_StreamUri;

		[PublicAPI]
		public bool InputActive
		{
			get { return m_InputActive; }
			set
			{
				if (value == m_InputActive)
					return;

				m_InputActive = value;
				Log(eSeverity.Debug, "Input Active set to {0}", m_InputActive);
				OnActiveInputsChanged.Raise(this,
				                            new ActiveInputStateChangeEventArgs(INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video,
				                                                                m_InputActive));
			}
		}

		[PublicAPI]
		public Uri StreamUri
		{
			get { return m_StreamUri; }
			set
			{
				if (value == m_StreamUri)
					return;

				m_StreamUri = value;
				Log(eSeverity.Debug, "Stream Uri set to {0}", m_StreamUri);
				OnInputStreamUriChanged.Raise(this,
				                              new StreamUriEventArgs(eConnectionType.Audio | eConnectionType.Video, INPUT_ADDRESS,
				                                                     m_StreamUri));
			}
		}

		public PanelStreamRouteDestinationControl(IPanelDevice parent, int id) 
			: base(parent, id)
		{
		}

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

			if (input != INPUT_ADDRESS)
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

		public override bool GetInputActiveState(int input, eConnectionType type)
		{
			return input == INPUT_ADDRESS && m_InputActive;
		}

		/// <summary>
		/// Gets the input at the given address.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override ConnectorInfo GetInput(int input)
		{
			if (input != INPUT_ADDRESS)
				throw new ArgumentOutOfRangeException("No inputs with address " + input);

			return new ConnectorInfo(INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video);
		}

		/// <summary>
		/// Returns true if the destination contains an input at the given address.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override bool ContainsInput(int input)
		{
			return input == INPUT_ADDRESS;
		}

		/// <summary>
		/// Returns the inputs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetInputs()
		{
			yield return GetInput(INPUT_ADDRESS);
		}

		public override bool SetStreamForInput(int input, Uri stream)
		{
			if (input != INPUT_ADDRESS)
				throw new ArgumentOutOfRangeException("No input with address " + input);

			StreamUri = stream;
			return true;
		}

		public override Uri GetStreamForInput(int input)
		{
			if (input != INPUT_ADDRESS)
				throw new ArgumentOutOfRangeException("No input with address " + input);

			return StreamUri;
		}
	}
}