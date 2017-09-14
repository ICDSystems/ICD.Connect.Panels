#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw560P
{
#if SIMPLSHARP
	public sealed class Tsw560PAdapter : AbstractTswX60BaseClassAdapter<Crestron.SimplSharpPro.UI.Tsw560P, Tsw560PAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw560P InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw560P(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw560PAdapter : AbstractTriListAdapter<Tsw560PAdapterSettings>
    {
    }
#endif
}
