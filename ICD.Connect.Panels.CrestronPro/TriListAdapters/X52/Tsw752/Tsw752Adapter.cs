#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52.Tsw752
{
#if SIMPLSHARP
	public sealed class Tsw752Adapter :
		AbstractTswX52ButtonVoiceControlAdapter<Crestron.SimplSharpPro.UI.Tsw752, Tsw752AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw752 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw752(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw752Adapter : AbstractTriListAdapter<Tsw752AdapterSettings>
    {
    }
#endif
}
