#if SIMPLSHARP
using Crestron.SimplSharpPro.UI;
#endif
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X52;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X60
{
#if SIMPLSHARP
	public abstract class AbstractTswX60BaseClassAdapter<TPanel, TSettings> :
		AbstractTswX52ButtonVoiceControlAdapter<TPanel, TSettings>, ITswX60BaseClassAdapter
		where TPanel : TswX60BaseClass
#else
	public abstract class AbstractTswX60BaseClassAdapter<TSettings> :
		AbstractTswX52ButtonVoiceControlAdapter<TSettings>, ITswX60BaseClassAdapter
#endif
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
