using ICD.Connect.Misc.Ethernet;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70
{
	public interface ITswXX70BaseAdapterSettings : ITriListAdapterSettings, ICrestronEthernetDeviceAdapterSettings
	{
		bool EnableVoip { get; set; }
	}
}