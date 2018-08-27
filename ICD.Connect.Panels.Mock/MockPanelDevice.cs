using ICD.Common.Properties;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Mock
{
	/// <summary>
	/// MockPanelDevice provides a way for us to test UI code without using a panel.
	/// </summary>
	public sealed class MockPanelDevice : AbstractPanelDevice<MockPanelDeviceSettings>
	{
		private readonly IDeviceBooleanInputCollection m_BooleanInput;
		private readonly IDeviceUShortInputCollection m_UShortInput;
		private readonly IDeviceStringInputCollection m_StringInput;
		private readonly ISmartObjectCollection m_SmartObjects;

		#region Properties

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
			m_SmartObjects = new MockSmartObjectCollection();

            Subscribe(m_SmartObjects);
		}

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
			return true;
		}

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
    }
}
