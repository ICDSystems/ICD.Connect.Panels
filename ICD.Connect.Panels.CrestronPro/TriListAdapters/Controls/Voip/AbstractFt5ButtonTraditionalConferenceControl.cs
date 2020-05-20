#if SIMPLSHARP
using ICD.Connect.Conferencing.IncomingCalls;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Conferencing.DialContexts;
using ICD.Connect.Conferencing.Participants;
using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Conferencing.EventArguments;
using ICD.Connect.Conferencing.Utils;
using ICD.Connect.Misc.CrestronPro.Extensions;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip
{
	public abstract class AbstractFt5ButtonTraditionalConferenceControl<TParent, TPanel, TVoIpSigs> :
		AbstractTraditionalConferenceDeviceControl<TParent>
		where TParent : ITswFt5ButtonAdapter
		where TPanel : TswFt5Button
		where TVoIpSigs : VOIPReservedCues
	{
		public override event EventHandler<GenericEventArgs<IIncomingCall>> OnIncomingCallAdded;
		public override event EventHandler<GenericEventArgs<IIncomingCall>> OnIncomingCallRemoved;

		private readonly Dictionary<Sig, Action<Sig>> m_SigCallbackMap;

		private ThinTraditionalParticipant m_ActiveParticipant;
		private TraditionalIncomingCall m_IncomingCall;

		#region Properties

		/// <summary>
		/// Gets the type of conference this dialer supports.
		/// </summary>
		public override eCallType Supports { get { return eCallType.Audio; } }

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
		protected AbstractFt5ButtonTraditionalConferenceControl(TParent parent, int id)
			: base(parent, id)
		{
			m_SigCallbackMap = new Dictionary<Sig, Action<Sig>>();

			SetPanel(parent.Panel as TPanel);

			SupportedConferenceFeatures |= eConferenceFeatures.DoNotDisturb;
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnIncomingCallAdded = null;
			OnIncomingCallRemoved = null;

			base.DisposeFinal(disposing);

			UnsubscribePanel();
		}

		#region Dialer Methods

		/// <summary>
		/// Returns the level of support the device has for the given booking.
		/// </summary>
		/// <param name="dialContext"></param>
		/// <returns></returns>
		public override eDialContextSupport CanDial(IDialContext dialContext)
		{
			if (dialContext == null)
				throw new ArgumentNullException("dialContext");

			if (dialContext.Protocol == eDialProtocol.Sip && SipUtils.IsValidSipUri(dialContext.DialString))
				return eDialContextSupport.Supported;

			if (dialContext.Protocol == eDialProtocol.Unknown && !string.IsNullOrEmpty(dialContext.DialString))
				return eDialContextSupport.Unknown;

			return eDialContextSupport.Unsupported;
		}

		/// <summary>
		/// Dials the given booking.
		/// </summary>
		/// <param name="dialContext"></param>
		public override void Dial(IDialContext dialContext)
		{
			Sigs.DialString.StringValue = dialContext.DialString;
			Sigs.DialCurrentNumber();
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
			bool doNotDisturb = Sigs.DoNotDisturbFeedback.GetBoolValueOrDefault();
			if (enabled == doNotDisturb)
				return;

			Sigs.DoNotDisturb();

			DoNotDisturb = Sigs.DoNotDisturbFeedback.GetBoolValueOrDefault();
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Subscribe(TParent parent)
		{
			base.Subscribe(parent);

			parent.OnPanelChanged += ParentOnPanelChanged;
		}

		/// <summary>
		/// Unsubscribe from the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Unsubscribe(TParent parent)
		{
			base.Unsubscribe(parent);

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

			TVoIpSigs sigs = Sigs;
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

			TVoIpSigs sigs = Sigs;
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

			map[sigs.MyURIFeedback] = HandleMyUriFeedback;
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
		/// Called when the MyUriFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleMyUriFeedback(Sig sig)
		{
			CallInInfo =
				new DialContext
				{
					Protocol = eDialProtocol.Sip,
					CallType = eCallType.Audio,
					DialString = sig.StringValue
				};
		}

		/// <summary>
		/// Called when the CallActiveFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleCallActiveFeedback(Sig sig)
		{
			if (sig.BoolValue)
				LazyLoadActiveParticipant();
		}

		/// <summary>
		/// Called when the CallTerminated sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleCallTerminatedFeedback(Sig sig)
		{
			if (sig.BoolValue)
				ClearActiveParticipant();
		}

		/// <summary>
		/// Called when the DialingFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleDialingFeedback(Sig sig)
		{
			if (sig.BoolValue)
				LazyLoadActiveParticipant();
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
				LazyLoadIncomingCall();
		}

		/// <summary>
		/// Called when the IncomingCallerInformationFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleIncomingCallerInformationFeedback(Sig sig)
		{
			UpdateIncomingCall();
		}

		/// <summary>
		/// Updates the active participant based on the current sig states.
		/// </summary>
		private void UpdateActiveSource()
		{
			if (m_ActiveParticipant == null)
				return;

			TVoIpSigs sigs = Sigs;
			if (sigs == null)
				return;

			// Caller number
			string uri = sigs.IncomingCallerInformationFeedback.GetSerialValueOrDefault();
			if (!StringUtils.IsNullOrWhitespace(uri))
			{
				m_ActiveParticipant.SetName(uri);
				m_ActiveParticipant.SetNumber(SipUtils.NumberFromUri(uri));
			}

			// Status
			if (sigs.CallActiveFeedback.GetBoolValueOrDefault())
			{
				m_ActiveParticipant.SetStatus(sigs.HoldFeedback.GetBoolValueOrDefault()
					                        ? eParticipantStatus.OnHold
					                        : eParticipantStatus.Connected);
			}
			else
			{
				m_ActiveParticipant.SetStatus(sigs.DialingFeedback.GetBoolValueOrDefault()
					                        ? eParticipantStatus.Dialing
					                        : eParticipantStatus.Disconnecting);
			}

			// Direction
			if (sigs.IncomingCallDetectedFeedback.GetBoolValueOrDefault())
				m_ActiveParticipant.SetDirection(eCallDirection.Incoming);
			if (m_ActiveParticipant.Direction != eCallDirection.Incoming)
				m_ActiveParticipant.SetDirection(eCallDirection.Outgoing);

			// Start/End
			switch (m_ActiveParticipant.Status)
			{
				case eParticipantStatus.Connected:
					m_ActiveParticipant.SetStart(m_ActiveParticipant.Start ?? IcdEnvironment.GetUtcTime());
					break;
				case eParticipantStatus.Disconnected:
					m_ActiveParticipant.SetEnd(m_ActiveParticipant.End ?? IcdEnvironment.GetUtcTime());
					break;
			}
		}

		#endregion

		#region Source Callbacks

		/// <summary>
		/// If the active participant is currently null, creates a new participant.
		/// Updates the active participant.
		/// </summary>
		private void LazyLoadActiveParticipant()
		{
			if (m_ActiveParticipant == null)
			{
				m_ActiveParticipant = new ThinTraditionalParticipant();
				m_ActiveParticipant.SetCallType(eCallType.Audio);
				AddParticipant(m_ActiveParticipant);
				Subscribe(m_ActiveParticipant);
			}

			UpdateActiveSource();
		}

		/// <summary>
		/// Clears the current active participant to null.
		/// </summary>
		private void ClearActiveParticipant()
		{
			if (m_ActiveParticipant == null)
				return;

			Unsubscribe(m_ActiveParticipant);

			m_ActiveParticipant.SetStatus(eParticipantStatus.Disconnected);
			m_ActiveParticipant.SetEnd(IcdEnvironment.GetUtcTime());

			RemoveParticipant(m_ActiveParticipant);

			m_ActiveParticipant = null;
		}

		/// <summary>
		/// Subscribe to the participant callbacks.
		/// </summary>
		/// <param name="participant"></param>
		private void Subscribe(ThinTraditionalParticipant participant)
		{
			participant.HoldCallback += HoldCallback;
			participant.ResumeCallback += ResumeCallback;
			participant.SendDtmfCallback += SendDtmfCallback;
			participant.HangupCallback += HangupCallback;
		}

		/// <summary>
		/// Unsubscribe from the participant callbacks.
		/// </summary>
		/// <param name="source"></param>
		private void Unsubscribe(ThinTraditionalParticipant source)
		{
			source.HoldCallback = null;
			source.ResumeCallback = null;
			source.SendDtmfCallback = null;
			source.HangupCallback = null;
		}

		private void HoldCallback(ThinTraditionalParticipant sender)
		{
			// Is this possible?
			Logger.Log(eSeverity.Warning, "Hold is unsupported");
		}

		private void ResumeCallback(ThinTraditionalParticipant sender)
		{
			// Is this possible?
			Logger.Log(eSeverity.Warning, "Resume is unsupported");
		}

		private void SendDtmfCallback(ThinTraditionalParticipant sender, string data)
		{
			foreach (char item in data)
				SendDtmfCallback(item);
		}

		private void HangupCallback(ThinTraditionalParticipant sender)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Reject();
			Sigs.Hangup();
		}

		/// <summary>
		/// Sends the DTMF character to the current participant.
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

		#endregion

		#region Incoming Call Callbacks

		private void LazyLoadIncomingCall()
		{
			if (m_IncomingCall == null)
			{
				m_IncomingCall = new TraditionalIncomingCall(eCallType.Audio);
				Subscribe(m_IncomingCall);
				OnIncomingCallAdded.Raise(this, new GenericEventArgs<IIncomingCall>(m_IncomingCall));
			}

			UpdateIncomingCall();
		}

		private void UpdateIncomingCall()
		{
			if (m_IncomingCall == null)
				return;

			TVoIpSigs sigs = Sigs;
			if (sigs == null)
				return;

			// Caller number
			string uri = sigs.IncomingCallerInformationFeedback.StringValue;
			if (!StringUtils.IsNullOrWhitespace(uri))
			{
				m_IncomingCall.Name = uri;
				m_IncomingCall.Number = SipUtils.NumberFromUri(uri);
			}

			// Direction
			if (sigs.IncomingCallDetectedFeedback.BoolValue)
				m_IncomingCall.Direction = eCallDirection.Incoming;
			if (m_IncomingCall.Direction != eCallDirection.Incoming)
				m_IncomingCall.Direction = eCallDirection.Outgoing;

			// Answer state
			if (sigs.CallActiveFeedback.BoolValue)
				m_IncomingCall.AnswerState = eCallAnswerState.Answered;
			if (m_IncomingCall.AnswerState != eCallAnswerState.Answered)
				m_IncomingCall.AnswerState = eCallAnswerState.Unanswered;
		}

		private void ClearIncomingCall()
		{
			if (m_IncomingCall == null)
				return;

			Unsubscribe(m_IncomingCall);

			OnIncomingCallRemoved.Raise(this, new GenericEventArgs<IIncomingCall>(m_IncomingCall));

			m_IncomingCall = null;
		}

		private void Subscribe(TraditionalIncomingCall call)
		{
			call.AnswerCallback += AnswerCallback;
			call.RejectCallback += RejectCallback;
		}

		private void Unsubscribe(TraditionalIncomingCall call)
		{
			call.AnswerCallback = null;
			call.RejectCallback = null;
		}

		private void AnswerCallback(IIncomingCall sender)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Answer();
			ClearIncomingCall();
		}

		/// <summary>
		/// Rejects the current incoming participant.
		/// </summary>
		private void RejectCallback(IIncomingCall sender)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Reject();
			ClearIncomingCall();
		}

		#endregion
	}
}

#endif
