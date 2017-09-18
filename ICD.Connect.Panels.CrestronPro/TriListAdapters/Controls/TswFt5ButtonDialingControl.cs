﻿#if SIMPLSHARP
using System;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Common.Services.Logging;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls
{
	public sealed class TswFt5ButtonDialingControl :
	AbstractButtonSystemDialingControl<ITswFt5ButtonAdapter, TswFt5Button, TsxVoipReservedSigs>
	{
		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		protected override TsxVoipReservedSigs Sigs
		{
			get { return Panel == null ? null : Panel.ExtenderVoipReservedSigs; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswFt5ButtonDialingControl(ITswFt5ButtonAdapter parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Sets the auto-answer enabled state.
		/// </summary>
		/// <param name="enabled"></param>
		public override void SetAutoAnswer(bool enabled)
		{
			Logger.AddEntry(eSeverity.Warning, "{0} does not support AutoAnswer", Parent);
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
#endif
