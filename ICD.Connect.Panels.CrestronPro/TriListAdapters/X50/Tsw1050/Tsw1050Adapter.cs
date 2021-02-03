using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw1050
{
#if SIMPLSHARP
	public sealed class Tsw1050Adapter :
		AbstractTswFt5ButtonSystemAdapter<global::Crestron.SimplSharpPro.UI.Tsw1050, Tsw1050AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw1050 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw1050(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1050Adapter : AbstractTriListAdapter<Tsw1050AdapterSettings>
    {
    }
#endif
}
