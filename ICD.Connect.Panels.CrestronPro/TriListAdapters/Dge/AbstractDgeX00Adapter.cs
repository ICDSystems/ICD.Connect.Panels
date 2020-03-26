namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public abstract class AbstractDgeX00Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, IDgeX00Adapter
		where TPanel : global::Crestron.SimplSharpPro.UI.Dge100
#else
	public abstract class AbstractDgeX00Adapter<TSettings> : AbstractTriListAdapter<TSettings>, IDgeX00Adapter
#endif
		where TSettings : IDgeX00AdapterSettings, new()
	{
	}
	public abstract class AbstractDgeX00AdapterSettings : AbstractTriListAdapterSettings, IDgeX00AdapterSettings
	{
	}

	public interface IDgeX00Adapter : ITriListAdapter
	{
	}

	public interface IDgeX00AdapterSettings : ITriListAdapterSettings
	{
	}
}
