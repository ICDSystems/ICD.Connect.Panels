#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52.Tsw1052
{
#if SIMPLSHARP
	public sealed class Tsw1052Adapter :
		AbstractTswX52ButtonVoiceControlAdapter<Crestron.SimplSharpPro.UI.Tsw1052, Tsw1052AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw1052 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw1052(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1052Adapter : AbstractTriListAdapter<Tsw1052AdapterSettings>
    {
    }
#endif
}
