using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;
using NUnit.Framework;

namespace ICD.Connect.Panels.Mock.Tests
{
	[TestFixture]
    public sealed class MockPanelDeviceTest
    {
	    [Test]
	    public void OnAnyOutputTest()
	    {
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

	        MockPanelDevice mockPanelDevice = new MockPanelDevice();
	        mockPanelDevice.OnAnyOutput += (sender, args) => callbackArgs.Add(args);

			SigInfo info = new SigInfo(1, 1, true);
	        mockPanelDevice.RaiseOutputSigChange(info);

            (mockPanelDevice.SmartObjects[1] as MockSmartObject).RaiseOutputSigChange(info);

			Assert.AreEqual(2, callbackArgs.Count);
			Assert.AreEqual(info, callbackArgs[0].Data);
	        Assert.AreEqual(info, callbackArgs[1].Data);
        }

	    [Test]
	    public void LastOutputTest()
	    {
		    MockPanelDevice mockPanelDevice = new MockPanelDevice();
			Assert.AreEqual(null, mockPanelDevice.LastOutput);

			mockPanelDevice.RaiseOutputSigChange(new SigInfo());
			
			Assert.AreEqual(0, (IcdEnvironment.GetUtcTime() - (DateTime)mockPanelDevice.LastOutput).TotalSeconds, 1);
	    }

	    [Test]
		public void RegisterSigChangeCallbackTest()
		{
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

			MockPanelDevice mockPanelDevice = new MockPanelDevice();
			Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);
			mockPanelDevice.RegisterOutputSigChangeCallback(1, eSigType.Digital, callback);

			SigInfo info = new SigInfo(1, 1, true);
			mockPanelDevice.RaiseOutputSigChange(info);

		    SigInfo info2 = new SigInfo(2, 1, true);
		    mockPanelDevice.RaiseOutputSigChange(info2);

		    SigInfo info3 = new SigInfo(1, 1, "false");
		    mockPanelDevice.RaiseOutputSigChange(info3);

            Assert.AreEqual(1, callbackArgs.Count);
			Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
		}

	    [Test]
		public void UnregisterOutputSigChangeCallbackTest()
		{
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

			MockPanelDevice mockPanelDevice = new MockPanelDevice();
			Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);

			mockPanelDevice.RegisterOutputSigChangeCallback(1, eSigType.Digital, callback);
			mockPanelDevice.UnregisterOutputSigChangeCallback(1, eSigType.Digital, callback);

			SigInfo info = new SigInfo(1, 1, true);
			mockPanelDevice.RaiseOutputSigChange(info);

			Assert.AreEqual(0, callbackArgs.Count);
		}

        [Test]
        public void RaiseOutputSigChangeTest()
        {
            List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

            MockPanelDevice mockPanelDevice = new MockPanelDevice();
            mockPanelDevice.OnAnyOutput += (sender, args) => callbackArgs.Add(args);

            SigInfo info = new SigInfo(1, 1, true);
            mockPanelDevice.RaiseOutputSigChange(info);

            Assert.AreEqual(1, callbackArgs.Count);
            Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
        }

        [Test]
        public void ClearTest()
        {
            MockPanelDevice mockPanelDevice = new MockPanelDevice();

            mockPanelDevice.SendInputDigital(1,true);
            mockPanelDevice.Clear();

            Assert.AreEqual(false, mockPanelDevice.BooleanInput[1].GetBoolValue());
        }

        [Test]
        public void SmartObjectsTest()
        {
            List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

            MockPanelDevice mockPanelDevice = new MockPanelDevice();
            mockPanelDevice.OnAnyOutput += (sender, args) => callbackArgs.Add(args);

            var ob = mockPanelDevice.SmartObjects[1] as MockSmartObject;

            Assert.NotNull(ob);

            SigInfo info = new SigInfo(1, 1, true);
            ob.RaiseOutputSigChange(info);
    
            Assert.AreEqual(1, callbackArgs.Count);
            Assert.AreEqual(0, (IcdEnvironment.GetUtcTime() - (DateTime)mockPanelDevice.LastOutput).TotalSeconds, 1);
        }

    }
}
