using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50
{
	public abstract class AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonAdapter<TPanel, TSettings>, ITswFt5ButtonSystemAdapter
		where TPanel : TswFt5ButtonSystem
		where TSettings : ITswFt5ButtonSystemAdapterSettings, new()
	{
		/// <summary>
		/// Called from constructor.
		/// Override to control the type of dialing control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IDialingDeviceControl InstantiateDialingControl(int id)
		{
			return new TswFt5ButtonSystemDialingControl(this, id);
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

	public interface ITswFt5ButtonSystemAdapter : ITswFt5ButtonAdapter
	{
	}

	public interface ITswFt5ButtonSystemAdapterSettings : ITswFt5ButtonAdapterSettings
	{
	}

	public abstract class AbstractTswFt5ButtonSystemAdapterSettings : AbstractTswFt5ButtonAdapterSettings,
	                                                                  ITswFt5ButtonSystemAdapterSettings
	{

	}
}
