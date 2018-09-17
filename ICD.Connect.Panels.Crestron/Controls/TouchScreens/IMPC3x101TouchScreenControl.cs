using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Misc.Keypads;

namespace ICD.Connect.Panels.Crestron.Controls.TouchScreens
{
	public interface IMPC3x101TouchScreenControl : IMPC3BasicTouchScreenControl
	{
		/// <summary>
		/// Raised when the proximity sensor detection state changes.
		/// </summary>
		event EventHandler<BoolEventArgs> OnProximityDetectedStateChange;

		#region Properties

		/// <summary>
		/// Get the Button object corresponding to the VolumeUp button for this device.
		/// </summary>
		eButtonState VolumeUp { get; }

		/// <summary>
		/// Get the Button object corresponding to the VolumeDown button for this device.
		/// </summary>
		eButtonState VolumeDown { get; }

		/// <summary>
		/// When true, indicates front panel button press beeping is enabled on this device.
		/// When false, indicates front panel button press beeping is disabled on this device.
		/// </summary>
		bool ButtonPressBeepingEnabled { get; }

		/// <summary>
		/// When true, indicates the target is detected within range.
		/// </summary>
		bool ProximityDetected { get; }

		/// <summary>
		/// When true, indicates the proximity wakeup is enabled on this device.
		/// </summary>
		bool ProximityWakeupEnabled { get; }

		/// <summary>
		/// Property that returns true when the volume down button is enabled on this device, false otherwise.
		/// </summary>
		bool VolumeDownButtonEnabled { get; }

		/// <summary>
		/// Property that returns true when the volume up button is enabled on this device, false otherwise.
		/// </summary>
		bool VolumeUpButtonEnabled { get; }

		/// <summary>
		/// Reports the current proximity range detected in centimeter.
		/// Valid values range from 5 to 200. Invalid value: -1.
		/// </summary>
		ushort ProximityRange { get; }

		/// <summary>
		/// Indicates Proximity Sensor Detecting Threshold in Centimeter Unit.
		/// Valid values range from 2 (cm) to 200 (cm).
		/// </summary>
		ushort ProximityThreshold { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Enable the volume down button on this device.
		/// </summary>
		void SetVolumeDownButtonEnabled(bool enable);

		/// <summary>
		/// Enable the volume up button on this device.
		/// </summary>
		void SetVolumeUpButtonEnabled(bool enabled);

		/// <summary>
		/// Method to enable beeping for front panel button presses.
		/// </summary>
		void SetButtonPressBeepingEnabled(bool enable);

		/// <summary>
		/// Property to allow proximity detection to wake unit - transition to Active.
		/// Set to true, to enable the proximity wakeup on this device.
		/// </summary>
		void SetProximityWakeupEnabled(bool enable);

		/// <summary>
		/// Specifies Proximity Sensor Detecting Threshold in Centimeter Unit.
		/// Valid values range from 2 (cm) to 200 (cm).
		/// </summary>
		void SetProximityThreshold(ushort centimeters);

		#endregion
	}
}
