#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70.TsX70
{
#if !NETSTANDARD
	public abstract class AbstractTsX70BaseAdapter<TPanel, TSettings> : AbstractTswXX70BaseAdapter<TPanel, TSettings>, ITsX70BaseAdapter
		where TPanel : TsX70Base
#else
	public abstract class AbstractTsX70BaseAdapter<TSettings> : AbstractTswXX70BaseAdapter<TSettings>, ITsX70BaseAdapter
#endif
		where TSettings : ITsX70BaseAdapterSettings, new()

	{
	}

	public abstract class AbstractTsX70BaseAdapterSettings : AbstractTswXX70BaseAdapterSettings,
	                                                               ITsX70BaseAdapterSettings
	{
	}

	public interface ITsX70BaseAdapter : ITswXX70BaseAdapter
	{
	}

	public interface ITsX70BaseAdapterSettings : ITswXX70BaseAdapterSettings
	{
	}
}