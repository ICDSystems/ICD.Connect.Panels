namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Ts1542
{
#if SIMPLSHARP
	public abstract class AbstractTs1542Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>,
																	 ITs1542Adapter
		where TPanel : global::Crestron.SimplSharpPro.UI.Ts1542
#else
	public abstract class AbstractTs1542Adapter<TSettings> : AbstractTriListAdapter<TSettings>, ITs1542Adapter
#endif
		where TSettings : ITs1542AdapterSettings, new()
	{
	}
}