#if !NETSTANDARD
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Panels.Controls.Backlight;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons
{
#if !NETSTANDARD
	public abstract class AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonAdapter<TPanel, TSettings>, ITswFt5ButtonSystemAdapter
		where TPanel : TswFt5ButtonSystem
#else
	public abstract class AbstractTswFt5ButtonSystemAdapter<TSettings> :
		AbstractTswFt5ButtonAdapter<TSettings>, ITswFt5ButtonSystemAdapter
#endif
		where TSettings : ITswFt5ButtonSystemAdapterSettings, new()
	{
#if !NETSTANDARD
		/// <summary>
		/// Called from constructor.
		/// Override to control the type of dialing control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override ITraditionalConferenceDeviceControl InstantiateDialingControl(int id)
		{
			return new TswFt5ButtonSystemConferenceControl(this, id);
		}

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of backlight control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IBacklightDeviceControl InstantiateBacklightControl(int id)
		{
			return new TswFt5ButtonSystemBacklightControl(this, id);
		}

		/// <summary>
		/// Registers the VoIP extender for the given panel.
		/// </summary>
		/// <param name="panel"></param>
		protected override void RegisterVoIpExtender(TPanel panel)
		{
			panel.ExtenderVoipReservedSigs.Use();
		}

		/// <summary>
		/// Registers the system extender for the given panel.
		/// </summary>
		/// <param name="panel"></param>
		protected override void RegisterSystemExtender(TPanel panel)
		{
			panel.ExtenderSystemReservedSigs.Use();
		}

		private void RegisterEthernetExternder(TPanel panel)
		{
			panel.ExtenderEthernetReservedSigs.Use();
		}
#endif
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
