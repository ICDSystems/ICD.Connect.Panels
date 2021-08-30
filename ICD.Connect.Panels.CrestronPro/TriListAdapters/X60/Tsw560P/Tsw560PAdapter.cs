#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw560P
{
#if !NETSTANDARD
	public sealed class Tsw560PAdapter :
		AbstractTswX60BaseClassAdapter<global::Crestron.SimplSharpPro.UI.Tsw560P, Tsw560PAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw560P InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw560P(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw560PAdapter : AbstractTswX60BaseClassAdapter<Tsw560PAdapterSettings>
    {
    }
#endif
}
