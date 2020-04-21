using ICD.Connect.Devices;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Panels.Controls
{
	public abstract class AbstractHardButtonBacklightControl<TParent> : AbstractDeviceControl<TParent>,
	                                                                    IHardButtonBacklightControl
		where TParent : IDevice
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractHardButtonBacklightControl(TParent parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Sets the enabled state of the backlight for the button at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="enabled"></param>
		public abstract void SetBacklightEnabled(int address, bool enabled);
	}
}
