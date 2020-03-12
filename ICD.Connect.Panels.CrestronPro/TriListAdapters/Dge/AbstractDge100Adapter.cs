using Crestron.SimplSharpPro.UI;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public abstract class AbstractDge100Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, IDge100Adapter
		where TPanel : Dge100
#else
	public abstract class AbstractDge100Adapter<TSettings> : AbstractTriListAdapter<TSettings>, IDge100Adapter
#endif
		where TSettings : IDge100AdapterSettings, new()
	{
	}
	public abstract class AbstractDge100AdapterSettings : AbstractTriListAdapterSettings, IDge100AdapterSettings
	{
	}

	public interface IDge100Adapter : ITriListAdapter
	{
	}

	public interface IDge100AdapterSettings : ITriListAdapterSettings
	{
	}
}
