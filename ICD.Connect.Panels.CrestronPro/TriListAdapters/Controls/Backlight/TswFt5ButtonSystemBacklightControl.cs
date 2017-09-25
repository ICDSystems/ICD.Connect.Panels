using Crestron.SimplSharpPro.DeviceSupport;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight
{
	public sealed class TswFt5ButtonSystemBacklightControl :
		AbstractFt5ButtonBacklightControl<ITswFt5ButtonSystemAdapter, TswFt5ButtonSystem, TswFtSystemReservedSigs>
	{
		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		protected override TswFtSystemReservedSigs Sigs
		{
			get { return Panel == null ? null : Panel.ExtenderSystemReservedSigs; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswFt5ButtonSystemBacklightControl(ITswFt5ButtonSystemAdapter parent, int id)
			: base(parent, id)
		{
		}
	}
}
