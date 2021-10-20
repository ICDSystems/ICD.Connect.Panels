#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70P
{
#if !NETSTANDARD
	public abstract class AbstractTswX70PBaseAdapter<TPanel, TSettings> : AbstractTswXX70BaseAdapter<TPanel, TSettings>, ITswX70PBaseAdapter
		where TPanel : TswX70PBase
#else
	public abstract class AbstractTswX70PBaseAdapter<TSettings> : AbstractTswXX70BaseAdapter<TSettings>, ITswX70PBaseAdapter
#endif
		where TSettings : ITswX70PBaseAdapterSettings, new()
	{
	}

	public abstract class AbstractTswX70PBaseAdapterSettings : AbstractTswXX70BaseAdapterSettings,
	                                                           ITswX70PBaseAdapterSettings
	{
	}

	public interface ITswX70PBaseAdapter : ITswXX70BaseAdapter
	{
	}

	public interface ITswX70PBaseAdapterSettings : ITswXX70BaseAdapterSettings
	{
	}
}