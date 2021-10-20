#if !NETSTANDARD
using System;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Misc.CrestronPro.Extensions;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip
{
	public sealed class TswXX70BaseConferenceControl : AbstractTswXX70BaseConferenceControl<ITswXX70BaseAdapter, TswXX70Base, TsxVoipReservedSigs>
	{
		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		protected override TsxVoipReservedSigs Sigs { get { return Panel == null ? null : Panel.ExtenderVoipReservedSigs; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswXX70BaseConferenceControl(ITswXX70BaseAdapter parent, int id)
			: base(parent, id)
		{
			SupportedConferenceControlFeatures |= eConferenceControlFeatures.PrivacyMute;
		}

		/// <summary>
		/// Sets the auto-answer enabled state.
		/// </summary>
		/// <param name="enabled"></param>
		public override void SetAutoAnswer(bool enabled)
		{
			throw new NotSupportedException();
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

			PrivacyMuted = Sigs.MutedFeedback.GetBoolValueOrDefault();
		}
	}
}
#endif
