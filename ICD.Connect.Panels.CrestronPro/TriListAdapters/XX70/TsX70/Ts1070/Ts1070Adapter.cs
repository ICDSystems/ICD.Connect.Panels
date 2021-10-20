#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TsX70.Ts1070
{
#if !NETSTANDARD
	public sealed class Ts1070Adapter : AbstractTsX70BaseAdapter<global::Crestron.SimplSharpPro.UI.Ts1070, Ts1070AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Ts1070 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Ts1070(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts1070Adapter : AbstractTsX70BaseAdapter<Ts1070AdapterSettings>
	{
	}
#endif
}