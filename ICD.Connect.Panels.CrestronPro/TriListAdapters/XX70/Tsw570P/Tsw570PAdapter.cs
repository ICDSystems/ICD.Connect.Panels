#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.Tsw570P
{
#if !NETSTANDARD
	public sealed class Tsw570PAdapter : AbstractTswXX70BaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw570P, Tsw570PAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw570P InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw570P(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw570PAdapter : AbstractTswXX70BaseAdapter<Tsw570PAdapterSettings>
	{
	}
#endif
}