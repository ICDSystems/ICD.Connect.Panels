#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TswX70Gv
{
#if !NETSTANDARD
	public abstract class AbstractTswX70GvBaseAdapter<TPanel, TSettings> : AbstractTswXX70BaseAdapter<TPanel, TSettings>, ITswX70GvBaseAdapter
		where TPanel : TswX70GVBase
#else
	public abstract class AbstractTswX70GvBaseAdapter<TSettings> : AbstractTswXX70BaseAdapter<TSettings>, ITswX70GvBaseAdapter
#endif
		where TSettings : ITswX70GvBaseAdapterSettings, new()
	{
	}

	public abstract class AbstractTswX70GvBaseAdapterSettings : AbstractTswXX70BaseAdapterSettings,
	                                                            ITswX70GvBaseAdapterSettings
	{
	}

	public interface ITswX70GvBaseAdapter : ITswXX70BaseAdapter
	{
	}

	public interface ITswX70GvBaseAdapterSettings : ITswXX70BaseAdapterSettings
	{
	}
}