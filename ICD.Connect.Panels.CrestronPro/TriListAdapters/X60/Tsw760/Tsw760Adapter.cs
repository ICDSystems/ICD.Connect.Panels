﻿#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60.Tsw760
{
#if SIMPLSHARP
	public sealed class Tsw760Adapter : AbstractTswX60BaseClassAdapter<Crestron.SimplSharpPro.UI.Tsw760, Tsw760AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Tsw760 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Tsw760(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw760Adapter : AbstractTriListAdapter<Tsw760AdapterSettings>
    {
    }
#endif
}
