#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52.Tsw552
{
#if SIMPLSHARP
	public sealed class Tsw552Adapter :
		AbstractTswX52ButtonVoiceControlAdapter<global::Crestron.SimplSharpPro.UI.Tsw552, Tsw552AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw552 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw552(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw552Adapter : AbstractTriListAdapter<Tsw552AdapterSettings>
    {
    }
#endif
}
