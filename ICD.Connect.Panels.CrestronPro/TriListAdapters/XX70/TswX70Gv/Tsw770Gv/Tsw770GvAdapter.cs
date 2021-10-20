#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70Gv.Tsw770Gv
{
#if !NETSTANDARD
	public sealed class Tsw770GvAdapter : AbstractTswX70GvBaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw770GV, Tsw770GvAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw770GV InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw770GV(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw770GvAdapter : AbstractTswX70GvBaseAdapter<Tsw770GvAdapterSettings>
	{
	}
#endif
}