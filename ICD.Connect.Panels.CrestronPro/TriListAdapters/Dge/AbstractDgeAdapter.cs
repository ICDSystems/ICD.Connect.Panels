namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public abstract class AbstractDgeAdapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, IDgeAdapter
		where TPanel : Crestron.SimplSharpPro.UI.Dge
#else
	public abstract class AbstractDgeAdapter<TSettings> : AbstractTriListAdapter<TSettings>, IDgeAdapter
#endif
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
