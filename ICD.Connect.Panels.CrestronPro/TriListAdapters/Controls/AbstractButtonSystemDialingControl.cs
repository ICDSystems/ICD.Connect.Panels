using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Common.Properties;
using ICD.Common.Services.Logging;
using ICD.Common.Utils;
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
		private const string SIP_NUMBER_REGEX = @"^sip:([^@]*)@";

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
		protected AbstractButtonSystemDialingControl(TParent parent, int id)
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

			map[sigs.BusyFeedback] = HandleBusyFeedback;
			map[sigs.CallActiveFeedback] = HandleCallActiveFeedback;
			map[sigs.CallTerminatedFeedback] = HandleCallTerminatedFeedback;
			map[sigs.ConnectedFeedback] = HandleConnectedFeedback;
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

		private void HandleBusyFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Busy: {0}", sig.BoolValue);
		}

		private void HandleCallActiveFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Call active: {0}", sig.BoolValue);
		}

		private void HandleCallTerminatedFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Call terminated: {0}", sig.BoolValue);
		}

		private void HandleConnectedFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Connected: {0}", sig.BoolValue);
			UpdateActiveSource();
		}

		private void HandleDialingFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Dialing: {0}", sig.BoolValue);
			UpdateActiveSource();
		}

		private void HandleDoNotDisturbFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Do not disturb: {0}", sig.BoolValue);
			DoNotDisturb = sig.BoolValue;
		}

		private void HandleHoldFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Hold: {0}", sig.BoolValue);
			UpdateActiveSource();
		}

		private void HandleIncomingCallDetectedFeedback(Sig sig)
		{
			IcdConsole.PrintLine("Incoming call detected: {0}", sig.BoolValue);

			if (m_ActiveSource != null)
				m_ActiveSource.Number = sig.StringValue;
		}

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
				m_ActiveSource.Number = NumberFromUri(uri);
			}

			m_ActiveSource.Status = StatusFromSigs(sigs);
			m_ActiveSource.Direction = DirectionFromSigs(sigs);
			m_ActiveSource.AnswerState = AnswerStateFromSigs(sigs);

			// TODO
			m_ActiveSource.Start = null;
			m_ActiveSource.End = null;
		}

		/// <summary>
		/// Gets the conference source status from the current sig states.
		/// </summary>
		/// <param name="sigs"></param>
		/// <returns></returns>
		private static eConferenceSourceStatus StatusFromSigs(TVoIpSigs sigs)
		{
			if (sigs == null)
				throw new ArgumentNullException("sigs");

			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the direction from the current sig states.
		/// </summary>
		/// <param name="sigs"></param>
		/// <returns></returns>
		private static eConferenceSourceDirection DirectionFromSigs(TVoIpSigs sigs)
		{
			if (sigs == null)
				throw new ArgumentNullException("sigs");

			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the answer state from the current sig states.
		/// </summary>
		/// <param name="sigs"></param>
		/// <returns></returns>
		private static eConferenceSourceAnswerState AnswerStateFromSigs(TVoIpSigs sigs)
		{
			if (sigs == null)
				throw new ArgumentNullException("sigs");

			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns the number portion from the given uri.
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		private static string NumberFromUri(string uri)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");

			Regex regex = new Regex(SIP_NUMBER_REGEX);
			Match match = regex.Match(uri);

			return match.Success ? match.Groups[1].Value : null;
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
			Logger.AddEntry(eSeverity.Warning, "{0} - Resume is unsupported", this);
		}

		/// <summary>
		/// Places the current source on hold.
		/// </summary>
		private void HoldCallback()
		{
			// Is this possible?
			Logger.AddEntry(eSeverity.Warning, "{0} - Hold is unsupported", this);
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
