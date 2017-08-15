#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif
using ICD.Connect.Panels.CrestronPro.Settings;
using ICD.Connect.Panels.CrestronPro.TriListAdapters;

namespace ICD.Connect.Panels.CrestronPro
{
#if SIMPLSHARP
    public sealed class Tsw1060Adapter : AbstractTriListAdapter<Tsw1060, Tsw1060AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Tsw1060 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Tsw1060(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1060Adapter : AbstractTriListAdapter<Tsw1060AdapterSettings>
    {
    }
#endif
}