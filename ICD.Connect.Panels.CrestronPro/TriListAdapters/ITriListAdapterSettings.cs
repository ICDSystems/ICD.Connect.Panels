using ICD.Connect.Panels.Devices;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public interface ITriListAdapterSettings : IPanelDeviceSettings
	{
		byte? Ipid { get; set; }
	}
}
