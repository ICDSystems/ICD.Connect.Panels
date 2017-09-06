using Crestron.SimplSharpPro.DeviceSupport;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50
{
	public abstract class AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonAdapter<TPanel, TSettings>, ITswFt5ButtonSystemAdapter
		where TPanel : TswFt5ButtonSystem
		where TSettings : ITswFt5ButtonSystemAdapterSettings, new()
	{
	}

	public interface ITswFt5ButtonSystemAdapter : ITswFt5ButtonAdapter
	{
	}

	public interface ITswFt5ButtonSystemAdapterSettings : ITswFt5ButtonAdapterSettings
	{
	}

	public abstract class AbstractTswFt5ButtonSystemAdapterSettings : AbstractTswFt5ButtonAdapterSettings,
	                                                                  ITswFt5ButtonSystemAdapterSettings
	{

	}
}
