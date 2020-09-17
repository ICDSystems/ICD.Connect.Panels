using ICD.Connect.API.EventArguments;
using ICD.Connect.Panels.Controls.Backlight;
using ICD.Connect.Panels.Proxies.Controls.Backlight;

namespace ICD.Connect.Panels.EventArguments
{
	public sealed class BacklightDeviceControlBacklightStateApiEventArgs : AbstractGenericApiEventArgs<eBacklightState>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public BacklightDeviceControlBacklightStateApiEventArgs(eBacklightState data)
			: base(BacklightDeviceControlApi.EVENT_BACKLIGHT_STATE, data)
		{
		}
	}
}
