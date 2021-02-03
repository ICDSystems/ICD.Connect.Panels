using ICD.Connect.Panels.Devices;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts
{
	public interface ITriListAdapterSettings : IPanelDeviceSettings
	{
		byte? Ipid { get; set; }
	}
}
