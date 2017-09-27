namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Ts1542
{
	public abstract class AbstractTs1542Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, ITs1542Adapter
		where TPanel : Crestron.SimplSharpPro.UI.Ts1542
		where TSettings : ITs1542AdapterSettings, new()
	{
	}

	public abstract class AbstractTs1542AdapterSettings : AbstractTriListAdapterSettings, ITs1542AdapterSettings
	{
	}

	public interface ITs1542Adapter : ITriListAdapter
	{
	}

	public interface ITs1542AdapterSettings : ITriListAdapterSettings
	{
	}
}
