using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.X50;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52
{
	public abstract class AbstractTswX52ButtonVoiceControlAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings>, ITswX52ButtonVoiceControlAdapter
		where TPanel : Tswx52ButtonVoiceControl
		where TSettings : ITswX52ButtonVoiceControlAdapterSettings, new()
	{
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
