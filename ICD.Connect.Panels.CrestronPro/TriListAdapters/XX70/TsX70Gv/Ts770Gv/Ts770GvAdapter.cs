#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TsX70Gv.Ts770Gv
{
#if !NETSTANDARD
	public sealed class Ts770GvAdapter : AbstractTsX70GvBaseAdapter<global::Crestron.SimplSharpPro.UI.Ts770GV, Ts770GvAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Ts770GV InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Ts770GV(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts770GvAdapter : AbstractTsX70GvBaseAdapter<Ts770GvAdapterSettings>
	{
	}
#endif
}