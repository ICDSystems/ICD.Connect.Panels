using Crestron.SimplSharpPro.UI;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50
{
	public abstract class AbstractFt5ButtonAdapter<TPanel, TSettings> : AbstractTswFt5ButtonAdapter<TPanel, TSettings>, IFt5ButtonAdapter
		where TSettings : IFt5ButtonAdapterSettings, new()
		where TPanel : Ft5Button
	{
	}

	public abstract class AbstractFt5ButtonAdapterSettings : AbstractTswFt5ButtonAdapterSettings, IFt5ButtonAdapterSettings
	{
	}

	public interface IFt5ButtonAdapter : ITswFt5ButtonAdapter
	{
	}

	public interface IFt5ButtonAdapterSettings : ITswFt5ButtonAdapterSettings
	{
	}
}
