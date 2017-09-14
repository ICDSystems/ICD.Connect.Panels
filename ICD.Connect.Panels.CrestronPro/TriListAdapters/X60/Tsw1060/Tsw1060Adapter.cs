#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw1060
{
#if SIMPLSHARP
	public sealed class Tsw1060Adapter : AbstractTswX60BaseClassAdapter<Crestron.SimplSharpPro.UI.Tsw1060, Tsw1060AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw1060 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw1060(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1060Adapter : AbstractTriListAdapter<Tsw1060AdapterSettings>
    {
    }
#endif
}