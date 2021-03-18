using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw750
{
	/// <summary>
	/// Tsw750Adapter wraps a Tsw750 for use with the UIPro library.
	/// </summary>
#if SIMPLSHARP
	public sealed class Tsw750Adapter :
		AbstractTswFt5ButtonSystemAdapter<global::Crestron.SimplSharpPro.UI.Tsw750, Tsw750AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw750 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw750(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw750Adapter : AbstractTswFt5ButtonSystemAdapter<Tsw750AdapterSettings>
    {
    }
#endif
}
