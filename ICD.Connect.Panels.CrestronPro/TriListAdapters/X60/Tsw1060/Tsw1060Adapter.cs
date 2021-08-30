#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw1060
{
#if !NETSTANDARD
	public sealed class Tsw1060Adapter :
		AbstractTswX60BaseClassAdapter<global::Crestron.SimplSharpPro.UI.Tsw1060, Tsw1060AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw1060 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw1060(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1060Adapter : AbstractTswX60BaseClassAdapter<Tsw1060AdapterSettings>
    {
    }
#endif
}
