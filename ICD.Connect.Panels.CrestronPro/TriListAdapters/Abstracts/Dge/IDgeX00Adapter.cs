#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Misc.CrestronPro.Devices;
using ICD.Connect.Panels.Crestron.Devices.Dge;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge
{
#if !NETSTANDARD
	public interface IDgeX00Adapter<TPanel> : IDgeX00Adapter, ITriListAdapter, IPortParent
		where TPanel : Dge100
	{
		TPanel Dge { get; }
	}
#endif
}