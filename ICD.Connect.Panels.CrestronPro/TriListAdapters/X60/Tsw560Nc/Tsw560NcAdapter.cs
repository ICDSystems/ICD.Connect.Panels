#if SIMPLSHARP
#endif
using Crestron.SimplSharpPro;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw560Nc
{
#if SIMPLSHARP
	public sealed class Tsw560NcAdapter : AbstractTswX60BaseClassAdapter<Crestron.SimplSharpPro.UI.Tsw560Nc, Tsw560NcAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw560Nc InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw560Nc(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw760Adapter : AbstractTriListAdapter<Tsw760AdapterSettings>
    {
    }
#endif
}
