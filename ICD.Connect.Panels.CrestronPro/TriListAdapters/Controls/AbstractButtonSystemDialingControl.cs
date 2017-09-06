using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Common.Properties;
using ICD.Connect.Conferencing.ConferenceSources;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Conferencing.EventArguments;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X50;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls
{
	public abstract class AbstractButtonSystemDialingControl<TParent, TPanel, TVoIpSigs> : AbstractDialingDeviceControl<TParent>
		where TParent : ITswFt5ButtonAdapter
		where TPanel : TswFt5Button
		where TVoIpSigs : VOIPReservedCues
	{
		public override event EventHandler<ConferenceSourceEventArgs> OnSourceAdded;

		private readonly Dictionary<Sig, Action<Sig>> m_SigCallbackMap;

		private TPanel m_SubscribedPanel;
		private ThinConferenceSource m_ActiveSource;

		#region Properties

		/// <summary>
		/// Gets the type of conference this dialer supports.
		/// </summary>
		public override eConferenceSourceType Supports { get { return eConferenceSourceType.Audio; } }

		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		[CanBeNull]
		protected abstract TVoIpSigs Sigs { get; }

		/// <summary>
		/// Gets the current panel for the control.
		/// </summary>
		[CanBeNull]
		protected TPanel Panel { get { return m_SubscribedPanel; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractButtonSystemDialingControl(TParent parent, int id)
			: base(parent, id)
		{
			m_SigCallbackMap = new Dictionary<Sig, Action<Sig>>();

			Subscribe(parent);
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

		/// <summary>
		/// Instantiates a new source.
		/// </summary>
		/// <returns></returns>
		private ThinConferenceSource InstantiateSource()
		{
			return new ThinConferenceSource
			{
				AnswerCallback = AnswerCallback,
				HoldCallback = HoldCallback,
				ResumeCallback = ResumeCallback,
				HangupCallback = HangupCallback,
				SendDtmfCallback = SendDtmfCallback,
			};
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
			if (m_SubscribedPanel == null)
				throw new InvalidOperationException("No panel");

			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

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
			UnsubscribePanel();

			m_SubscribedPanel = panel as TPanel;
			BuildSigCallbacks();

			SubscribePanel();
		}

		#endregion

		#region Panel Callbacks

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		private void SubscribePanel()
		{
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
		private void BuildSigCallbacks()
		{
			m_SigCallbackMap.Clear();

			TVoIpSigs sigs = Sigs;
			if (sigs == null)
				return;

			m_SigCallbackMap[sigs.BusyFeedback] = HandleBusyFeedback;
			m_SigCallbackMap[sigs.CallActiveFeedback] = HandleCallActiveFeedback;
			m_SigCallbackMap[sigs.CallTerminatedFeedback] = HandleCallTerminatedFeedback;
			//m_SigCallbackMap[sigs.CommandStringFeedback] = HandleCommandStringFeedback;
			m_SigCallbackMap[sigs.ConnectedFeedback] = HandleConnectedFeedback;
			m_SigCallbackMap[sigs.DialingFeedback] = HandleDialingFeedback;
			m_SigCallbackMap[sigs.DoNotDisturbFeedback] = HandleDoNotDisturbFeedback;
			m_SigCallbackMap[sigs.HoldFeedback] = HandleHoldFeedback;
			m_SigCallbackMap[sigs.IncomingCallDetectedFeedback] = HandleIncomingCallDetectedFeedback;
			m_SigCallbackMap[sigs.IncomingCallerInformationFeedback] = HandleIncomingCallerInformationFeedback;
			//m_SigCallbackMap[sigs.MutedFeedback] = HandleMutedFeedback;
			//m_SigCallbackMap[sigs.MyURIFeedback] = HandleMyUriFeedback;
			//m_SigCallbackMap[sigs.PTTFeedback] = HandlePttFeedback;
			//m_SigCallbackMap[sigs.PTTModeFeedback] = HandlePttModeFeedback;
			m_SigCallbackMap[sigs.RingbackFeedback] = HandleRingbackFeedback;
			m_SigCallbackMap[sigs.RingingFeedback] = HandleRingingFeedback;
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

		private void HandleBusyFeedback(Sig obj)
		{
			throw new NotImplementedException();
		}

		private void HandleCallActiveFeedback(Sig obj)
		{
			throw new NotImplementedException();
		}

		private void HandleCallTerminatedFeedback(Sig obj)
		{
			throw new NotImplementedException();
		}

		private void HandleCommandStringFeedback(Sig obj)
		{
			throw new NotImplementedException();
		}

		private void HandleConnectedFeedback(Sig obj)
		{
			UpdateActiveSource();
		}

		private void HandleDialingFeedback(Sig obj)
		{
			UpdateActiveSource();
		}

		private void HandleDoNotDisturbFeedback(Sig sig)
		{
			DoNotDisturb = sig.BoolValue;
		}

		private void HandleHoldFeedback(Sig obj)
		{
			UpdateActiveSource();
		}

		private void HandleIncomingCallDetectedFeedback(Sig obj)
		{
			if (m_ActiveSource != null)
				m_ActiveSource.Number = obj.StringValue;
		}

		private void HandleIncomingCallerInformationFeedback(Sig obj)
		{
			UpdateActiveSource();
		}

		private void HandleMutedFeedback(Sig sig)
		{
			PrivacyMuted = sig.BoolValue;
		}

		private void HandleRingbackFeedback(Sig sig)
		{
			UpdateActiveSource();
		}

		private void HandleRingingFeedback(Sig sig)
		{
			UpdateActiveSource();
		}

		private void UpdateActiveSource()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Source Callbacks

		/// <summary>
		/// Sends a DTMF string to the current source.
		/// </summary>
		/// <param name="digits"></param>
		private void SendDtmfCallback(string digits)
		{
			foreach (char item in digits)
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
		private void HangupCallback()
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Hangup();
		}

		/// <summary>
		/// Resumes the current source from hold.
		/// </summary>
		private void ResumeCallback()
		{
			// Is this possible?
		}

		/// <summary>
		/// Places the current source on hold.
		/// </summary>
		private void HoldCallback()
		{
			// Is this possible?
		}

		/// <summary>
		/// Answers the current incoming source.
		/// </summary>
		private void AnswerCallback()
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Answer();
		}

		#endregion
	}
}
