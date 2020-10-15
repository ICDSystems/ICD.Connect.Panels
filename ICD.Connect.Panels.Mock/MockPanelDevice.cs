using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Mock;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.Settings;

namespace ICD.Connect.Panels.Mock
{
	/// <summary>
	/// MockPanelDevice provides a way for us to test UI code without using a panel.
	/// </summary>
	public sealed class MockPanelDevice : AbstractPanelDevice<MockPanelDeviceSettings>, IMockDevice
	{
		private readonly IDeviceBooleanInputCollection m_BooleanInput;
		private readonly IDeviceUShortInputCollection m_UShortInput;
		private readonly IDeviceStringInputCollection m_StringInput;
		private readonly IDeviceBooleanOutputCollection m_BooleanOutput;
		private readonly IDeviceUShortOutputCollection m_UShortOutput;
		private readonly IDeviceStringOutputCollection m_StringOutput;
		private readonly ISmartObjectCollection m_SmartObjects;
		private bool m_IsOnline;

		#region Properties

		public bool DefaultOffline { get; set; }

		/// <summary>
		/// Collection of Boolean Inputs sent to the device.
		/// </summary>
		public override IDeviceBooleanInputCollection BooleanInput { get { return m_BooleanInput; } }

		/// <summary>
		/// Collection of Integer Inputs sent to the device.
		/// </summary>
		public override IDeviceUShortInputCollection UShortInput { get { return m_UShortInput; } }

		/// <summary>
		/// Collection of String Inputs sent to the device.
		/// </summary>
		public override IDeviceStringInputCollection StringInput { get { return m_StringInput; } }

		/// <summary>
		/// Collection of Boolean Outputs sent from the device.
		/// </summary>
		public override IDeviceBooleanOutputCollection BooleanOutput { get { return m_BooleanOutput; } }

		/// <summary>
		/// Collection of Integer Outputs sent from the device.
		/// </summary>
		public override IDeviceUShortOutputCollection UShortOutput { get { return m_UShortOutput; } }

		/// <summary>
		/// Collection of String Outputs sent from the device.
		/// </summary>
		public override IDeviceStringOutputCollection StringOutput { get { return m_StringOutput; } }

		/// <summary>
		/// Collection containing the loaded SmartObjects of this device.
		/// </summary>
		public override ISmartObjectCollection SmartObjects { get { return m_SmartObjects; } }

		#endregion

        /// <summary>
        /// Constructor.
        /// </summary>
		public MockPanelDevice()
		{
			m_BooleanInput = new MockBooleanInputCollection();
			m_UShortInput = new MockUShortInputCollection();
			m_StringInput = new MockStringInputCollection();
			m_BooleanOutput = new MockBooleanOutputCollection();
			m_UShortOutput = new MockUShortOutputCollection();
			m_StringOutput = new MockStringOutputCollection();
			m_SmartObjects = new MockSmartObjectCollection();

            Subscribe(m_SmartObjects);
		}

		#region Methods

		/// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing"></param>
	    protected override void DisposeFinal(bool disposing)
	    {
	        base.DisposeFinal(disposing);

            Unsubscribe(m_SmartObjects);
	    }

		/// <summary>
		/// Raises the sig change callbacks.
		/// </summary>
		/// <param name="sigInfo"></param>
		[PublicAPI]
		public void RaiseOutputSigChange(SigInfo sigInfo)
		{
			RaiseOutputSigChangeCallback(sigInfo);
		}

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			return m_IsOnline;
		}

		public void SetIsOnlineState(bool isOnline)
		{
			m_IsOnline = isOnline;
			UpdateCachedOnlineStatus();
		}

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

		#region Settings

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(MockPanelDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			MockDeviceHelper.ApplySettings(this, settings);
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(MockPanelDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);

			MockDeviceHelper.CopySettings(this,settings);
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			MockDeviceHelper.ClearSettings(this);
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

			MockDeviceHelper.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in MockDeviceHelper.GetConsoleCommands(this))
				yield return command;
		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
