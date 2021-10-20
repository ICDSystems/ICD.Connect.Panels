#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.Ts570
{
#if !NETSTANDARD
	public sealed class Ts570Adapter : AbstractTswXX70BaseAdapter<global::Crestron.SimplSharpPro.UI.Ts570, Ts570AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Ts570 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Ts570(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts570Adapter : AbstractTswXX70BaseAdapter<Ts570AdapterSettings>
	{
	}
#endif
}