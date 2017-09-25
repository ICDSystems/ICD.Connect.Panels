#if SIMPLSHARP
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X60;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight
{
	public sealed class TswX60BaseBacklightControl :
		AbstractFt5ButtonBacklightControl<ITswX60BaseClassAdapter, TswX60BaseClass, Tpmc9lSystemReservedSigs>
	{
		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		protected override Tpmc9lSystemReservedSigs Sigs
		{
			get { return Panel == null ? null : Panel.ExtenderSystemReservedSigs; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswX60BaseBacklightControl(ITswX60BaseClassAdapter parent, int id)
			: base(parent, id)
		{
		}
	}
}
#endif
