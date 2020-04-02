#if SIMPLSHARP
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public abstract class AbstractDgeX00Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, IDgeX00Adapter<TPanel>
		where TPanel : global::Crestron.SimplSharpPro.UI.Dge100
#else
	public abstract class AbstractDgeX00Adapter<TSettings> : AbstractTriListAdapter<TSettings>, IDgeX00Adapter
#endif
		where TSettings : IDgeX00AdapterSettings, new()
	{
#if SIMPLSHARP
		public TPanel Dge { get { return Panel; } }
#endif
	}

	public abstract class AbstractDgeX00AdapterSettings : AbstractTriListAdapterSettings, IDgeX00AdapterSettings
	{
	}

	public interface IDgeX00Adapter : ITriListAdapter
	{


	}

#if SIMPLSHARP
	public interface IDgeX00Adapter<TPanel> : IDgeX00Adapter
		where TPanel : Dge100
	{
		TPanel Dge { get; }
	}
#endif

	public interface IDgeX00AdapterSettings : ITriListAdapterSettings
	{
	}
}
