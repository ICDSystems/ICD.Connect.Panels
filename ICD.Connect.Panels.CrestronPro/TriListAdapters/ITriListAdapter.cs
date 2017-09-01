using Crestron.SimplSharpPro.DeviceSupport;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public delegate void PanelChangeCallback(ITriListAdapter sender, BasicTriListWithSmartObject panel);

	public interface ITriListAdapter : IPanelDevice
	{
		/// <summary>
		/// Raised when the internal wrapped panel changes.
		/// </summary>
		event PanelChangeCallback OnPanelChanged;

		/// <summary>
		/// Gets the internal wrapped panel instance.
		/// </summary>
		BasicTriListWithSmartObject Panel { get; }
	}
}
