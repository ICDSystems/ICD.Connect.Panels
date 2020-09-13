using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Panels.Controls.HardButtons
{
	public interface IHardButtonBacklightControl : IDeviceControl
	{
		/// <summary>
		/// Sets the enabled state of the backlight for the button at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="enabled"></param>
		void SetBacklightEnabled(int address, bool enabled);
	}
}
