using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X50;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52
{
	public abstract class AbstractTswX52ButtonVoiceControlAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings>, ITswX52ButtonVoiceControlAdapter
		where TPanel : Tswx52ButtonVoiceControl
		where TSettings : ITswX52ButtonVoiceControlAdapterSettings, new()
	{
	}

	public abstract class AbstractTswX52ButtonVoiceControlAdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings,
	                                                                        ITswX52ButtonVoiceControlAdapterSettings
	{

	}

	public interface ITswX52ButtonVoiceControlAdapterSettings : ITswFt5ButtonSystemAdapterSettings
	{
	}

	public interface ITswX52ButtonVoiceControlAdapter : ITswFt5ButtonSystemAdapter
	{
	}
}
