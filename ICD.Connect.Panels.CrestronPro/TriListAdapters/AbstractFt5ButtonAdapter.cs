#if SIMPLSHARP
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
#if SIMPLSHARP
	public abstract class AbstractFt5ButtonAdapter<TPanel, TSettings> : AbstractTswFt5ButtonAdapter<TPanel, TSettings>, IFt5ButtonAdapter
		where TPanel : Ft5Button
#else
	public abstract class AbstractFt5ButtonAdapter<TSettings> : AbstractTswFt5ButtonAdapter<TSettings>, IFt5ButtonAdapter
#endif
		where TSettings : IFt5ButtonAdapterSettings, new()
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
