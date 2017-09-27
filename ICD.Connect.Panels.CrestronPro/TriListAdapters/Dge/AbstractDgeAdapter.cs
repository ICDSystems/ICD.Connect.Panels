namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
	public abstract class AbstractDgeAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, IDgeAdapter
		where TPanel : Crestron.SimplSharpPro.UI.Dge
		where TSettings : IDgeAdapterSettings, new()
	{
	}

	public abstract class AbstractDgeAdapterSettings : AbstractTriListAdapterSettings, IDgeAdapterSettings
	{
	}

	public interface IDgeAdapter : ITriListAdapter
	{
	}

	public interface IDgeAdapterSettings : ITriListAdapterSettings
	{
	}
}
