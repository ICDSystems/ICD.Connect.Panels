using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge;
using ICD.Connect.Settings.Attributes;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public sealed class Dge1Adapter : AbstractDge1BaseAdapter<Dge1, Dge1AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Dge1 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Dge1(ipid, controlSystem);
		}
	}
#else
	public sealed class Dge1Adapter : AbstractDge1BaseAdapter<Dge1AdapterSettings>
	{
	}
#endif

	[KrangSettings("Dge1", typeof(Dge1Adapter))]
	public sealed class Dge1AdapterSettings : AbstractDge1BaseAdapterSettings
	{
	}
}
