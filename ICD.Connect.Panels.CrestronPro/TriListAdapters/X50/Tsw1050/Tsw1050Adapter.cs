using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons;
#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw1050
{
#if !NETSTANDARD
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
    public sealed class Tsw1050Adapter : AbstractTswFt5ButtonSystemAdapter<Tsw1050AdapterSettings>
    {
    }
#endif
}
