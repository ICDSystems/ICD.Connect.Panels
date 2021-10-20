#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70.Tsw1070
{
#if !NETSTANDARD
	public sealed class Tsw1070Adapter : AbstractTswX70BaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw1070, Tsw1070AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw1070 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw1070(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw1070Adapter : AbstractTswX70BaseAdapter<Tsw1070AdapterSettings>
    {
    }
#endif
}