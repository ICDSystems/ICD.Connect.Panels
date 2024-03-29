﻿using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons;
#if !NETSTANDARD
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X52
{
#if !NETSTANDARD
	public abstract class AbstractTswX52ButtonVoiceControlAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings>, ITswX52ButtonVoiceControlAdapter
		where TPanel : Tswx52ButtonVoiceControl
#else
	public abstract class AbstractTswX52ButtonVoiceControlAdapter<TSettings> :
		AbstractTswFt5ButtonSystemAdapter<TSettings>, ITswX52ButtonVoiceControlAdapter
#endif
		where TSettings : ITswX52ButtonVoiceControlAdapterSettings, new()
	{
#if !NETSTANDARD
		/// <summary>
		/// Called from constructor.
		/// Override to control the type of dialing control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IConferenceDeviceControl InstantiateDialingControl(int id)
		{
			return new TswX52ButtonVoiceControlConferenceControl(this, id);
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
