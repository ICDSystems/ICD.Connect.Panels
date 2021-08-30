#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw760
{
#if !NETSTANDARD
	public sealed class Tsw760Adapter :
		AbstractTswX60BaseClassAdapter<global::Crestron.SimplSharpPro.UI.Tsw760, Tsw760AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw760 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw760(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw760Adapter : AbstractTswX60BaseClassAdapter<Tsw760AdapterSettings>
    {
    }
#endif
}
