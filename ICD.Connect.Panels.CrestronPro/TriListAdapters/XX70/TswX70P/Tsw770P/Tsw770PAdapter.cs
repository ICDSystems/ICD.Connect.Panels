#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70P.Tsw770P
{
#if !NETSTANDARD
	public sealed class Tsw770PAdapter : AbstractTswX70PBaseAdapter<global::Crestron.SimplSharpPro.UI.Tsw770P, Tsw770PAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Tsw770P InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Tsw770P(ipid, controlSystem);
		}
	}
#else
	public sealed class Tsw770PAdapter : AbstractTswX70PBaseAdapter<Tsw770PAdapterSettings>
	{
	}
#endif
}