using Crestron.SimplSharpPro.UI;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X52;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60
{
	public abstract class AbstractTswX60BaseClassAdapter<TPanel, TSettings> :
		AbstractTswX52ButtonVoiceControlAdapter<TPanel, TSettings>, ITswX60BaseClassAdapter
		where TPanel : TswX60BaseClass
		where TSettings : ITswX60BaseClassAdapterSettings, new()
	{
	}

	public abstract class AbstractTswX60BaseClassAdapterSettings : AbstractTswX52ButtonVoiceControlAdapterSettings,
	                                                               ITswX60BaseClassAdapterSettings
	{
	}

	public interface ITswX60BaseClassAdapterSettings : ITswX52ButtonVoiceControlAdapterSettings
	{
	}

	public interface ITswX60BaseClassAdapter : ITswX52ButtonVoiceControlAdapter
	{
	}
}
