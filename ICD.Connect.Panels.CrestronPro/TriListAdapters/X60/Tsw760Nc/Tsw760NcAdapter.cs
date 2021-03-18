#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw760Nc
{
#if SIMPLSHARP
	public sealed class Tsw760NcAdapter :
		AbstractTswX60BaseClassAdapter<global::Crestron.SimplSharpPro.UI.Tsw760Nc, Tsw760NcAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw760Nc InstantiateTriList(byte ipid,
		                                                                         CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw760Nc(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw760NcAdapter : AbstractTswX60BaseClassAdapter<Tsw760NcAdapterSettings>
    {
    }
#endif
}
