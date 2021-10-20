#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight
{
	public sealed class TswXX70BaseBacklightControl : AbstractTswXX70BaseBacklightControl<ITswXX70BaseAdapter, TswXX70Base, Tswxx70SystemReservedSigs>
	{
		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		protected override Tswxx70SystemReservedSigs Sigs { get { return Panel == null ? null : Panel.ExtenderSystemReservedSigs; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswXX70BaseBacklightControl(ITswXX70BaseAdapter parent, int id)
			: base(parent, id)
		{
		}
	}
}
#endif
