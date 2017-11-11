using System;
using ICD.Common.Utils;
using ICD.Connect.API.Nodes;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Misc.CrestronPro;
using ICD.Connect.Misc.CrestronPro.Sigs;
#endif
using ICD.Common.Properties;
using ICD.Common.Services.Logging;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Settings.Core;
using ISmartObject = ICD.Connect.Panels.SmartObjects.ISmartObject;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	/// <summary>
	/// TriListAdapter wraps a TriList to provide IPanelDevice features.
	/// </summary>
#if SIMPLSHARP
	public abstract class AbstractTriListAdapter<TPanel, TSettings> : AbstractPanelDevice<TSettings>, ITriListAdapter
		where TPanel : BasicTriListWithSmartObject
#else
    public abstract class AbstractTriListAdapter<TSettings> : AbstractPanelDevice<TSettings>
#endif
		where TSettings : ITriListAdapterSettings, new()
	{
#if SIMPLSHARP
		/// <summary>
		/// Raised when the internal wrapped panel changes.
		/// </summary>
		public event PanelChangeCallback OnPanelChanged;

		private readonly DeviceBooleanInputCollectionAdapter m_BooleanInput;
		private readonly DeviceUShortInputCollectionAdapter m_UShortInput;
		private readonly DeviceStringInputCollectionAdapter m_StringInput;
		private readonly SmartObjectCollectionAdapter m_SmartObjects;

		private TPanel m_Panel;
#endif

		#region Properties

#if SIMPLSHARP
		/// <summary>
		/// Gets the wrapped panel instance.
		/// </summary>
		[PublicAPI]
		[CanBeNull]
		public TPanel Panel { get { return m_Panel; } }

		/// <summary>
		/// Gets the internal wrapped panel instance.
		/// </summary>
		BasicTriListWithSmartObject ITriListAdapter.Panel { get { return Panel; } }
#endif

		/// <summary>
		/// Collection of Boolean Inputs sent to the panel.
		/// </summary>
		protected override IDeviceBooleanInputCollection BooleanInput
		{
			get
			{
#if SIMPLSHARP
				return m_BooleanInput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection of Integer Inputs sent to the panel.
		/// </summary>
		protected override IDeviceUShortInputCollection UShortInput
		{
			get
			{
#if SIMPLSHARP
				return m_UShortInput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection of String Inputs sent to the panel.
		/// </summary>
		protected override IDeviceStringInputCollection StringInput
		{
			get
			{
#if SIMPLSHARP
				return m_StringInput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection containing the loaded SmartObjects of this panel.
		/// </summary>
		public override ISmartObjectCollection SmartObjects
		{
			get
			{
#if SIMPLSHARP
				return m_SmartObjects;
#else
				throw new NotSupportedException();
#endif
			}
		}

		#endregion

		protected AbstractTriListAdapter()
		{
#if SIMPLSHARP
			m_SmartObjects = new SmartObjectCollectionAdapter();
			m_BooleanInput = new DeviceBooleanInputCollectionAdapter();
			m_UShortInput = new DeviceUShortInputCollectionAdapter();
			m_StringInput = new DeviceStringInputCollectionAdapter();

			Subscribe(m_SmartObjects);
#endif
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			base.DisposeFinal(disposing);

#if SIMPLSHARP
			// Unsubscribe and unregister
			Unsubscribe(m_SmartObjects);
			SetPanel(null);
#endif
		}

#if SIMPLSHARP
		/// <summary>
		/// Sets the wrapped panel device.
		/// </summary>
		/// <param name="panel"></param>
		[PublicAPI]
		public void SetPanel(TPanel panel)
		{
			if (panel == Panel)
				return;

			Unsubscribe(Panel);

			if (Panel != null)
			{
				if (Panel.Registered)
					Panel.UnRegister();

				try
				{
					Panel.Dispose();
				}
				catch
				{
				}
			}

			m_Panel = panel;

			m_BooleanInput.SetCollection(Panel == null ? null : Panel.BooleanInput);
			m_UShortInput.SetCollection(Panel == null ? null : Panel.UShortInput);
			m_StringInput.SetCollection(Panel == null ? null : Panel.StringInput);
			m_SmartObjects.SetSmartObjects(Panel == null ? null : Panel.SmartObjects);

			if (Panel != null && !Panel.Registered)
			{
				if (Name != null)
					Panel.Description = Name;

				RegisterExtenders(Panel);

				eDeviceRegistrationUnRegistrationResponse result = Panel.Register();
				if (result != eDeviceRegistrationUnRegistrationResponse.Success)
					Logger.AddEntry(eSeverity.Error, "Unable to register {0} - {1}", Panel.GetType().Name, result);
			}

			Subscribe(Panel);
			UpdateCachedOnlineStatus();

			PanelChangeCallback handler = OnPanelChanged;
			if (handler != null)
				handler(this, m_Panel);
		}

		/// <summary>
		/// Called before registration.
		/// Override to control which extenders are used with the panel.
		/// </summary>
		/// <param name="panel"></param>
		protected virtual void RegisterExtenders(TPanel panel)
		{
		}
#endif

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the current online status of the panel.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
#if SIMPLSHARP
			return Panel != null && Panel.IsOnline;
#else
            return false;
#endif
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

#if SIMPLSHARP
			SetPanel(null);
#endif
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

#if SIMPLSHARP
			settings.Ipid = Panel == null ? (byte)0 : (byte)Panel.ID;
#else
            settings.Ipid = 0;
#endif
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

#if SIMPLSHARP
			TPanel triList = InstantiateTriList(settings.Ipid, ProgramInfo.ControlSystem);
			SetPanel(triList);
#else
            throw new NotImplementedException();
#endif
		}

#if SIMPLSHARP
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected abstract TPanel InstantiateTriList(byte ipid, CrestronControlSystem controlSystem);
#endif

		#endregion

		#region Panel Callbacks

#if SIMPLSHARP
		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		/// <param name="panel"></param>
		private void Subscribe(TPanel panel)
		{
			if (panel == null)
				return;

			panel.SigChange += PanelOnSigChange;
			panel.OnlineStatusChange += PanelOnlineStatusChange;
		}

		/// <summary>
		/// Subscribe to the TriList events.
		/// </summary>
		/// <param name="panel"></param>
		private void Unsubscribe(TPanel panel)
		{
			if (panel == null)
				return;

			panel.SigChange -= PanelOnSigChange;
			panel.OnlineStatusChange -= PanelOnlineStatusChange;
		}

		/// <summary>
		/// Called when the panel goes online/offline.
		/// </summary>
		/// <param name="currentDevice"></param>
		/// <param name="args"></param>
		private void PanelOnlineStatusChange(GenericBase currentDevice, OnlineOfflineEventArgs args)
		{
			UpdateCachedOnlineStatus();
		}

		/// <summary>
		/// Called when a sig change comes from the TriList.
		/// </summary>
		/// <param name="currentDevice"></param>
		/// <param name="args"></param>
		private void PanelOnSigChange(BasicTriList currentDevice, SigEventArgs args)
		{
			try
			{
				ISig sig = SigAdapterFactory.GetSigAdapter(args.Sig);
				SigInfo sigInfo = new SigInfo(sig);

				RaiseOutputSigChangeCallback(sigInfo);
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "{0} output sig change exception", this);
			}
		}
#endif

		#endregion

		#region SmartObject Callbacks

		private void Subscribe(ISmartObjectCollection smartObjects)
		{
			smartObjects.OnSmartObjectSubscribe += SmartObjectsOnSmartObjectSubscribe;
			smartObjects.OnSmartObjectUnsubscribe += SmartObjectsOnSmartObjectUnsubscribe;
		}

		private void Unsubscribe(ISmartObjectCollection smartObjects)
		{
			smartObjects.OnSmartObjectSubscribe -= SmartObjectsOnSmartObjectSubscribe;
			smartObjects.OnSmartObjectUnsubscribe -= SmartObjectsOnSmartObjectUnsubscribe;
		}

		/// <summary>
		/// Subscribes to SmartObject Touch Events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="smartObject"></param>
		private void SmartObjectsOnSmartObjectSubscribe(object sender, ISmartObject smartObject)
		{
			smartObject.OnAnyOutput += SmartObjectOnAnyOutput;
		}

		/// <summary>
		/// Unsubscribes from SmartObject Touch Events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="smartObject"></param>
		private void SmartObjectsOnSmartObjectUnsubscribe(object sender, ISmartObject smartObject)
		{
			smartObject.OnAnyOutput -= SmartObjectOnAnyOutput;
		}

		/// <summary>
		/// Called when the user interacts with a SmartObject.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void SmartObjectOnAnyOutput(object sender, SigInfoEventArgs eventArgs)
		{
			RaiseOnAnyOutput(eventArgs.Data);
		}

		#endregion

		#region Console

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

#if SIMPLSHARP
			addRow("IPID", m_Panel == null ? null : StringUtils.ToIpIdString((byte)m_Panel.ID));
#endif
		}

		#endregion
	}
}
