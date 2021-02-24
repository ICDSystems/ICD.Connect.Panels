#if SIMPLSHARP
using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels.Crestron.Controls.Streaming.Dge;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge;
using ICD.Connect.Routing;
using ICD.Connect.Routing.Connections;
using ICD.Connect.Routing.Controls;
using ICD.Connect.Routing.Controls.Streaming;
using ICD.Connect.Routing.EventArguments;
using ICD.Connect.Routing.Utils;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.DM;
using Crestron.SimplSharpPro.UI;

namespace ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge
{
	public abstract class AbstractDgeStreamSwitcherControl<TParent, TPanel> : AbstractRouteSwitcherControl<TParent>, IDgeX00StreamSwitcherControl, IStreamRouteDestinationControl 
		where TParent : IDgeX00Adapter<TPanel>
		where TPanel : Dge100
	{
		private const int STREAM_H264_INPUT_ADDRESS = 1;
		private const int STREAM_MJPEG_INPUT_ADDRESS = 2;
		private const int STREAM_AIRBOARD_INPUT_ADDRESS = 3;
		private const int HDMI_INPUT_ADDRESS = 4;

		private const int OUTPUT_ADDRESS = 1;

#region Events

		public event EventHandler<StreamUriEventArgs> OnInputStreamUriChanged;
		public override event EventHandler<SourceDetectionStateChangeEventArgs> OnSourceDetectionStateChange;
		public override event EventHandler<ActiveInputStateChangeEventArgs> OnActiveInputsChanged;
		public override event EventHandler<RouteChangeEventArgs> OnRouteChange;
		public override event EventHandler<TransmissionStateEventArgs> OnActiveTransmissionStateChanged;

#endregion

#region Properties

		protected SwitcherCache Cache { get { return m_Cache; } }

		protected TPanel Dge { get { return m_Dge; } }

		public RouteDelegate UiRoute { get; set; }

		public ClearOutputDelegate UiClearOutput { get; set; }

		public SetStreamDelegate UiSetStream { get; set; }

#endregion

		private readonly SwitcherCache m_Cache;

		private TPanel m_Dge;

		private readonly Dictionary<int, Uri> m_StreamUris;
		private readonly SafeCriticalSection m_StreamUrisSection;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractDgeStreamSwitcherControl(TParent parent, int id) 
			: base(parent, id)
		{
			m_StreamUris = new Dictionary<int, Uri>();
			m_StreamUrisSection = new SafeCriticalSection();
			
			
			// Switcher Cache
			m_Cache = new SwitcherCache();
			Subscribe(m_Cache);

			parent.OnPanelChanged += ParentOnPanelChanged;

			SetDge(parent.Dge);
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			base.DisposeFinal(disposing);

			if (!disposing)
				return;

			Parent.OnPanelChanged -= ParentOnPanelChanged;
			Unsubscribe(m_Cache);
			SetDge(null);
		}

#region Panel Callbacks

		private void ParentOnPanelChanged(ITriListAdapter sender, BasicTriListWithSmartObject panel)
		{
			SetDge(panel as TPanel);
		}

#endregion

#region Dge Callbacks

		private void SetDge(TPanel dge)
		{
			if (dge == m_Dge)
				return;

			Unsubscribe(m_Dge);
			m_Dge = dge;
			Subscribe(m_Dge);

			SetInitialInputDetectState();
		}

		protected virtual void Subscribe([CanBeNull] TPanel dge)
		{
			if (dge == null)
				return;

			dge.HdmiIn.StreamChange += HdmiInOnStreamChange;
		}

		protected virtual void Unsubscribe([CanBeNull] TPanel dge)
		{
			if (dge == null)
				return;

			dge.HdmiIn.StreamChange -= HdmiInOnStreamChange;
		}

		private void HdmiInOnStreamChange(Stream stream, StreamEventArgs args)
		{
			switch (args.EventId)
			{
				case DMInputEventIds.SourceSyncEventId:
					Cache.SetSourceDetectedState(HDMI_INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video,GetHdmiInputSyncState());
					break;
			}
		}

#endregion

#region Methods

		protected virtual void SetInitialInputDetectState()
		{
			// Set Stream Inputs as always detected
			Cache.SetSourceDetectedState(STREAM_H264_INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video, true);
			Cache.SetSourceDetectedState(STREAM_MJPEG_INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video, true);
			Cache.SetSourceDetectedState(STREAM_AIRBOARD_INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video, true);
			
			Cache.SetSourceDetectedState(HDMI_INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video,GetHdmiInputSyncState());
		}

		private bool GetHdmiInputSyncState()
		{
			return Dge != null && Dge.HdmiIn.SyncDetectedFeedback.BoolValue;
		}

		private void UpdateStreamUriCache(int input, Uri stream)
		{
			bool changed = true;

			m_StreamUrisSection.Enter();
			try
			{
				Uri currentStream;
				// Handle null streams for stream or currentStream
				if (m_StreamUris.TryGetValue(input, out currentStream))
					changed = currentStream == null ? stream != null : !currentStream.Equals(stream);

				m_StreamUris[input] = stream;

			}
			finally
			{
				m_StreamUrisSection.Leave();
			}

			if (changed)
				OnInputStreamUriChanged.Raise(this, new StreamUriEventArgs(eConnectionType.Audio | eConnectionType.Video, input, stream));
		}

		private bool IsInputStreamInput(int input)
		{
			return input == STREAM_H264_INPUT_ADDRESS ||
			       input == STREAM_MJPEG_INPUT_ADDRESS ||
			       input == STREAM_AIRBOARD_INPUT_ADDRESS;
		}

#region AbstractRouteSwitcher Overrides

		/// <summary>
		/// Returns true if a signal is detected at the given input.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public override bool GetSignalDetectedState(int input, eConnectionType type)
		{
			return Cache.GetSourceDetectedState(input, type);
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
			return input == STREAM_H264_INPUT_ADDRESS ||
			       input == STREAM_MJPEG_INPUT_ADDRESS ||
			       input == STREAM_AIRBOARD_INPUT_ADDRESS ||
			       input == HDMI_INPUT_ADDRESS;
		}

		/// <summary>
		/// Returns the inputs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetInputs()
		{
			yield return GetInput(STREAM_H264_INPUT_ADDRESS);
			yield return GetInput(STREAM_MJPEG_INPUT_ADDRESS);
			yield return GetInput(STREAM_AIRBOARD_INPUT_ADDRESS);
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
				throw new ArgumentOutOfRangeException("No input with address " + input);
			
			return Cache.GetOutputsForInput(input, type);
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
				throw new ArgumentOutOfRangeException("No output with address " + output);

			return Cache.GetInputConnectorInfoForOutput(output, type);
		}

		/// <summary>
		/// Performs the given route operation.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public override bool Route([NotNull] RouteOperation info)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			RouteDelegate callback = UiRoute;

			bool result = callback != null && callback(info);

			if (result)
				Cache.SetInputForOutput(info.LocalOutput, info.LocalInput, info.ConnectionType);

			return result;
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
			bool result = callback != null && callback();

			if (result)
				Cache.SetInputForOutput(output, null, type);

			return result;
		}

