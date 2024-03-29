﻿using ICD.Connect.API.Nodes;
using ICD.Connect.Misc.CrestronPro.Utils;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.Settings;
using ISmartObject = ICD.Connect.Panels.SmartObjects.ISmartObject;
#if !NETSTANDARD
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Misc.CrestronPro;
using ICD.Connect.Misc.CrestronPro.Sigs;
using ICD.Connect.Misc.CrestronPro.Extensions;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.SmartObjects;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Services.Logging;
#else
using System;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts
{
	/// <summary>
	/// TriListAdapter wraps a TriList to provide IPanelDevice features.
	/// </summary>
#if !NETSTANDARD
	public abstract class AbstractTriListAdapter<TPanel, TSettings> : AbstractPanelDevice<TSettings>, ITriListAdapter
		where TPanel : BasicTriListWithSmartObject
#else
    public abstract class AbstractTriListAdapter<TSettings> : AbstractPanelDevice<TSettings>
#endif
		where TSettings : ITriListAdapterSettings, new()
	{
#if !NETSTANDARD
		/// <summary>
		/// Raised when the internal wrapped panel changes.
		/// </summary>
		public event PanelChangeCallback OnPanelChanged;

		private readonly DeviceBooleanInputCollectionAdapter m_BooleanInput;
		private readonly DeviceUShortInputCollectionAdapter m_UShortInput;
		private readonly DeviceStringInputCollectionAdapter m_StringInput;
		private readonly DeviceBooleanOutputCollectionAdapter m_BooleanOutput;
		private readonly DeviceUShortOutputCollectionAdapter m_UShortOutput;
		private readonly DeviceStringOutputCollectionAdapter m_StringOutput;
		private readonly SmartObjectCollectionAdapter m_SmartObjects;

		private TPanel m_Panel;
#endif

		#region Properties

#if !NETSTANDARD
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
		public override IDeviceBooleanInputCollection BooleanInput
		{
			get
			{
#if !NETSTANDARD
				return m_BooleanInput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection of Integer Inputs sent to the panel.
		/// </summary>
		public override IDeviceUShortInputCollection UShortInput
		{
			get
			{
#if !NETSTANDARD
				return m_UShortInput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection of String Inputs sent to the panel.
		/// </summary>
		public override IDeviceStringInputCollection StringInput
		{
			get
			{
#if !NETSTANDARD
				return m_StringInput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection of Boolean Outputs sent from the panel.
		/// </summary>
		public override IDeviceBooleanOutputCollection BooleanOutput
		{
			get
			{
#if !NETSTANDARD
				return m_BooleanOutput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection of Integer Outputs sent from the panel.
		/// </summary>
		public override IDeviceUShortOutputCollection UShortOutput
		{
			get
			{
#if !NETSTANDARD
				return m_UShortOutput;
#else
				throw new NotSupportedException();
#endif
			}
		}

		/// <summary>
		/// Collection of String Outputs sent from the panel.
		/// </summary>
		public override IDeviceStringOutputCollection StringOutput
		{
			get
			{
#if !NETSTANDARD
				return m_StringOutput;
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
#if !NETSTANDARD
				return m_SmartObjects;
#else
				throw new NotSupportedException();
#endif
			}
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractTriListAdapter()
		{
#if !NETSTANDARD
			m_SmartObjects = new SmartObjectCollectionAdapter();
			m_BooleanInput = new DeviceBooleanInputCollectionAdapter();
			m_UShortInput = new DeviceUShortInputCollectionAdapter();
			m_StringInput = new DeviceStringInputCollectionAdapter();
			m_BooleanOutput = new DeviceBooleanOutputCollectionAdapter();
			m_UShortOutput = new DeviceUShortOutputCollectionAdapter();
			m_StringOutput = new DeviceStringOutputCollectionAdapter();

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

#if !NETSTANDARD
			// Unsubscribe and unregister
			Unsubscribe(m_SmartObjects);
			SetPanel(null);
#endif
		}

#if !NETSTANDARD
		/// <summary>
		/// Sets the wrapped panel device.
		/// </summary>
		/// <param name="panel"></param>
		[PublicAPI]
		public virtual void SetPanel(TPanel panel)
		{
			if (panel == Panel)
				return;

			Unsubscribe(Panel);

			if (Panel != null)
				GenericBaseUtils.TearDown(Panel);

			m_Panel = panel;

			eDeviceRegistrationUnRegistrationResponse result;
			if (Panel != null && !GenericBaseUtils.SetUp(Panel, this, RegisterExtenders, out result))
				Logger.Log(eSeverity.Error, "Unable to register {0} - {1}", Panel.GetType().Name, result);

			m_BooleanInput.SetCollection(Panel == null ? null : Panel.BooleanInput);
			m_UShortInput.SetCollection(Panel == null ? null : Panel.UShortInput);
			m_StringInput.SetCollection(Panel == null ? null : Panel.StringInput);
			m_BooleanOutput.SetCollection(Panel == null ? null : Panel.BooleanOutput);
			m_UShortOutput.SetCollection(Panel == null ? null : Panel.UShortOutput);
			m_StringOutput.SetCollection(Panel == null ? null : Panel.StringOutput);
			m_SmartObjects.SetSmartObjects(Panel == null ? null : Panel.SmartObjects);

			// Set the panel to "offline" visually by default.
			if (m_Panel != null)
				SendInputDigital(CommonJoins.DIGITAL_OFFLINE_JOIN, true);

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
#if !NETSTANDARD
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

#if !NETSTANDARD
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

#if !NETSTANDARD
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

#if !NETSTANDARD
			TPanel triList = settings.Ipid == null
							 ? null 
							 : InstantiateTriList(settings.Ipid.Value, ProgramInfo.ControlSystem);
			SetPanel(triList);
#else
			throw new NotSupportedException();
#endif
		}

#if !NETSTANDARD
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

#if !NETSTANDARD
		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		/// <param name="panel"></param>
		protected virtual void Subscribe(TPanel panel)
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
		protected virtual void Unsubscribe(TPanel panel)
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
			SigInfo sigInfo = args.Sig.ToSigInfo();

			RaiseOutputSigChangeCallback(sigInfo);
		}
#endif

		#endregion

		#region SmartObject Callbacks

		private void Subscribe(ISmartObjectCollection smartObjects)
		{
			smartObjects.OnSmartObjectAdded += SmartObjectsOnSmartObjectAdded;
			smartObjects.OnSmartObjectRemoved += SmartObjectsOnSmartObjectRemoved;
		}

		private void Unsubscribe(ISmartObjectCollection smartObjects)
		{
			smartObjects.OnSmartObjectAdded -= SmartObjectsOnSmartObjectAdded;
			smartObjects.OnSmartObjectRemoved -= SmartObjectsOnSmartObjectRemoved;
		}

		/// <summary>
		/// Subscribes to SmartObject Touch Events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="smartObject"></param>
		private void SmartObjectsOnSmartObjectAdded(object sender, ISmartObject smartObject)
		{
			smartObject.OnAnyOutput += SmartObjectOnAnyOutput;
		}

		/// <summary>
		/// Unsubscribes from SmartObject Touch Events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="smartObject"></param>
		private void SmartObjectsOnSmartObjectRemoved(object sender, ISmartObject smartObject)
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

#if !NETSTANDARD
			addRow("IPID", m_Panel == null ? null : StringUtils.ToIpIdString((byte)m_Panel.ID));
#endif
		}

		#endregion
	}
}
