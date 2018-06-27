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
    public sealed class MockSmartObjectTest
    {
	    [Test]
	    public void OnAnyOutputTest()
	    {
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

	        MockSmartObject MockSmartObject = new MockSmartObject();
	        MockSmartObject.OnAnyOutput += (sender, args) => callbackArgs.Add(args);

			SigInfo info = new SigInfo(1, 1, true);
	        MockSmartObject.RaiseOutputSigChange(info);

			Assert.AreEqual(1, callbackArgs.Count);
			Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
		}

	    [Test]
	    public void LastOutputTest()
	    {
		    MockSmartObject MockSmartObject = new MockSmartObject();
			Assert.AreEqual(null, MockSmartObject.LastOutput);

			MockSmartObject.RaiseOutputSigChange(new SigInfo());
			
			Assert.AreEqual(0, (IcdEnvironment.GetLocalTime() - (DateTime)MockSmartObject.LastOutput).TotalSeconds, 1);
	    }

	    [Test]
		public void RegisterSigChangeCallbackTest()
		{
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

			MockSmartObject MockSmartObject = new MockSmartObject();
			Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);
			MockSmartObject.RegisterOutputSigChangeCallback(1, eSigType.Digital, callback);

			SigInfo info = new SigInfo(1, 1, true);
			MockSmartObject.RaiseOutputSigChange(info);

		    SigInfo info2 = new SigInfo(2, 1, true);
		    MockSmartObject.RaiseOutputSigChange(info2);

		    SigInfo info3 = new SigInfo(1, 1, "false");
		    MockSmartObject.RaiseOutputSigChange(info3);

            Assert.AreEqual(1, callbackArgs.Count);
			Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
		}

	    [Test]
		public void UnregisterOutputSigChangeCallbackTest()
		{
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

			MockSmartObject MockSmartObject = new MockSmartObject();
			Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);

			MockSmartObject.RegisterOutputSigChangeCallback(1, eSigType.Digital, callback);
			MockSmartObject.UnregisterOutputSigChangeCallback(1, eSigType.Digital, callback);

			SigInfo info = new SigInfo(1, 1, true);
			MockSmartObject.RaiseOutputSigChange(info);

			Assert.AreEqual(0, callbackArgs.Count);
		}

        [Test]
        public void RaiseOutputSigChangeTest()
        {
            List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

            MockSmartObject MockSmartObject = new MockSmartObject();
            MockSmartObject.OnAnyOutput += (sender, args) => callbackArgs.Add(args);

            SigInfo info = new SigInfo(1, 1, true);
            MockSmartObject.RaiseOutputSigChange(info);

            Assert.AreEqual(1, callbackArgs.Count);
            Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
        }

        [Test]
        public void ClearTest()
        {
            MockSmartObject MockSmartObject = new MockSmartObject();

            MockSmartObject.SendInputDigital(1,true);
            MockSmartObject.Clear();

            Assert.AreEqual(false, MockSmartObject.BooleanInput[1].GetBoolValue());
        }


    }
}
