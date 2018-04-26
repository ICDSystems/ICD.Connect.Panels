using ICD.Connect.Settings.Attributes;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public sealed class Dge2Adapter : AbstractDge2BaseAdapter<Dge2, Dge2AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Dge2 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Dge2(ipid, controlSystem);
		}
	}
#else
	public sealed class Dge2Adapter : AbstractDge2BaseAdapter<Dge2AdapterSettings>
	{
	}
#endif

	[KrangSettings("Dge2", typeof(Dge2Adapter))]
	public sealed class Dge2AdapterSettings : AbstractDge2BaseAdapterSettings
	{
	}
}
