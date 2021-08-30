using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons;
#if !NETSTANDARD
using Crestron.SimplSharpPro.DeviceSupport;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight
{
	public sealed class TswFt5ButtonBacklightControl :
		AbstractFt5ButtonBacklightControl<ITswFt5ButtonAdapter, TswFt5Button, TsxSystemReservedSigs>
	{
		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		protected override TsxSystemReservedSigs Sigs
		{
			get { return Panel == null ? null : Panel.ExtenderSystemReservedSigs; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswFt5ButtonBacklightControl(ITswFt5ButtonAdapter parent, int id)
			: base(parent, id)
		{
		}
	}
}

#endif
