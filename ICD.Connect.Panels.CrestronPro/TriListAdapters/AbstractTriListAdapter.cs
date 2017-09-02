using System;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using ICD.Connect.Misc.CrestronPro.Sigs;
#endif
using ICD.Common.Properties;
using ICD.Common.Services.Logging;
using ICD.Connect.Misc.CrestronPro;
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
	public abstract class AbstractTriListAdapter<TPanel, TSettings> : AbstractPanelDevice<TSettings>
		where TPanel : BasicTriListWithSmartObject
#else
    public abstract class AbstractTriListAdapter<TSettings> : AbstractPanelDevice<TSettings>
#endif
        where TSettings : AbstractTriListAdapterSettings, new()
	{
		private IDeviceBooleanInputCollection m_BooleanInput;
		private IDeviceUShortInputCollection m_UShortInput;
		private IDeviceStringInputCollection m_StringInput;
#if SIMPLSHARP
        private readonly SmartObjectCollectionAdapter m_SmartObjects;
#endif

#region Properties

#if SIMPLSHARP
        /// <summary>
        /// Gets the wrapped panel instance.
        /// </summary>
		[PublicAPI]
		[CanBeNull]
        public TPanel Device { get; private set; }
#endif

		/// <summary>
		/// Collection of Boolean Inputs sent to the panel.
		/// </summary>
		protected override IDeviceBooleanInputCollection BooleanInput
		{
			get
            {
#if SIMPLSHARP
				if (IsDisposed)
					throw new ObjectDisposedException(GetType().Name);

				if (Device == null)
					throw new InvalidOperationException("No device assigned");

                return m_BooleanInput ?? (m_BooleanInput = new DeviceBooleanInputCollectionAdapter(Device.BooleanInput));
#else
                throw new NotImplementedException();
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
				if (IsDisposed)
					throw new ObjectDisposedException(GetType().Name);

				if (Device == null)
					throw new InvalidOperationException("No device assigned");

                return m_UShortInput ?? (m_UShortInput = new DeviceUShortInputCollectionAdapter(Device.UShortInput));
#else
                throw new NotImplementedException();
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
				if (IsDisposed)
					throw new ObjectDisposedException(GetType().Name);

				if (Device == null)
					throw new InvalidOperationException("No device assigned");

                return m_StringInput ?? (m_StringInput = new DeviceStringInputCollectionAdapter(Device.StringInput));
#else
                throw new NotImplementedException();
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
				if (IsDisposed)
					throw new ObjectDisposedException(GetType().Name);

				if (Device == null)
					throw new InvalidOperationException("No device assigned");

                return m_SmartObjects;
#else
                throw new NotImplementedException();
#endif
            }
        }

#endregion

		protected AbstractTriListAdapter()
		{
#if SIMPLSHARP
            m_SmartObjects = new SmartObjectCollectionAdapter();
			m_SmartObjects.OnSmartObjectSubscribe += SmartObjectsOnSmartObjectSubscribe;
			m_SmartObjects.OnSmartObjectUnsubscribe += SmartObjectsOnSmartObjectUnsubscribe;
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
            SetDevice(null);
#endif
		}

#if SIMPLSHARP
		/// <summary>
		/// Sets the wrapped panel panel.
		/// </summary>
		/// <param name="device"></param>
		[PublicAPI]
		public void SetDevice(TPanel device)
		{
			Unsubscribe(Device);

			if (Device != null)
			{
				if (Device.Registered)
					Device.UnRegister();

				try
				{
					Device.Dispose();
				}
				catch
				{
				}
			}

		    m_BooleanInput = null;
			m_UShortInput = null;
			m_StringInput = null;

		    Device = device;
            m_SmartObjects.SetSmartObjects(Device == null ? null : Device.SmartObjects);

			if (Device != null && !Device.Registered)
			{
				if (Name != null)
					Device.Description = Name;

				eDeviceRegistrationUnRegistrationResponse result = Device.Register();
				if (result != eDeviceRegistrationUnRegistrationResponse.Success)
					Logger.AddEntry(eSeverity.Error, "Unable to register {0} - {1}", Device.GetType().Name, result);
			}

			Subscribe(Device);
			UpdateCachedOnlineStatus();
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
            return Device != null && Device.IsOnline;
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
            SetDevice(null);
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
            settings.Ipid = Device == null ? (byte)0 : (byte)Device.ID;
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
			SetDevice(triList);
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
				RaiseOutputSigChangeCallback(SigAdapterFactory.GetSigAdapter(args.Sig));
				RaiseOnAnyOutput();
			}
			catch (Exception e)
			{
				Logger.AddEntry(eSeverity.Error, e, "Port panel output sig change exception - {0}", e.Message);
			}
		}
#endif

#endregion

#region SmartObject Callbacks

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
		private void SmartObjectOnAnyOutput(object sender, EventArgs eventArgs)
		{
			RaiseOnAnyOutput();
		}

#endregion
	}
}
