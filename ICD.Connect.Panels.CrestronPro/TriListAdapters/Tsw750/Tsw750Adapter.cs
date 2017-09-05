﻿#if SIMPLSHARP
#endif
using Crestron.SimplSharpPro;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Tsw750
{
    /// <summary>
    /// Tsw750Adapter wraps a Tsw750 for use with the UIPro library.
    /// </summary>
#if SIMPLSHARP
	public sealed class Tsw750Adapter : AbstractTswFt5ButtonAdapter<Crestron.SimplSharpPro.UI.Tsw750, Tsw750AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw750 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw750(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw750Adapter : AbstractTriListAdapter<Tsw750AdapterSettings>
    {
    }
#endif
}