using Crestron.SimplSharpPro.DeviceSupport;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public abstract class AbstractTswFt5ButtonAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, ITswFt5ButtonAdapter
		where TSettings : ITswFt5ButtonAdapterSettings, new()
		where TPanel : TswFt5Button
	{
	}
}
