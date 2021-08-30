#if !NETSTANDARD
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge
{
#if !NETSTANDARD
	public abstract class AbstractDge1BaseAdapter<TPanel, TSettings> : AbstractDgeAdapter<TPanel, TSettings>,
	                                                                   IDge1BaseAdapter
		where TPanel : Dge1Base
#else
	public abstract class AbstractDge1BaseAdapter<TSettings> : AbstractDgeAdapter<TSettings>, IDge1BaseAdapter
#endif
		where TSettings : IDge1BaseAdapterSettings, new()
	{
	}

	public abstract class AbstractDge1BaseAdapterSettings : AbstractDgeAdapterSettings, IDge1BaseAdapterSettings
	{
	}

	public interface IDge1BaseAdapter : IDgeAdapter
	{
	}

	public interface IDge1BaseAdapterSettings : IDgeAdapterSettings
	{
	}
}
