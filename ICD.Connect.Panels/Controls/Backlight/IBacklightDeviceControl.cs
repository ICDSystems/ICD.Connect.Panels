using System;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.Proxies.Controls.Backlight;

namespace ICD.Connect.Panels.Controls.Backlight
{
	public enum eBacklightState
	{
		Unknown,
		BacklightOff,
		BacklightOn
	}

	[ApiClass(typeof(ProxyBacklightDeviceControl), typeof(IDeviceControl))]
	public interface IBacklightDeviceControl : IDeviceControl
	{
		/// <summary>
		/// Raised when the backlight state changes.
		/// </summary>
		[ApiEvent(BacklightDeviceControlApi.EVENT_BACKLIGHT_STATE, BacklightDeviceControlApi.HELP_EVENT_BACKLIGHT_STATE)]
		event EventHandler<BacklightDeviceControlBacklightStateApiEventArgs> OnBacklightStateChanged;

		/// <summary>
		/// Gets the backlight state of the device.
		/// </summary>
		[ApiProperty(BacklightDeviceControlApi.PROPERTY_BACKLIGHT_STATE, BacklightDeviceControlApi.HELP_PROPERTY_BACKLIGHT_STATE)]
		eBacklightState BacklightState { get; }

		/// <summary>
		/// Turns on the backlight.
		/// </summary>
		[ApiMethod(BacklightDeviceControlApi.METHOD_BACKLIGHT_ON, BacklightDeviceControlApi.HELP_METHOD_BACKLIGHT_ON)]
		void BacklightOn();

		/// <summary>
		/// Turns off the backlight.
		/// </summary>
		[ApiMethod(BacklightDeviceControlApi.METHOD_BACKLIGHT_OFF, BacklightDeviceControlApi.HELP_METHOD_BACKLIGHT_OFF)]
		void BacklightOff();
	}
}
