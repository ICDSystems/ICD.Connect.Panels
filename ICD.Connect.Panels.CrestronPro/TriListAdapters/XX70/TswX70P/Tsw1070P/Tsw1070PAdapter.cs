#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70P.Tsw1070P
{
#if !NETSTANDARD
	public sealed class Tsw1070PAdapter : AbstractTswX70PBaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw1070P, Tsw1070PAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw1070P InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw1070P(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw1070PAdapter : AbstractTswX70PBaseAdapter<Tsw1070PAdapterSettings>
	{
	}
#endif
}