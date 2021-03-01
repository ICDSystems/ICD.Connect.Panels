using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Ts1542;
using ICD.Connect.Settings.Attributes;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Ts1542
{
#if SIMPLSHARP
	public sealed class Ts1542Adapter : AbstractTs1542Adapter<global::Crestron.SimplSharpPro.UI.Ts1542, Ts1542AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Ts1542 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Ts1542(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts1542Adapter : AbstractTs1542Adapter<Ts1542AdapterSettings>
	{
	}
#endif

	[KrangSettings("Ts1542", typeof(Ts1542Adapter))]
	public sealed class Ts1542AdapterSettings : AbstractTs1542AdapterSettings
	{
	}
}
