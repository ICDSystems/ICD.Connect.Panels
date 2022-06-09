#if !NETSTANDARD
using System;
using ICD.Connect.API.Nodes;
using ICD.Connect.Misc.CrestronPro.Extensions;
using System.Collections.Generic;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.UI;
using ICD.Common.Utils;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Panels.Controls.HardButtons;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.HardButtons
{
// ReSharper disable once InconsistentNaming
	public sealed class TswXX70HardButtonBacklightControl : AbstractHardButtonBacklightControl<ITswXX70BaseAdapter>
	{

		private readonly Dictionary<int, bool> m_CachedBacklightState;
		private readonly SafeCriticalSection m_CriticalSection;


		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public TswXX70HardButtonBacklightControl(ITswXX70BaseAdapter parent, int id) : base(parent, id)
		{
			m_CachedBacklightState = new Dictionary<int, bool>();
			m_CriticalSection = new SafeCriticalSection();

			//Set Default State for button 6 to off
			m_CachedBacklightState.Add(6, false);

			parent.OnIsOnlineStateChanged += ParentOnIsOnlineStateChanged;
		}

		/// <summary>
		/// Sets the enabled state of the backlight for the button at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="enabled"></param>
		public override void SetBacklightEnabled(int address, bool enabled)
		{
			if (address < 1 || address > 6)
				throw new ArgumentOutOfRangeException("address", "No button at address " + address);

			m_CriticalSection.Enter();

			try
			{
				m_CachedBacklightState[address] = enabled;

				SetBacklightEnabledInternal(address, enabled);
			}
			finally
			{
				m_CriticalSection.Leave();
			}
		}

		private void SetBacklightEnabledInternal(int address, bool enabled)
		{
			TswXX70Base panel = Parent.Panel as TswXX70Base;
			if (panel == null)
			{
				Logger.Log(eSeverity.Error, "Unable to set button backlight state - internal panel is null");
				return;
			}

			Tswxx70ButtonToolbarReservedSigs sigs = panel.ExtenderButtonToolbarReservedSigs;

			switch (address)
			{
				case 1:
					if (enabled)
						sigs.Button1On();
					else
						sigs.Button1Off();
					break;

				case 2:
					if (enabled)
						sigs.Button2On();
					else
						sigs.Button2Off();
					break;

				case 3:
					if (enabled)
						sigs.Button3On();
					else
						sigs.Button3Off();
					break;

				case 4:
					if (enabled)
						sigs.Button4On();
					else
						sigs.Button4Off();
					break;

				case 5:
					if (enabled)
						sigs.Button5On();
					else
						sigs.Button5Off();
					break;

				case 6:
					if (enabled)
						sigs.Button6On();
					else
						sigs.Button6Off();
					break;

				default:
					throw new ArgumentOutOfRangeException("address", "No button at address " + address);
			}
		}

		private bool GetButtonOnFeedback(int address)
		{
			TswXX70Base panel = Parent.Panel as TswXX70Base;
			if (panel == null)
			{
				Logger.Log(eSeverity.Error, "Unable to get button state - internal panel is null");
				return false;
			}

			Tswxx70ButtonToolbarReservedSigs sigs = panel.ExtenderButtonToolbarReservedSigs;

			switch (address)
			{
				case 1:
					return sigs.Button1OnFeedback.GetBoolValueOrDefault();
				case 2:
					return sigs.Button2OnFeedback.GetBoolValueOrDefault();
				case 3:
					return sigs.Button3OnFeedback.GetBoolValueOrDefault();
				case 4:
					return sigs.Button4OnFeedback.GetBoolValueOrDefault();
				case 5:
					return sigs.Button5OnFeedback.GetBoolValueOrDefault();
				case 6:
					return sigs.Button6OnFeedback.GetBoolValueOrDefault();
					
				default:
					Logger.Log(eSeverity.Error, "unable to get button state - no button {0}", address);
					return false;
			}


		}

		/// <summary>
		/// Called when the panel online state changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ParentOnIsOnlineStateChanged(object sender, DeviceBaseOnlineStateApiEventArgs eventArgs)
		{
			if (!eventArgs.Data)
				return;

			m_CriticalSection.Enter();

			try
			{
				foreach (var kvp in m_CachedBacklightState)
					SetBacklightEnabledInternal(kvp.Key, kvp.Value);
			}
			finally
			{
				m_CriticalSection.Leave();
			}
		}

#region Console

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			for (int i = 1; i <= 5; i++)
			{
				addRow(string.Format("Button {0} On:", i), GetButtonOnFeedback(i));
			}
		}

#endregion
	}
}
#endif