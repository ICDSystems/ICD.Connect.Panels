#if SIMPLSHARP
#endif
using Crestron.SimplSharpPro;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw560
{
#if SIMPLSHARP
	public sealed class Tsw560Adapter : AbstractTswX60BaseClassAdapter<Crestron.SimplSharpPro.UI.Tsw560, Tsw560AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw560 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw560(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw760Adapter : AbstractTriListAdapter<Tsw760AdapterSettings>
    {
    }
#endif
}
