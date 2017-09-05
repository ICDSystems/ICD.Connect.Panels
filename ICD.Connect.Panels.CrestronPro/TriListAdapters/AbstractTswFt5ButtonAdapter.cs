using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public abstract class AbstractTswFt5ButtonAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, ITswFt5ButtonAdapter
		where TSettings : ITswFt5ButtonAdapterSettings, new()
		where TPanel : TswFt5Button
	{
		protected AbstractTswFt5ButtonAdapter()
		{
			Controls.Add(new TswFt5ButtonDialingControl(this, 1));
		}
	}
}
