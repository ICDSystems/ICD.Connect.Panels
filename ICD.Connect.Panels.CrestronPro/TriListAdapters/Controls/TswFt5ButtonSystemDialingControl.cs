using System;
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
	}
}
