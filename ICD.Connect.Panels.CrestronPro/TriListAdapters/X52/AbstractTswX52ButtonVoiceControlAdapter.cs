#if SIMPLSHARP
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52
{
#if SIMPLSHARP
	public abstract class AbstractTswX52ButtonVoiceControlAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings>, ITswX52ButtonVoiceControlAdapter
		where TPanel : Tswx52ButtonVoiceControl
#else
	public abstract class AbstractTswX52ButtonVoiceControlAdapter<TSettings> :
		AbstractTswFt5ButtonSystemAdapter<TSettings>, ITswX52ButtonVoiceControlAdapter
#endif
		where TSettings : ITswX52ButtonVoiceControlAdapterSettings, new()
	{
#if SIMPLSHARP
		/// <summary>
		/// Called from constructor.
		/// Override to control the type of dialing control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IDialingDeviceControl InstantiateDialingControl(int id)
		{
			return new TswX52ButtonVoiceControlDialingControl(this, id);
		}

		/// <summary>
		/// Registers the VoIP extender for the given panel.
		/// </summary>
		/// <param name="panel"></param>
		protected override void RegisterVoIpExtender(TPanel panel)
		{
			panel.ExtenderVoipReservedSigs.Use();
		}
#endif
	}

	public abstract class AbstractTswX52ButtonVoiceControlAdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings,
	                                                                        ITswX52ButtonVoiceControlAdapterSettings
	{
	}

	public interface ITswX52ButtonVoiceControlAdapterSettings : ITswFt5ButtonSystemAdapterSettings
	{
	}

	public interface ITswX52ButtonVoiceControlAdapter : ITswFt5ButtonSystemAdapter
	{
	}
}
