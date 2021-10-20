#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TsX70Gv
{
#if !NETSTANDARD
	public abstract class AbstractTsX70GvBaseAdapter<TPanel, TSettings> : AbstractTswXX70BaseAdapter<TPanel, TSettings>, ITsX70GvBaseAdapter
		where TPanel : TsX70GVBase
#else
	public abstract class AbstractTsX70GvBaseAdapter<TSettings> : AbstractTswXX70BaseAdapter<TSettings>, ITsX70GvBaseAdapter
#endif
		where TSettings : ITsX70GvBaseAdapterSettings, new()
	{
	}

	public abstract class AbstractTsX70GvBaseAdapterSettings : AbstractTswXX70BaseAdapterSettings,
	                                                           ITsX70GvBaseAdapterSettings
	{
	}

	public interface ITsX70GvBaseAdapter : ITswXX70BaseAdapter
	{
	}

	public interface ITsX70GvBaseAdapterSettings : ITswXX70BaseAdapterSettings
	{
	}
}