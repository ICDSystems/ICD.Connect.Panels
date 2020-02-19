#if SIMPLSHARP
using System;
using System.Collections.Generic;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.DM.Endpoints;
using Crestron.SimplSharpPro.UI;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Routing;
using ICD.Connect.Routing.Connections;
using ICD.Connect.Routing.Controls;
using ICD.Connect.Routing.EventArguments;
using ICD.Connect.Routing.Utils;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Ts1542
{
	public sealed class Ts1542CRouteDestinationControl : AbstractRouteDestinationControl<Ts1542CAdapter>
	{
		/// <summary>
		/// Raised when an input source status changes.
		/// </summary>
		public override event EventHandler<SourceDetectionStateChangeEventArgs> OnSourceDetectionStateChange;

		/// <summary>
		/// Raised when the device starts/stops actively using an input, e.g. unroutes an input.
		/// </summary>
		public override event EventHandler<ActiveInputStateChangeEventArgs> OnActiveInputsChanged;

		private readonly SwitcherCache m_SwitcherCache;
		private Ts1542C m_Panel;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public Ts1542CRouteDestinationControl(Ts1542CAdapter parent, int id)
			: base(parent, id)
		{
			m_SwitcherCache = new SwitcherCache();
			Subscribe(m_SwitcherCache);

			SetPanel(parent.Panel);
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnSourceDetectionStateChange = null;
			OnActiveInputsChanged = null;

			base.DisposeFinal(disposing);

			Unsubscribe(m_SwitcherCache);
			SetPanel(null);
		}

		#region Methods

		/// <summary>
		/// Returns true if a signal is detected at the given input.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public override bool GetSignalDetectedState(int input, eConnectionType type)
		{
			if (!ContainsInput(input))
				throw new ArgumentOutOfRangeException("input");

			return m_SwitcherCache.GetSourceDetectedState(input, type);
		}

		/// <summary>
		/// Returns the true if the input is actively being used by the source device.
		/// For example, a display might true if the input is currently on screen,
		/// while a switcher may return true if the input is currently routed.
		/// </summary>
		public override bool GetInputActiveState(int input, eConnectionType type)
		{
			if (!ContainsInput(input))
				throw new ArgumentOutOfRangeException("input");

			return true;
		}

		/// <summary>
		/// Gets the input at the given address.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override ConnectorInfo GetInput(int input)
		{
			if (!ContainsInput(input))
				throw new ArgumentOutOfRangeException("input");

			return new ConnectorInfo(input, eConnectionType.Audio | eConnectionType.Video);
		}

		/// <summary>
		/// Returns true if the destination contains an input at the given address.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override bool ContainsInput(int input)
		{
			return input == 1;
		}

		/// <summary>
		/// Returns the inputs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetInputs()
		{
			yield return GetInput(1);
		}

		#endregion

		#region Parent Callbacks

		protected override void Subscribe(Ts1542CAdapter parent)
		{
			base.Subscribe(parent);

			parent.OnPanelChanged += ParentOnPanelChanged;
		}

		protected override void Unsubscribe(Ts1542CAdapter parent)
		{
			base.Unsubscribe(parent);

			parent.OnPanelChanged -= ParentOnPanelChanged;
		}

		private void ParentOnPanelChanged(ITriListAdapter sender, BasicTriListWithSmartObject panel)
		{
			SetPanel(panel as Ts1542C);
		}

		#endregion

		#region Panel Callbacks

		private void SetPanel(Ts1542C panel)
		{
			if (panel == m_Panel)
				return;

			Unsubscribe(m_Panel);
			m_Panel = panel;
			Subscribe(m_Panel);

			UpdateCache();
		}

		private void Subscribe(Ts1542C panel)
		{
			if (panel == null)
				return;

			panel.BaseEvent += PanelOnBaseEvent;
			panel.DmIn.InputStreamChange += DmInOnInputStreamChange;
		}

		private void Unsubscribe(Ts1542C panel)
		{
			if (panel == null)
				return;

			panel.BaseEvent -= PanelOnBaseEvent;
			panel.DmIn.InputStreamChange -= DmInOnInputStreamChange;
		}

		private void DmInOnInputStreamChange(EndpointInputStream inputStream, EndpointInputStreamEventArgs args)
		{
			UpdateCache();
		}

		private void PanelOnBaseEvent(GenericBase device, BaseEventArgs args)
		{
			UpdateCache();
		}

		private void UpdateCache()
		{
			bool detected = m_Panel != null && m_Panel.DmIn.SyncDetectedFeedback.BoolValue;

			m_SwitcherCache.SetSourceDetectedState(1, eConnectionType.Audio | eConnectionType.Video, detected);
		}

		#endregion

		#region SwitcherCache Callbacks

		private void Subscribe(SwitcherCache switcherCache)
		{
			switcherCache.OnSourceDetectionStateChange += SwitcherCacheOnSourceDetectionStateChange;
		}

		private void Unsubscribe(SwitcherCache switcherCache)
		{
			switcherCache.OnSourceDetectionStateChange -= SwitcherCacheOnSourceDetectionStateChange;
		}

		private void SwitcherCacheOnSourceDetectionStateChange(object sender, SourceDetectionStateChangeEventArgs eventArgs)
		{
			OnSourceDetectionStateChange.Raise(this, new SourceDetectionStateChangeEventArgs(eventArgs));
		}

		#endregion
	}
}
#endif
