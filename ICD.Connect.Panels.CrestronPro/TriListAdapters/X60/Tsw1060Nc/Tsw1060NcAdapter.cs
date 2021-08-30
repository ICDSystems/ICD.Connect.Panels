#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw1060Nc
{
#if !NETSTANDARD
	public sealed class Tsw1060NcAdapter :
		AbstractTswX60BaseClassAdapter<global::Crestron.SimplSharpPro.UI.Tsw1060Nc, Tsw1060NcAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw1060Nc InstantiateTriList(byte ipid,
		                                                                          CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw1060Nc(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1060NcAdapter : AbstractTswX60BaseClassAdapter<Tsw1060NcAdapterSettings>
    {
    }
#endif
}
