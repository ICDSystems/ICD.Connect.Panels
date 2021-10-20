#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70Gv.Tsw1070Gv
{
#if !NETSTANDARD
	public sealed class Tsw1070GvAdapter : AbstractTswX70GvBaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw1070GV, Tsw1070GvAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw1070GV InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw1070GV(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw1070GvAdapter : AbstractTswX70GvBaseAdapter<Tsw1070GvAdapterSettings>
	{
	}
#endif
}