#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TsX70.Ts770
{
#if !NETSTANDARD
	public sealed class Ts770Adapter : AbstractTsX70BaseAdapter<global::Crestron.SimplSharpPro.UI.Ts770, Ts770AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Ts770 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Ts770(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts770Adapter : AbstractTsX70BaseAdapter<Ts770AdapterSettings>
	{
	}
#endif
}