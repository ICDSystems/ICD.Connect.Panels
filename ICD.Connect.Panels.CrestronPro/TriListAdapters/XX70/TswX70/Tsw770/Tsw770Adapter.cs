#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70.Tsw770
{
#if !NETSTANDARD
	public sealed class Tsw770Adapter : AbstractTswX70BaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw770, Tsw770AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw770 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw770(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw770Adapter : AbstractTswX70BaseAdapter<Tsw770AdapterSettings>
	{
	}
#endif
}