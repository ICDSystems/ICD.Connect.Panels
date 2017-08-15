#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif
using ICD.Connect.Panels.CrestronPro.Settings;
using ICD.Connect.Panels.CrestronPro.TriListAdapters;

namespace ICD.Connect.Panels.CrestronPro
{
    /// <summary>
    /// Tsw750Adapter wraps a Tsw750 for use with the UIPro library.
    /// </summary>
#if SIMPLSHARP
    public sealed class Tsw750Adapter : AbstractTriListAdapter<Tsw750, Tsw750AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Tsw750 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Tsw750(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw750Adapter : AbstractTriListAdapter<Tsw750AdapterSettings>
    {
    }
#endif
}
