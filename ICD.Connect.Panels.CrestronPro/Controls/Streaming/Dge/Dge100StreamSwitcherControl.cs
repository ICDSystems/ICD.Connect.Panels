#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge;

namespace ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge
{
	public sealed class Dge100StreamSwitcherControl : AbstractDgeStreamSwitcherControl<Dge100Adapter, Dge100>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public Dge100StreamSwitcherControl(Dge100Adapter parent, int id) 
			: base(parent, id)
		{
		}
	}
}
#endif