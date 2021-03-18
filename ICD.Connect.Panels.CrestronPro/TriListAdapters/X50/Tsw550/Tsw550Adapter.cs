using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw550
{
	/// <summary>
	/// Tsw550Adapter wraps a Tsw550 for use with the UIPro library.
	/// </summary>
#if SIMPLSHARP
	public sealed class Tsw550Adapter :
		AbstractTswFt5ButtonSystemAdapter<global::Crestron.SimplSharpPro.UI.Tsw550, Tsw550AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw550 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw550(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw550Adapter : AbstractTswFt5ButtonSystemAdapter<Tsw550AdapterSettings>
    {
    }
#endif
}
