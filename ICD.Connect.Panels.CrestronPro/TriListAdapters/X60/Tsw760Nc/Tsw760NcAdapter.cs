#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw760Nc
{
#if SIMPLSHARP
	public sealed class Tsw760NcAdapter : AbstractTswX60BaseClassAdapter<Crestron.SimplSharpPro.UI.Tsw760Nc, Tsw760NcAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw760Nc InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw760Nc(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw760NcAdapter : AbstractTriListAdapter<Tsw760NcAdapterSettings>
    {
    }
#endif
}
