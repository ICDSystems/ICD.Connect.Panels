#if SIMPLSHARP
#endif
using Crestron.SimplSharpPro;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Tsw1060
{
#if SIMPLSHARP
	public sealed class Tsw1060Adapter : AbstractTswFt5ButtonAdapter<Crestron.SimplSharpPro.UI.Tsw1060, Tsw1060AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw1060 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw1060(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1060Adapter : AbstractTriListAdapter<Tsw1060AdapterSettings>
    {
    }
#endif
}