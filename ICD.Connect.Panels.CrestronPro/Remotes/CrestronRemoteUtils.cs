using System;
using System.Collections.Generic;
using ICD.Connect.Protocol.HardButtons;
#if !NETSTANDARD
using Crestron.SimplSharpPro.DeviceSupport;
#endif

namespace ICD.Connect.Panels.CrestronPro.Remotes
{
	public static class CrestronRemoteUtils
	{

		/// <summary>
		/// Mapping of standard crestron hard button number to eHardButton
		/// Suitable for HR-100, HR-150, TSR-310
		/// Not suitable for TSR-302 or TSR-310 remotes
		/// Not all remotes have all buttons
		/// </summary>
		private static readonly Dictionary<int, eHardButton> s_HrButtonMap = new Dictionary<int, eHardButton>
		{
			{1, eHardButton.Power},
			{2, eHardButton.Backlight},
			{3, eHardButton.Menu},
			{4, eHardButton.Guide},
			{5, eHardButton.Info},
			{6, eHardButton.VolumeUp},
			{7, eHardButton.VolumeDown},
			{8, eHardButton.DPadUp},
			{9, eHardButton.DPadDown},
			{10, eHardButton.DPadLeft},
			{11, eHardButton.DPadRight},
			{12, eHardButton.DPadCenter},
			{13, eHardButton.ChannelUp},
			{14, eHardButton.ChannelDown},
			{15, eHardButton.VolumeMuteToggle},
			{16, eHardButton.Exit},
			{17, eHardButton.Last},
			{18, eHardButton.Play},
			{19, eHardButton.Pause},
			{20, eHardButton.Reverse},
			{21, eHardButton.Forward},
			{22, eHardButton.Previous},
			{23, eHardButton.Next},
			{24, eHardButton.Stop},
			{25, eHardButton.Record},
			{26, eHardButton.Dvr},
			{27, eHardButton.Numpad1},
			{28, eHardButton.Numpad2},
			{29, eHardButton.Numpad3},
			{30, eHardButton.Numpad4},
			{31, eHardButton.Numpad5},
			{32, eHardButton.Numpad6},
			{33, eHardButton.Numpad7},
			{34, eHardButton.Numpad8},
			{35, eHardButton.Numpad9},
			{36, eHardButton.Numpad0},
			{37, eHardButton.NumpadMiscLeft},
			{38, eHardButton.NumpadMiscRight},
			{39, eHardButton.Red},
			{40, eHardButton.Green},
			{41, eHardButton.Yellow},
			{42, eHardButton.Blue},
			// Unsure why 43 was skipped
			{44, eHardButton.Custom1},
			{45, eHardButton.Custom2},
			{46, eHardButton.Custom3},
			{47, eHardButton.Custom4},
			{48, eHardButton.Custom5},
			{49, eHardButton.Custom6},
			{50, eHardButton.Custom7},
			{51, eHardButton.Custom8},
			{52, eHardButton.Custom9},
			// Unsure why 53 and 54 were skipped
			{55, eHardButton.Favorite},
			{56, eHardButton.Home}
		};

		public static bool TryGetHrHardButtonFromButtonNumber(int buttonNumber, out eHardButton hardButton)
		{
			return s_HrButtonMap.TryGetValue(buttonNumber, out hardButton);
		}

#if !NETSTANDARD

		public static eButtonAction ButtonStateToButtonAction(eButtonState state)
		{
			switch (state)
			{
				case eButtonState.Tapped:
					return eButtonAction.Tap;
				case eButtonState.DoubleTapped:
					return eButtonAction.DoubleTap;
				case eButtonState.Held:
					return eButtonAction.Hold;
				case eButtonState.HeldReleased:
					return eButtonAction.HoldRelease;
				case eButtonState.Pressed:
					return eButtonAction.Press;
				case eButtonState.Released:
					return eButtonAction.Release;
				default:
					throw new ArgumentOutOfRangeException("state");
			}
		}

#endif

	}
}