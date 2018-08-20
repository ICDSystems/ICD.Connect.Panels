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
