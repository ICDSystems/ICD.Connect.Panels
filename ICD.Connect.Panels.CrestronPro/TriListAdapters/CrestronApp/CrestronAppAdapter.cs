#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.CrestronApp
{
#if SIMPLSHARP
	public sealed class CrestronAppAdapter : AbstractTriListAdapter<Crestron.SimplSharpPro.UI.CrestronApp, CrestronAppAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.CrestronApp InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.CrestronApp(ipid, controlSystem);
		}
	}
#else
	public sealed class CrestronAppAdapter : AbstractTriListAdapter<CrestronAppAdapterSettings>
	{
	}
#endif
}
