using ICD.Connect.Misc.Ethernet;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons
{
	public interface ITswFt5ButtonAdapterSettings : ITriListAdapterSettings, ICrestronEthernetDeviceAdapterSettings
	{
		bool EnableVoip { get; set; }
	}
}