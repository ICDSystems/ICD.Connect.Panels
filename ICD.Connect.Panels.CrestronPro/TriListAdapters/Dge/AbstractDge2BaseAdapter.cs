#if SIMPLSHARP
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public abstract class AbstractDge2BaseAdapter<TPanel, TSettings> : AbstractDgeAdapter<TPanel, TSettings>, IDge2BaseAdapter
		where TPanel : Dge2Base
#else
	public abstract class AbstractDge2BaseAdapter<TSettings> : AbstractDgeAdapter<TSettings>, IDge2BaseAdapter
#endif
		where TSettings : IDge2BaseAdapterSettings, new()
	{
	}

	public abstract class AbstractDge2BaseAdapterSettings : AbstractDgeAdapterSettings, IDge2BaseAdapterSettings
	{
	}

	public interface IDge2BaseAdapter : IDgeAdapter
	{
	}

	public interface IDge2BaseAdapterSettings : IDgeAdapterSettings
	{
	}
}
