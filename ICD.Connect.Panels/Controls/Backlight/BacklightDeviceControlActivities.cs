using System;
using ICD.Common.Logging.Activities;
using ICD.Common.Utils.Services.Logging;

namespace ICD.Connect.Panels.Controls.Backlight
{
	public static class BacklightDeviceControlActivities
	{
		public static Activity GetBacklightActivity(eBacklightState backlightState)
		{
			switch (backlightState)
			{
				case eBacklightState.Unknown:
					return new Activity(Activity.ePriority.Lowest, "Backlight", "Unknown Backlight State", eSeverity.Warning);
				case eBacklightState.BacklightOff:
					return new Activity(Activity.ePriority.Medium, "Backlight", "Backlight Off", eSeverity.Informational);
				case eBacklightState.BacklightOn:
					return new Activity(Activity.ePriority.Lowest, "Backlight", "Backlight On", eSeverity.Informational);
				default:
					throw new ArgumentOutOfRangeException("backlightState");
			}
		}
	}
}
