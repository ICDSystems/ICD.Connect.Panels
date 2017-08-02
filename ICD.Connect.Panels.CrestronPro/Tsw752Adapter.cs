using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Panels.CrestronPro.Settings;
using ICD.Connect.Panels.CrestronPro.TriListAdapters;

namespace ICD.Connect.Panels.CrestronPro
{
	public sealed class Tsw752Adapter : AbstractTriListAdapter<Tsw752, Tsw752AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Tsw752 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Tsw752(ipid, controlSystem);
		}
	}
}