#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public sealed class DmDge200CAdapter : AbstractDge100Adapter<DmDge200C, DmDge200CAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override DmDge200C InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new DmDge200C(ipid, controlSystem);
		}
	}
#else
	public sealed class DmDge200CAdapter : AbstractDge100Adapter<DmDge200CAdapterSettings>
	{
	}
#endif
}
