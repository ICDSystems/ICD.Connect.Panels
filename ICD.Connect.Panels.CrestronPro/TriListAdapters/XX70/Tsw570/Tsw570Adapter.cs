#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.Tsw570
{
#if !NETSTANDARD
	public sealed class Tsw570Adapter : AbstractTswXX70BaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw570, Tsw570AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw570 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw570(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw570Adapter : AbstractTswXX70BaseAdapter<Tsw570AdapterSettings>
	{
	}
#endif
}