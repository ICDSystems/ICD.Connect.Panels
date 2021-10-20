#if !NETSTANDARD
using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.UI;
using ICD.Common.Properties;
using ICD.Connect.Misc.CrestronPro.Extensions;
using ICD.Connect.Panels.Controls.Backlight;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.XX70;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight
{
	public abstract class AbstractTswXX70BaseBacklightControl<TParent, TPanel, TSystemSigs> : AbstractBacklightDeviceControl<TParent>
		where TParent : ITswXX70BaseAdapter
		where TPanel : TswXX70Base
		where TSystemSigs : TsxSystemReservedSigs
	{
		private readonly Dictionary<Sig, Action<Sig>> m_SigCallbackMap;

		#region Properties

		/// <summary>
		/// Gets the voip sig extender for the panel.
		/// </summary>
		[CanBeNull]
		protected abstract TSystemSigs Sigs { get; }

		/// <summary>
		/// Gets the current panel for the control.
		/// </summary>
		[CanBeNull]
		protected TPanel Panel { get; private set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractTswXX70BaseBacklightControl(TParent parent, int id)
			: base(parent, id)
		{
			m_SigCallbackMap = new Dictionary<Sig, Action<Sig>>();

			SetPanel(parent.Panel as TPanel);
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			base.DisposeFinal(disposing);

			UnsubscribePanel();
		}

		/// <summary>
		/// Turns on the backlight.
		/// </summary>
		public override void BacklightOn()
		{
			if (Panel == null)
				throw new InvalidOperationException("No panel");

			if (Sigs == null)
				throw new InvalidOperationException("No system extender");

			Sigs.BacklightOn();
		}

		/// <summary>
		/// Turns off the backlight.
		/// </summary>
		public override void BacklightOff()
		{
			if (Panel == null)
				throw new InvalidOperationException("No panel");

			if (Sigs == null)
				throw new InvalidOperationException("No system extender");

			Sigs.BacklightOff();
		}

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Subscribe(TParent parent)
		{
			base.Subscribe(parent);

			parent.OnPanelChanged += ParentOnPanelChanged;
		}

		/// <summary>
		/// Unsubscribe from the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Unsubscribe(TParent parent)
		{
			base.Unsubscribe(parent);

			parent.OnPanelChanged -= ParentOnPanelChanged;
		}

		/// <summary>
		/// Called when the wrapped panel instance changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="panel"></param>
		private void ParentOnPanelChanged(ITriListAdapter sender, BasicTriListWithSmartObject panel)
		{
			SetPanel(panel as TPanel);
		}

		/// <summary>
		/// Sets the current subscribed panel to match the parent device.
		/// </summary>
		/// <param name="panel"></param>
		private void SetPanel(TPanel panel)
		{
			if (panel == Panel)
				return;

			UnsubscribePanel();

			Panel = panel;

			SubscribePanel();

			UpdateIsPowered();
		}

		#endregion

		#region Panel Callbacks

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		private void SubscribePanel()
		{
			BuildSigCallbacks(m_SigCallbackMap);

			if (Panel == null)
				return;

			TSystemSigs sigs = Sigs;
			if (sigs == null)
				return;

			sigs.DeviceExtenderSigChange += SigsOnDeviceExtenderSigChange;
		}

		/// <summary>
		/// Unsubscribe from the panel events.
		/// </summary>
		private void UnsubscribePanel()
		{
			m_SigCallbackMap.Clear();

			if (Panel == null)
				return;

			TSystemSigs sigs = Sigs;
			if (sigs == null)
				return;

			sigs.DeviceExtenderSigChange -= SigsOnDeviceExtenderSigChange;
		}

		/// <summary>
		/// Populates the sig callback map with handlers for sig changes.
		/// </summary>
		protected virtual void BuildSigCallbacks(Dictionary<Sig, Action<Sig>> map)
		{
			TSystemSigs sigs = Sigs;
			if (sigs == null)
				return;

			map[sigs.BacklightOnFeedback] = HandleBacklightOnFeedback;
			map[sigs.BacklightOffFeedback] = HandleBacklightOffFeedback;
		}

		/// <summary>
		/// Called when the backlight on state changes.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleBacklightOnFeedback(Sig sig)
		{
			if (sig.BoolValue)
				UpdateIsPowered();
		}

		/// <summary>
		/// Called when the backlight off state changes.
		/// </summary>
		/// <param name="sig"></param>
		private void HandleBacklightOffFeedback(Sig sig)
		{
			if (sig.BoolValue)
				UpdateIsPowered();
		}

		/// <summary>
		/// Sets the powered state to match the panel backlight state.
		/// </summary>
		private void UpdateIsPowered()
		{
			bool backlightOn = Panel != null && Sigs != null && Sigs.BacklightOnFeedback.GetBoolValueOrDefault();

			BacklightState = backlightOn ? eBacklightState.BacklightOn : eBacklightState.BacklightOff;
		}

		/// <summary>
		/// Called when a sig on the VOIP extender changes.
		/// </summary>
		/// <param name="extender"></param>
		/// <param name="args"></param>
		private void SigsOnDeviceExtenderSigChange(DeviceExtender extender, SigEventArgs args)
		{
			Action<Sig> callback;
			if (m_SigCallbackMap.TryGetValue(args.Sig, out callback))
				callback(args.Sig);
		}

		#endregion
	}
}
#endif
