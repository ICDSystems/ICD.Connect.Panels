using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X50;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls
{
	public sealed class TswFt5ButtonSystemDialingControl :
		AbstractButtonSystemDialingControl<ITswFt5ButtonSystemAdapter, TswFt5ButtonSystem, Tss752VoipReservedSigs>
	{
		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		protected override Tss752VoipReservedSigs Sigs
		{
			get { return Panel == null ? null : Panel.ExtenderVoipReservedSigs; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswFt5ButtonSystemDialingControl(ITswFt5ButtonSystemAdapter parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Sets the auto-answer enabled state.
		/// </summary>
		/// <param name="enabled"></param>
		public override void SetAutoAnswer(bool enabled)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No panel");

			// We will handle auto-answer ourselves
			Sigs.AutoAnswer.BoolValue = enabled;

			AutoAnswer = Sigs.AutoAnswerFeedback.BoolValue;
		}

		/// <summary>
		/// Sets the privacy mute enabled state.
		/// </summary>
		/// <param name="enabled"></param>
		public override void SetPrivacyMute(bool enabled)
		{
			if (Sigs == null)
				throw new InvalidOperationException("No VoIP extender");

			Sigs.Muted.BoolValue = enabled;

			PrivacyMuted = Sigs.MutedFeedback.BoolValue;
		}

		/// <summary>
		/// Populates the sig callback map with handlers for sig changes.
		/// </summary>
		protected override void BuildSigCallbacks(Dictionary<Sig, Action<Sig>> map)
		{
			base.BuildSigCallbacks(map);

			Tss752VoipReservedSigs sigs = Sigs;
			if (sigs == null)
				return;

			map[sigs.AutoAnswerFeedback] = HandleAutoAnswerFeedback;
			map[sigs.MutedFeedback] = HandleMutedFeedback;
		}

		private void HandleAutoAnswerFeedback(Sig sig)
		{
			AutoAnswer = sig.BoolValue;
		}

		private void HandleMutedFeedback(Sig sig)
		{
			PrivacyMuted = sig.BoolValue;
		}
	}
}
