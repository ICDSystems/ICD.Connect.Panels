﻿#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52.Tsw1052
{
#if !NETSTANDARD
	public sealed class Tsw1052Adapter :
		AbstractTswX52ButtonVoiceControlAdapter<global::Crestron.SimplSharpPro.UI.Tsw1052, Tsw1052AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw1052 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw1052(ipid, controlSystem);
		}
	}
#else
    public sealed class Tsw1052Adapter : AbstractTswX52ButtonVoiceControlAdapter<Tsw1052AdapterSettings>
    {
    }
#endif
}
