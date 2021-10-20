#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70
{
#if !NETSTANDARD
	public abstract class AbstractTswX70BaseAdapter<TPanel, TSettings> : AbstractTswXX70BaseAdapter<TPanel, TSettings>, ITswX70BaseAdapter
		where TPanel : TswX70Base
#else
	public abstract class AbstractTswX70BaseAdapter<TSettings> : AbstractTswXX70BaseAdapter<TSettings>, ITswX70BaseAdapter
#endif
		where TSettings : ITswX70BaseAdapterSettings, new()
	{
	}

	public abstract class AbstractTswX70BaseAdapterSettings : AbstractTswXX70BaseAdapterSettings,
	                                                               ITswX70BaseAdapterSettings
	{
	}

	public interface ITswX70BaseAdapter : ITswXX70BaseAdapter
	{
	}

	public interface ITswX70BaseAdapterSettings : ITswXX70BaseAdapterSettings
	{
	}
}