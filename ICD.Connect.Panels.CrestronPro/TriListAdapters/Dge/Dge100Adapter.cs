#if SIMPLSHARP
using ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public sealed class Dge100Adapter : AbstractDgeX00Adapter<Dge100, Dge100AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Dge100 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Dge100(ipid, controlSystem);
		}

		public Dge100Adapter()
		{
			Controls.Add(new Dge100StreamSwitcherControl(this, 0));
		}
	}
#else
	public sealed class Dge100Adapter : AbstractDgeX00Adapter<Dge100AdapterSettings>
	{
	}
#endif
}
