#if SIMPLSHARP
using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Common.Properties;
using ICD.Common.Services.Logging;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Conferencing.ConferenceSources;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Conferencing.EventArguments;
using ICD.Connect.Conferencing.Utils;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip
{
	public abstract class AbstractFt5ButtonDialingControl<TParent, TPanel, TVoIpSigs> : AbstractDialingDeviceControl<TParent>
		where TParent : ITswFt5ButtonAdapter
		where TPanel : TswFt5Button
		where TVoIpSigs : VOIPReservedCues
	{
		public override event EventHandler<ConferenceSourceEventArgs> OnSourceAdded;

		private readonly Dictionary<Sig, Action<Sig>> m_SigCallbackMap;

		private ThinConferenceSource m_ActiveSource;

		#region Properties

		/// <summary>
		/// Gets the type of conference this dialer supports.
		/// </summary>
		public override eConferenceSourceType Supports { get { return eConferenceSourceType.Audio; } }

		/// <summary>
		/// Gets the current active source.
		/// </summary>
		[CanBeNull]
		public IConferenceSource ActiveSource { get { return m_ActiveSource; } }

		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		[CanBeNull]
		protected abstract TVoIpSigs Sigs { get; }

		/// <summary>
		/// Gets the current panel for the control.
		/// </summary>
		[CanBeNull]
		protected TPanel Panel { get; private set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractFt5ButtonDialingControl(TParent parent, int id)
			: base(parent, id)
		{
			m_SigCallbackMap = new Dictionary<Sig, Action<Sig>>();

			Subscribe(parent);
			SetPanel(parent.Panel as TPanel);
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			base.DisposeFinal(disposing);

			Unsubscribe(Parent);
			UnsubscribePanel();
		}

		#region Dialer Methods

		/// <summary>
		/// Gets the active conference sources.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConferenceSource> GetSources()
		{
			if (m_ActiveSource != null)
				yield return m_ActiveSource;
		}

		/// <summary>
		/// Dials the given number.
		/// </summary>
		/// <param name="number"></param>
		public override void Dial(string number)
		{
			if (Panel == null)
				throw new InvalidOperationException("No panel");

			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			if (!SipUtils.IsValidNumber(number))
				throw new InvalidOperationException(string.Format("Not a valid SIP number: {0}", number));

			Sigs.DialString.StringValue = number;
			Sigs.DialCurrentNumber();
		}

		/// <summary>
		/// Dials the given number.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="callType"></param>
		public override void Dial(string number, eConferenceSourceType callType)
		{
			if (callType != eConferenceSourceType.Audio)
			{
				string message = string.Format("{0} does not support {1} calls", Parent.GetType().Name, callType);
				throw new InvalidOperationException(message);
			}

			Dial(number);
		}

		/// <summary>
		/// Sets the do-not-disturb enabled state.
		/// </summary>
		/// <param name="enabled"></param>
		public override void SetDoNotDisturb(bool enabled)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			// Do-not-disturb only toggles :/
			bool doNotDisturb = Sigs.DoNotDisturbFeedback.BoolValue;
			if (enabled == doNotDisturb)
				return;

			Sigs.DoNotDisturb();

			DoNotDisturb = Sigs.DoNotDisturbFeedback.BoolValue;
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the parent events.
		/// </summary>
		/// <param name="parent"></param>
		private void Subscribe(TParent parent)
		{
			parent.OnPanelChanged += ParentOnPanelChanged;
		}

		/// <summary>
		/// Unsubscribe from the parent events.
		/// </summary>
		/// <param name="parent"></param>
		private void Unsubscribe(TParent parent)
		{
			parent.OnPanelChanged -= ParentOnPanelChanged;
		}

		/// <summary>
		/// Called when the wrapped panel instance changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="panel"></param>
		private void ParentOnPanelChanged(ITriListAdapter sender, BasicTriListWithSmartObject panel)
		{
			SetPanel(panel as TPanel);
		}

		/// <summary>
		/// Sets the current subscribed panel to match the parent device.
		/// </summary>
		/// <param name="panel"></param>
		private void SetPanel(TPanel panel)
		{
			if (panel == Panel)
				return;

			UnsubscribePanel();

			Panel = panel;

			SubscribePanel();
		}

		#endregion

		#region Panel Callbacks

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		private void SubscribePanel()
		{
			BuildSigCallbacks(m_SigCallbackMap);

			if (Panel == null)
				return;

			var sigs = Sigs;
			if (sigs == null)
				return;

			sigs.DeviceExtenderSigChange += ExtenderVoipReservedSigsOnDeviceExtenderSigChange;
		}

		/// <summary>
		/// Unsubscribe from the panel events.
		/// </summary>
		private void UnsubscribePanel()
		{
			m_SigCallbackMap.Clear();

			if (Panel == null)
				return;

			var sigs = Sigs;
			if (sigs == null)
				return;

			sigs.DeviceExtenderSigChange -= ExtenderVoipReservedSigsOnDeviceExtenderSigChange;
		}

		/// <summary>
		/// Populates the sig callback map with handlers for sig changes.
		/// </summary>
		protected virtual void BuildSigCallbacks(Dictionary<Sig, Action<Sig>> map)
		{
			TVoIpSigs sigs = Sigs;
			if (sigs == null)
				return;

			map[sigs.CallActiveFeedback] = HandleCallActiveFeedback;
			map[sigs.CallTerminatedFeedback] = HandleCallTerminatedFeedback;
			map[sigs.DialingFeedback] = HandleDialingFeedback;
			map[sigs.DoNotDisturbFeedback] = HandleDoNotDisturbFeedback;
			map[sigs.HoldFeedback] = HandleHoldFeedback;
			map[sigs.IncomingCallDetectedFeedback] = HandleIncomingCallDetectedFeedback;
			map[sigs.IncomingCallerInformationFeedback] = HandleIncomingCallerInformationFeedback;
		}

		/// <summary>
		/// Called when a sig on the VOIP extender changes.
		/// </summary>
		/// <param name="extender"></param>
		/// <param name="args"></param>
		private void ExtenderVoipReservedSigsOnDeviceExtenderSigChange(DeviceExtender extender, SigEventArgs args)
		{
			Action<Sig> callback;
			if (m_SigCallbackMap.TryGetValue(args.Sig, out callback))
				callback(args.Sig);
		}

		/// <summary>
		/// Called when the CallActiveFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleCallActiveFeedback(Sig sig)
		{
			if (sig.BoolValue)
				LazyLoadActiveSource();
		}

		/// <summary>
		/// Called when the CallTerminated sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleCallTerminatedFeedback(Sig sig)
		{
			if (sig.BoolValue)
				ClearActiveSource();
		}

		/// <summary>
		/// Called when the CallActiveFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleDialingFeedback(Sig sig)
		{
			if (sig.BoolValue)
				LazyLoadActiveSource();
		}

		/// <summary>
		/// Called when the CallActiveFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleDoNotDisturbFeedback(Sig sig)
		{
			DoNotDisturb = sig.BoolValue;
		}

		/// <summary>
		/// Called when the HoldFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleHoldFeedback(Sig sig)
		{
			UpdateActiveSource();
		}

		/// <summary>
		/// Called when the IncomingCallDetectedFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleIncomingCallDetectedFeedback(Sig sig)
		{
			if (sig.BoolValue)
				LazyLoadActiveSource();
		}

		/// <summary>
		/// Called when the IncomingCallerInformationFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleIncomingCallerInformationFeedback(Sig sig)
		{
			UpdateActiveSource();
		}

		/// <summary>
		/// Updates the active source based on the current sig states.
		/// </summary>
		private void UpdateActiveSource()
		{
			if (m_ActiveSource == null)
				return;

			var sigs = Sigs;
			if (sigs == null)
				return;

			// Caller number
			string uri = sigs.IncomingCallerInformationFeedback.StringValue;
			if (!string.IsNullOrEmpty(uri))
			{
				m_ActiveSource.Name = uri;
				m_ActiveSource.Number = SipUtils.NumberFromUri(uri);
			}

			// Status
			if (sigs.CallActiveFeedback.BoolValue)
			{
				m_ActiveSource.Status = sigs.HoldFeedback.BoolValue
										? eConferenceSourceStatus.OnHold
										: eConferenceSourceStatus.Connected;
			}
			else
			{
				m_ActiveSource.Status = sigs.DialingFeedback.BoolValue
					                        ? eConferenceSourceStatus.Dialing
					                        : eConferenceSourceStatus.Disconnecting;
			}

			// Direction
			if (sigs.IncomingCallDetectedFeedback.BoolValue)
				m_ActiveSource.Direction = eConferenceSourceDirection.Incoming;
			if (m_ActiveSource.Direction != eConferenceSourceDirection.Incoming)
				m_ActiveSource.Direction = eConferenceSourceDirection.Outgoing;

			// Start/End
			switch (m_ActiveSource.Status)
			{
				case eConferenceSourceStatus.Connected:
					m_ActiveSource.Start = m_ActiveSource.Start ?? IcdEnvironment.GetLocalTime();
					break;
				case eConferenceSourceStatus.Disconnected:
					m_ActiveSource.End = m_ActiveSource.End ?? IcdEnvironment.GetLocalTime();
					break;
			}

			// Answer state
			if (sigs.CallActiveFeedback.BoolValue)
				m_ActiveSource.AnswerState = eConferenceSourceAnswerState.Answered;
			if (m_ActiveSource.AnswerState != eConferenceSourceAnswerState.Answered)
				m_ActiveSource.AnswerState = eConferenceSourceAnswerState.Unanswered;
		}

		#endregion

		#region Source Callbacks

		/// <summary>
		/// If the active source is currently null, creates a new source.
		/// Updates the active source.
		/// </summary>
		private void LazyLoadActiveSource()
		{
			bool instantiated = false;

			if (m_ActiveSource == null)
			{
				m_ActiveSource = new ThinConferenceSource();
				instantiated = true;

				Subscribe(m_ActiveSource);
			}

			UpdateActiveSource();

			if (instantiated)
				OnSourceAdded.Raise(this, new ConferenceSourceEventArgs(m_ActiveSource));
		}

		/// <summary>
		/// Clears the current active source to null.
		/// </summary>
		private void ClearActiveSource()
		{
			if (m_ActiveSource == null)
				return;

			Unsubscribe(m_ActiveSource);

			m_ActiveSource.Status = eConferenceSourceStatus.Disconnected;
			m_ActiveSource.End = IcdEnvironment.GetLocalTime();

			m_ActiveSource = null;
		}

		/// <summary>
		/// Subscribe to the source callbacks.
		/// </summary>
		/// <param name="source"></param>
		private void Subscribe(ThinConferenceSource source)
		{
			source.OnSendDtmfCallback += SendDtmfCallback;
			source.OnHangupCallback += HangupCallback;
			source.OnResumeCallback += ResumeCallback;
			source.OnHoldCallback += HoldCallback;
			source.OnAnswerCallback += AnswerCallback;
		}

		/// <summary>
		/// Unsubscribe from the source callbacks.
		/// </summary>
		/// <param name="source"></param>
		private void Unsubscribe(ThinConferenceSource source)
		{
			source.OnSendDtmfCallback -= SendDtmfCallback;
			source.OnHangupCallback -= HangupCallback;
			source.OnResumeCallback -= ResumeCallback;
			source.OnHoldCallback -= HoldCallback;
			source.OnAnswerCallback -= AnswerCallback;
		}

		/// <summary>
		/// Sends a DTMF string to the current source.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void SendDtmfCallback(object sender, StringEventArgs eventArgs)
		{
			foreach (char item in eventArgs.Data)
				SendDtmfCallback(item);
		}

		/// <summary>
		/// Sends the DTMF character to the current source.
		/// </summary>
		/// <param name="digit"></param>
		private void SendDtmfCallback(char digit)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			switch (digit)
			{
				case '0':
					Sigs.Dial0();
					break;

				case '1':
					Sigs.Dial1();
					break;

				case '2':
					Sigs.Dial2();
					break;

				case '3':
					Sigs.Dial3();
					break;

				case '4':
					Sigs.Dial4();
					break;

				case '5':
					Sigs.Dial5();
					break;

				case '6':
					Sigs.Dial6();
					break;

				case '7':
					Sigs.Dial7();
					break;

				case '8':
					Sigs.Dial8();
					break;

				case '9':
					Sigs.Dial9();
					break;

				case '#':
					Sigs.DialPound();
					break;

				case '*':
					Sigs.DialAsterisk();
					break;
			}
		}

		/// <summary>
		/// Hangs-up the current source.
		/// </summary>
		private void HangupCallback(object sender, EventArgs eventArgs)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Reject();
			Sigs.Hangup();
		}

		/// <summary>
		/// Resumes the current source from hold.
		/// </summary>
		private void ResumeCallback(object sender, EventArgs eventArgs)
		{
			// Is this possible?
			Logger.AddEntry(eSeverity.Warning, "{0} - Resume is unsupported", this);
		}

		/// <summary>
		/// Places the current source on hold.
		/// </summary>
		private void HoldCallback(object sender, EventArgs eventArgs)
		{
			// Is this possible?
			Logger.AddEntry(eSeverity.Warning, "{0} - Hold is unsupported", this);
		}

		/// <summary>
		/// Answers the current incoming source.
		/// </summary>
		private void AnswerCallback(object sender, EventArgs eventArgs)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Answer();
		}

		#endregion
	}
}
#endif
