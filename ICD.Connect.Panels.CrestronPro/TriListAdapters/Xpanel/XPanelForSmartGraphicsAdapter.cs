using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Xpanel
{
#if SIMPLSHARP
	public sealed class XpanelForSmartGraphicsAdapter : AbstractTriListAdapter<XpanelForSmartGraphics, XpanelForSmartGraphicsAdapterSettings>
#else
	public sealed class XpanelForSmartGraphicsAdapter : AbstractTriListAdapter<XpanelForSmartGraphicsAdapterSettings>
#endif
	{
#if SIMPLSHARP
		/// <summary>
		/// Gets the current online status of the panel.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			// Always show XPanels as online (ignore Crestron IsOnline feedback) because
			// they're used for diagnostics and we don't want to pollute telemetry every
			// time someone connects/disconnects
			return Panel != null;
		}

		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override XpanelForSmartGraphics InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new XpanelForSmartGraphics(ipid, controlSystem);
		}
#endif
	}
}
