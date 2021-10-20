#if !NETSTANDARD
using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.UI;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Conferencing.Conferences;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Conferencing.DialContexts;
using ICD.Connect.Conferencing.EventArguments;
using ICD.Connect.Conferencing.IncomingCalls;
using ICD.Connect.Conferencing.Participants.Enums;
using ICD.Connect.Conferencing.Utils;
using ICD.Connect.Misc.CrestronPro.Extensions;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip
{
	public abstract class AbstractTswXX70BaseConferenceControl<TParent, TPanel, TVoIpSigs> :
		AbstractConferenceDeviceControl<TParent, ThinConference>
		where TParent : ITswXX70BaseAdapter
		where TPanel : TswXX70Base
		where TVoIpSigs : VOIPReservedCues
	{
		public override event EventHandler<GenericEventArgs<IIncomingCall>> OnIncomingCallAdded;
		public override event EventHandler<GenericEventArgs<IIncomingCall>> OnIncomingCallRemoved;

		public override event EventHandler<ConferenceEventArgs> OnConferenceAdded;
		public override event EventHandler<ConferenceEventArgs> OnConferenceRemoved;

		private readonly Dictionary<Sig, Action<Sig>> m_SigCallbackMap;

		private ThinConference m_ActiveConference;
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
		protected AbstractTswXX70BaseConferenceControl(TParent parent, int id)
			: base(parent, id)
		{
			m_SigCallbackMap = new Dictionary<Sig, Action<Sig>>();

			SetPanel(parent.Panel as TPanel);

			SupportedConferenceControlFeatures |= eConferenceControlFeatures.DoNotDisturb;
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnIncomingCallAdded = null;
			OnIncomingCallRemoved = null;
			OnConferenceAdded = null;
			OnConferenceRemoved = null;

			UnsubscribePanel();

			if (m_ActiveConference != null)
				ClearActiveConference();

			base.DisposeFinal(disposing);


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
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

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

		/// <summary>
		/// Sets the camera mute state.
		/// </summary>
		/// <param name="mute"></param>
		public override void SetCameraMute(bool mute)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the active conference sources.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ThinConference> GetConferences()
		{
			yield return m_ActiveConference;
		}

		/// <summary>
		/// Starts a personal meeting.
		/// </summary>
		public override void StartPersonalMeeting()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Locks the current active conference so no more participants may join.
		/// </summary>
		/// <param name="enabled"></param>
		public override void EnableCallLock(bool enabled)
		{
			throw new NotSupportedException();
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
				LazyLoadActiveConference();
		}

		/// <summary>
		/// Called when the CallTerminated sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleCallTerminatedFeedback(Sig sig)
		{
			if (sig.BoolValue)
				ClearActiveConference();
		}

		/// <summary>
		/// Called when the DialingFeedback sig changes state.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleDialingFeedback(Sig sig)
		{
			if (sig.BoolValue)
				LazyLoadActiveConference();
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
			UpdateActiveConference();
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
		private void UpdateActiveConference()
		{
			if (m_ActiveConference == null)
				return;

			TVoIpSigs sigs = Sigs;
			if (sigs == null)
				return;

			// Caller number
			string uri = sigs.IncomingCallerInformationFeedback.GetSerialValueOrDefault();
			if (!StringUtils.IsNullOrWhitespace(uri))
			{
				m_ActiveConference.Name = uri;
				m_ActiveConference.Number = SipUtils.NumberFromUri(uri);
			}

			// Status
			if (sigs.CallActiveFeedback.GetBoolValueOrDefault())
			{
				m_ActiveConference.Status = sigs.HoldFeedback.GetBoolValueOrDefault()
					                            ? eConferenceStatus.OnHold
					                            : eConferenceStatus.Connected;
			}
			else
			{
				m_ActiveConference.Status = sigs.DialingFeedback.GetBoolValueOrDefault()
					                            ? eConferenceStatus.Connecting
					                            : eConferenceStatus.Disconnected;
			}

			// Direction
			if (sigs.IncomingCallDetectedFeedback.GetBoolValueOrDefault())
				m_ActiveConference.Direction = (eCallDirection.Incoming);
			if (m_ActiveConference.Direction != eCallDirection.Incoming)
				m_ActiveConference.Direction = (eCallDirection.Outgoing);

			// Start/End
			switch (m_ActiveConference.Status)
			{
				case eConferenceStatus.Connected:
					m_ActiveConference.StartTime = (m_ActiveConference.StartTime ?? IcdEnvironment.GetUtcTime());
					break;
				case eConferenceStatus.Disconnected:
					m_ActiveConference.EndTime = (m_ActiveConference.EndTime ?? IcdEnvironment.GetUtcTime());
					break;
			}
		}

		#endregion

		#region Source Callbacks

		/// <summary>
		/// If the active participant is currently null, creates a new participant.
		/// Updates the active participant.
		/// </summary>
		private void LazyLoadActiveConference()
		{
			bool added = false;
			ThinConference conference = m_ActiveConference;
			if (conference == null)
			{
				conference = new ThinConference
				{
					CallType = eCallType.Audio
				};

				Subscribe(conference);
				m_ActiveConference = conference;
				added = true;
			}

			UpdateActiveConference();

			if (added)
				OnConferenceAdded.Raise(this, conference);
		}

		/// <summary>
		/// Clears the current active participant to null.
		/// </summary>
		private void ClearActiveConference()
		{
			if (m_ActiveConference == null)
				return;

			Unsubscribe(m_ActiveConference);

			m_ActiveConference.Status = eConferenceStatus.Disconnected;
			m_ActiveConference.EndTime = IcdEnvironment.GetUtcTime();

			ThinConference removed = m_ActiveConference;

			m_ActiveConference = null;

			OnConferenceRemoved.Raise(this, removed);

			removed.Dispose();
		}

		private void Subscribe(ThinConference conference)
		{
			if (conference == null)
				return;

			conference.SendDtmfCallback = SendDtmfCallback;
			conference.LeaveConferenceCallback = HangupCallback;
		}

		private void Unsubscribe(ThinConference conference)
		{
			conference.SendDtmfCallback = null;
			conference.LeaveConferenceCallback = null;
		}

		private void SendDtmfCallback(ThinConference sender, string data)
		{
			foreach (char item in data)
				SendDtmfCallback(item);
		}

		private void HangupCallback(ThinConference sender)
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
				OnIncomingCallAdded.Raise(this, m_IncomingCall);
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

			OnIncomingCallRemoved.Raise(this, m_IncomingCall);

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
