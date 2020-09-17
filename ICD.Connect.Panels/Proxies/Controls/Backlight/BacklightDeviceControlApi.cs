namespace ICD.Connect.Panels.Proxies.Controls.Backlight
{
	public static class BacklightDeviceControlApi
	{
		public const string EVENT_BACKLIGHT_STATE = "OnBacklightStateChanged";
		public const string PROPERTY_BACKLIGHT_STATE = "BacklightState";

		public const string METHOD_BACKLIGHT_ON = "BacklightOn";
		public const string METHOD_BACKLIGHT_OFF = "BacklightOff";

		public const string HELP_EVENT_BACKLIGHT_STATE = "Raised when the backlight state changes.";
		public const string HELP_PROPERTY_BACKLIGHT_STATE = "The backlight state of the device.";

		public const string HELP_METHOD_BACKLIGHT_ON = "Turns on the backlight.";
		public const string HELP_METHOD_BACKLIGHT_OFF = "Turns off the backlight.";
	}
}
