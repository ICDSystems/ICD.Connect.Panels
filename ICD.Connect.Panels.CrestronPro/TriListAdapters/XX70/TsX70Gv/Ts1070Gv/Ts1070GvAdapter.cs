#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TsX70Gv.Ts1070Gv
{
#if !NETSTANDARD
	public sealed class Ts1070GvAdapter : AbstractTsX70GvBaseAdapter<global::Crestron.SimplSharpPro.UI.Ts1070GV, Ts1070GvAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Ts1070GV InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Ts1070GV(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts1070GvAdapter : AbstractTsX70GvBaseAdapter<Ts1070GvAdapterSettings>
	{
	}
#endif
}