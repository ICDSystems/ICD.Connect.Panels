using Crestron.SimplSharpPro.UI;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
	public abstract class AbstractDge2BaseAdapter<TPanel, TSettings> : AbstractDgeAdapter<TPanel, TSettings>, IDge2BaseAdapter
		where TPanel : Dge2Base
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
