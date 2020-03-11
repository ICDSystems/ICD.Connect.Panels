using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;
using NUnit.Framework;

namespace ICD.Connect.Panels.Tests
{
	[TestFixture]
    public sealed class SigCallbackManagerTest
    {
	    [Test]
	    public void AnyCallbackTest()
	    {
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

		    SigCallbackManager manager = new SigCallbackManager();
		    manager.OnAnyCallback += (sender, args) => callbackArgs.Add(args);

			SigInfo info = new SigInfo(1, 1, true);
		    manager.RaiseSigChangeCallback(info);

			Assert.AreEqual(1, callbackArgs.Count);
			Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
		}

	    [Test]
	    public void LastOutputTest()
	    {
		    SigCallbackManager manager = new SigCallbackManager();
			Assert.AreEqual(null, manager.LastOutput);

			manager.RaiseSigChangeCallback(new SigInfo());
			
		    // ReSharper disable once PossibleInvalidOperationException
			Assert.AreEqual(0, (IcdEnvironment.GetUtcTime() - (DateTime)manager.LastOutput).TotalSeconds, 1);
	    }

		[Test]
		public void ClearTest()
	    {
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

		    SigCallbackManager manager = new SigCallbackManager();
		    Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);

		    manager.RegisterSigChangeCallback(1, eSigType.Digital, callback);
			manager.Clear();

		    SigInfo info = new SigInfo(1, 1, true);
		    manager.RaiseSigChangeCallback(info);

		    Assert.AreEqual(0, callbackArgs.Count);
		}

		[Test]
		public void RaiseSigChangeCallbackTest()
		{
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

			SigCallbackManager manager = new SigCallbackManager();
			Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);
			manager.RegisterSigChangeCallback(1, eSigType.Digital, callback);

			SigInfo info = new SigInfo(1, 1, true);
			manager.RaiseSigChangeCallback(info);

			Assert.AreEqual(1, callbackArgs.Count);
			Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
		}

	    [Test]
		public void RegisterSigChangeCallbackTest()
		{
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

			SigCallbackManager manager = new SigCallbackManager();
			Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);
			manager.RegisterSigChangeCallback(1, eSigType.Digital, callback);

			SigInfo info = new SigInfo(1, 1, true);
			manager.RaiseSigChangeCallback(info);

			Assert.AreEqual(1, callbackArgs.Count);
			Assert.AreEqual(info, callbackArgs.Select(a => a.Data).First());
		}

	    [Test]
		public void UnregisterSigChangeCallbackTest()
		{
			List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

			SigCallbackManager manager = new SigCallbackManager();
			Action<SigCallbackManager, SigInfoEventArgs> callback = (callbackManager, args) => callbackArgs.Add(args);

			manager.RegisterSigChangeCallback(1, eSigType.Digital, callback);
			manager.UnregisterSigChangeCallback(1, eSigType.Digital, callback);

			SigInfo info = new SigInfo(1, 1, true);
			manager.RaiseSigChangeCallback(info);

			Assert.AreEqual(0, callbackArgs.Count);
		}
	}
}
