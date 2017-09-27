using Crestron.SimplSharpPro;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.CrestronApp
{
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
}
