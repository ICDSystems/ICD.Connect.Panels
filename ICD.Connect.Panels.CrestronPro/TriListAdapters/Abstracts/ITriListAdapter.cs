using ICD.Connect.Panels.Devices;
#if !NETSTANDARD
using Crestron.SimplSharpPro.DeviceSupport;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts
{
#if !NETSTANDARD
	public delegate void PanelChangeCallback(ITriListAdapter sender, BasicTriListWithSmartObject panel);
#endif

	public interface ITriListAdapter : IPanelDevice
	{
#if !NETSTANDARD
		/// <summary>
		/// Raised when the internal wrapped panel changes.
		/// </summary>
		event PanelChangeCallback OnPanelChanged;

		/// <summary>
		/// Gets the internal wrapped panel instance.
		/// </summary>
		BasicTriListWithSmartObject Panel { get; }
#endif
	}
}
