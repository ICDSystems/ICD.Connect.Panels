﻿#if SIMPLSHARP
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Conferencing.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
#if SIMPLSHARP
	public abstract class AbstractTswFt5ButtonSystemAdapter<TPanel, TSettings> :
		AbstractTswFt5ButtonAdapter<TPanel, TSettings>, ITswFt5ButtonSystemAdapter
		where TPanel : TswFt5ButtonSystem
#else
	public abstract class AbstractTswFt5ButtonSystemAdapter<TSettings> :
		AbstractTswFt5ButtonAdapter<TSettings>, ITswFt5ButtonSystemAdapter
#endif
		where TSettings : ITswFt5ButtonSystemAdapterSettings, new()
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
