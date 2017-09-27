using Crestron.SimplSharpPro.UI;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
	public abstract class AbstractDge1BaseAdapter<TPanel, TSettings> : AbstractDgeAdapter<TPanel, TSettings>, IDge1BaseAdapter
		where TPanel : Dge1Base
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
