using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Panels.CrestronPro.Settings;
using ICD.Connect.Panels.CrestronPro.TriListAdapters;

namespace ICD.Connect.Panels.CrestronPro
{
	public sealed class Tsw1052Adapter : AbstractTriListAdapter<Tsw1052, Tsw1052AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Tsw1052 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Tsw1052(ipid, controlSystem);
		}
	}
}
