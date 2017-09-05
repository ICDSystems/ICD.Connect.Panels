using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Conferencing.ConferenceSources;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Conferencing.EventArguments;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls
{
	public sealed class TswFt5ButtonDialingControl : AbstractDialingDeviceControl<ITswFt5ButtonAdapter>
	{
		public override event EventHandler<ConferenceSourceEventArgs> OnSourceAdded;

		private TswFt5Button m_SubscribedPanel;
		private ThinConferenceSource m_ActiveSource;

		#region Properties

		/// <summary>
		/// Gets the type of conference this dialer supports.
		/// </summary>
		public override eConferenceSourceType Supports { get { return eConferenceSourceType.Audio; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswFt5ButtonDialingControl(ITswFt5ButtonAdapter parent, int id)
			: base(parent, id)
		{
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
			Unsubscribe(m_SubscribedPanel);
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

			m_SubscribedPanel.ExtenderVoipReservedSigs.DialString.StringValue = number;
			m_SubscribedPanel.ExtenderVoipReservedSigs.DialCurrentNumber();
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
			if (m_SubscribedPanel == null)
				throw new InvalidOperationException("No panel");

			// Do-not-disturb only toggles :/
			bool doNotDisturb = m_SubscribedPanel.ExtenderVoipReservedSigs.DoNotDisturbFeedback.BoolValue;
			if (enabled == doNotDisturb)
				return;

			m_SubscribedPanel.ExtenderVoipReservedSigs.DoNotDisturb();

			DoNotDisturb = m_SubscribedPanel.ExtenderVoipReservedSigs.DoNotDisturbFeedback.BoolValue;
		}

		/// <summary>
		/// Sets the auto-answer enabled state.
		/// </summary>
		/// <param name="enabled"></param>
		public override void SetAutoAnswer(bool enabled)
		{
			if (m_SubscribedPanel == null)
				throw new InvalidOperationException("No panel");

			// We will handle auto-answer ourselves
			AutoAnswer = enabled;
		}

		/// <summary>
		/// Sets the privacy mute enabled state.
		/// </summary>
		/// <param name="enabled"></param>
		public override void SetPrivacyMute(bool enabled)
		{
			if (m_SubscribedPanel == null)
				throw new InvalidOperationException("No panel");

			m_SubscribedPanel.ExtenderVoipReservedSigs.Muted.BoolValue = enabled;

			PrivacyMuted = m_SubscribedPanel.ExtenderVoipReservedSigs.MutedFeedback.BoolValue;
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the parent events.
		/// </summary>
		/// <param name="parent"></param>
		private void Subscribe(ITswFt5ButtonAdapter parent)
		{
			parent.OnPanelChanged += ParentOnPanelChanged;
		}

		/// <summary>
		/// Unsubscribe from the parent events.
		/// </summary>
		/// <param name="parent"></param>
		private void Unsubscribe(ITswFt5ButtonAdapter parent)
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
			Unsubscribe(m_SubscribedPanel);
			m_SubscribedPanel = panel as TswFt5Button;
			Subscribe(m_SubscribedPanel);
		}

		#endregion

		#region Panel Callbacks

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		/// <param name="panel"></param>
		private void Subscribe(TswFt5Button panel)
		{
			if (panel == null)
				return;

			panel.ExtenderVoipReservedSigs.DeviceExtenderSigChange += ExtenderVoipReservedSigsOnDeviceExtenderSigChange;
		}

		/// <summary>
		/// Unsubscribe from the panel events.
		/// </summary>
		/// <param name="panel"></param>
		private void Unsubscribe(TswFt5Button panel)
		{
			if (panel == null)
				return;

			panel.ExtenderVoipReservedSigs.DeviceExtenderSigChange -= ExtenderVoipReservedSigsOnDeviceExtenderSigChange;
		}

		/// <summary>
		/// Called when a sig on the VOIP extender changes.
		/// </summary>
		/// <param name="extender"></param>
		/// <param name="args"></param>
		private void ExtenderVoipReservedSigsOnDeviceExtenderSigChange(DeviceExtender extender, SigEventArgs args)
		{
			
		}

		#endregion
	}
}
