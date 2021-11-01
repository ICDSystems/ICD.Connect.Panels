﻿using System;
using System.Collections.Generic;
using ICD.Connect.Devices;
using ICD.Connect.Panels.EventArguments;

namespace ICD.Connect.Panels.HardButtons
{
	public enum eHardButton
	{
		Numpad0,
		Numpad1,
		Numpad2,
		Numpad3,
		Numpad4,
		Numpad5,
		Numpad6,
		Numpad7,
		Numpad8,
		Numpad9,
		NumpadMiscLeft,
		NumpadMiscRight,
		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,
		DPadCenter,
		Favorite,
		Home,
		Guide,
		Info,
		Menu,
		Exit,
		Last,
		Dvr,
		Play,
		Pause,
		Stop,
		Forward,
		Reverse,
		Previous,
		Next,
		SkipForward,
		SkipReverse,
		Record,
		Red,
		Green,
		Yellow,
		Blue,
		ChannelUp,
		ChannelDown,
		PageUp,
		PageDown,
		VolumeUp,
		VolumeDown,
		VolumeMuteToggle,
		VolumeMuteOn,
		VolumeMuteOff,
		Power,
		Backlight,
		Custom1,
		Custom2,
		Custom3,
		Custom4,
		Custom5,
		Custom6,
		Custom7,
		Custom8,
		Custom9
	}

	public enum eButtonAction
	{
		Press,
		Release,
		Tap,
		DoubleTap,
		Hold,
		HoldRelease
	}
	
	public interface IHardButtonDevice : IDevice
	{
		event EventHandler<ButtonActionEventArgs> OnButtonAction;
	}
}