		public bool SetStreamForInput(int input, Uri stream)
		{
			if (!IsInputStreamInput(input))
				throw new ArgumentOutOfRangeException("No stream input with address " + input);

			var callback = UiSetStream;

			if (callback != null && callback(input, stream))
			{
				UpdateStreamUriCache(input, stream);
				return true;
			}

			return false;
		}

		public Uri GetStreamForInput(int input)
		{
			if (!IsInputStreamInput(input))
				throw new ArgumentOutOfRangeException("No stream for input with address " + input);

			m_StreamUrisSection.Enter();
			try
			{
				Uri streamUri;
				return m_StreamUris.TryGetValue(input, out streamUri) ? streamUri : null;
			}
			finally
			{
				m_StreamUrisSection.Leave();
			}
		}

		public void SetInputActive(int? input)
		{
			Cache.SetInputForOutput(OUTPUT_ADDRESS, input, eConnectionType.Audio | eConnectionType.Video);
		}

#endregion

#endregion


#region Switcher Cache Callbacks

		private void Subscribe(SwitcherCache cache)
		{
			cache.OnActiveInputsChanged += CacheOnActiveInputsChanged;
			cache.OnActiveTransmissionStateChanged += CacheOnActiveTransmissionStateChanged;
			cache.OnRouteChange += CacheOnRouteChange;
			cache.OnSourceDetectionStateChange += CacheOnSourceDetectionStateChange;
		}

		private void Unsubscribe(SwitcherCache cache)
		{
			cache.OnActiveInputsChanged -= CacheOnActiveInputsChanged;
			cache.OnActiveTransmissionStateChanged -= CacheOnActiveTransmissionStateChanged;
			cache.OnRouteChange -= CacheOnRouteChange;
			cache.OnSourceDetectionStateChange -= CacheOnSourceDetectionStateChange;
		}

		private void CacheOnActiveInputsChanged(object sender, ActiveInputStateChangeEventArgs args)
		{
			OnActiveInputsChanged.Raise(this, args);
		}

		private void CacheOnActiveTransmissionStateChanged(object sender, TransmissionStateEventArgs args)
		{
			OnActiveTransmissionStateChanged.Raise(this, args);
		}

		private void CacheOnRouteChange(object sender, RouteChangeEventArgs args)
		{
			OnRouteChange.Raise(this, args);
		}

		private void CacheOnSourceDetectionStateChange(object sender, SourceDetectionStateChangeEventArgs args)
		{
			OnSourceDetectionStateChange.Raise(this, args);
		}

#endregion
	}
}
#endif