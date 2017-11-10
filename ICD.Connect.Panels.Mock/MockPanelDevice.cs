using ICD.Common.Properties;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Panels.SmartObjectCollections;
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
		protected override IDeviceBooleanInputCollection BooleanInput { get { return m_BooleanInput; } }

		/// <summary>
		/// Collection of Integer Inputs sent to the device.
		/// </summary>
		protected override IDeviceUShortInputCollection UShortInput { get { return m_UShortInput; } }

		/// <summary>
		/// Collection of String Inputs sent to the device.
		/// </summary>
		protected override IDeviceStringInputCollection StringInput { get { return m_StringInput; } }

		/// <summary>
		/// Collection containing the loaded SmartObjects of this device.
		/// </summary>
		public override ISmartObjectCollection SmartObjects { get { return m_SmartObjects; } }

		#endregion

		public MockPanelDevice()
		{
			m_BooleanInput = new MockBooleanInputCollection();
			m_UShortInput = new MockUShortInputCollection();
			m_StringInput = new MockStringInputCollection();
			m_SmartObjects = new MockSmartObjectCollection();
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
	}
}
