#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw560Nc
{
#if SIMPLSHARP
	public sealed class Tsw560NcAdapter :
		AbstractTswX60BaseClassAdapter<global::Crestron.SimplSharpPro.UI.Tsw560Nc, Tsw560NcAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw560Nc InstantiateTriList(byte ipid,
		                                                                         CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw560Nc(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw560NcAdapter : AbstractTriListAdapter<Tsw560NcAdapterSettings>
    {
    }
#endif
}
