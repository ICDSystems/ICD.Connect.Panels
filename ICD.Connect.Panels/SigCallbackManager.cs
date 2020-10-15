using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels
{
	/// <summary>
	/// Simple class for registering/unregistering callbacks for sig changes.
	/// </summary>
	public sealed class SigCallbackManager
	{
		/// <summary>
		/// Raised when any sig callback is called.
		/// Useful for determining activity when a user interacts with a touchpanel.
		/// </summary>
		public event EventHandler<SigInfoEventArgs> OnAnyCallback;

		/// <summary>
		/// Maps sigs to registered callbacks.
		/// </summary>
		private readonly Dictionary<uint, Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>>>
			m_SigToCallback;

		private readonly SafeCriticalSection m_RegistrationSection;

		private DateTime? m_LastOutput;

		#region Properties

		/// <summary>
		/// Gets the time that a callback was called.
		/// </summary>
		public DateTime? LastOutput { get { return m_LastOutput; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public SigCallbackManager()
		{
			m_SigToCallback =
				new Dictionary<uint, Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>>>();
			m_RegistrationSection = new SafeCriticalSection();
		}

		#region Methods

		/// <summary>
		/// Clears all of the registered callbacks.
		/// </summary>
		public void Clear()
		{
			m_RegistrationSection.Execute(() => m_SigToCallback.Clear());
		}

		/// <summary>
		/// Gets all of the registered output sigs.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SigInfo> GetOutputSigs()
		{
			m_RegistrationSection.Enter();

			try
			{
				return m_SigToCallback.SelectMany(byNumber => byNumber.Value
				                                                      .Select(byType =>
				                                                              new SigInfo(byType.Key, byNumber.Key, null, 0)))
				                      .ToArray();
			}
			finally
			{
				m_RegistrationSection.Leave();
			}
		}

		/// <summary>
		/// Raises the callbacks registered with the signature.
		/// </summary>
		/// <param name="sigInfo"></param>
		public void RaiseSigChangeCallback(SigInfo sigInfo)
		{
			m_LastOutput = IcdEnvironment.GetUtcTime();

			OnAnyCallback.Raise(this, new SigInfoEventArgs(sigInfo));

			foreach (Action<SigCallbackManager, SigInfoEventArgs> callback in GetCallbacksForSig(sigInfo))
			{
				try
				{
					callback(this, new SigInfoEventArgs(sigInfo));
				}
				catch (Exception e)
				{
					ServiceProvider.TryGetService<ILoggerService>()
					               .AddEntry(eSeverity.Error, e, "Exception in callback");
				}
			}
		}

		/// <summary>
		/// Registers the callback for sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public void RegisterSigChangeCallback(uint number, eSigType type,
		                                      Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			m_RegistrationSection.Execute(() => RegisterCallback(m_SigToCallback, number, type, callback));
		}

		/// <summary>
		/// Unregisters the callback for sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public void UnregisterSigChangeCallback(uint number, eSigType type,
		                                        Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			m_RegistrationSection.Execute(() => UnregisterCallback(m_SigToCallback, number, type, callback));
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Adds the callback to the dictionary.
		/// </summary>
		/// <param name="callbacks"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		private static void RegisterCallback(
			IDictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>> callbacks, eSigType type,
			Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>> cache;
			if (!callbacks.TryGetValue(type, out cache))
			{
				cache = new IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>();
				callbacks[type] = cache;
			}

			cache.Add(callback);
		}

		/// <summary>
		/// Adds the callback to the dictionary.
		/// </summary>
		/// <param name="callbacks"></param>
		/// <param name="key"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		private static void RegisterCallback(
			IDictionary<uint, Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>>> callbacks,
			uint key, eSigType type, Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>> cache;
			if (!callbacks.TryGetValue(key, out cache))
			{
				cache = new Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>>();
				callbacks[key] = cache;
			}

			RegisterCallback(cache, type, callback);
		}

		/// <summary>
		/// Removes the callback from the dictionary.
		/// </summary>
		/// <param name="callbacks"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		private static void UnregisterCallback(
			IDictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>> callbacks, eSigType type,
			Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>> cache;
			if (callbacks.TryGetValue(type, out cache))
				cache.Remove(callback);
		}

		/// <summary>
		/// Removes the callback from the dictionary.
		/// </summary>
		/// <param name="callbacks"></param>
		/// <param name="key"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		private static void UnregisterCallback(
			IDictionary<uint, Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>>> callbacks,
			uint key, eSigType type, Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>> cache;
			if (callbacks.TryGetValue(key, out cache))
				UnregisterCallback(cache, type, callback);
		}

		/// <summary>
		/// Gets the registered callbacks matching the given sig.
		/// </summary>
		/// <param name="callbacks"></param>
		/// <param name="sig"></param>
		/// <returns></returns>
		private static IEnumerable<Action<SigCallbackManager, SigInfoEventArgs>> GetCallbacksForSig(
			IDictionary<uint, Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>>> callbacks,
			ISig sig)
		{
			Dictionary<eSigType, IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>>> cache;
			if (!callbacks.TryGetValue(sig.Number, out cache))
				return Enumerable.Empty<Action<SigCallbackManager, SigInfoEventArgs>>();

			IcdHashSet<Action<SigCallbackManager, SigInfoEventArgs>> innerCache;
			if (!cache.TryGetValue(sig.Type, out innerCache))
				return Enumerable.Empty<Action<SigCallbackManager, SigInfoEventArgs>>();

			return innerCache.ToArray(innerCache.Count);
		}

		/// <summary>
		/// Gets the registered callbacks matching the given sig.
		/// </summary>
		/// <param name="sig"></param>
		/// <returns></returns>
		private IEnumerable<Action<SigCallbackManager, SigInfoEventArgs>> GetCallbacksForSig(ISig sig)
		{
			return m_RegistrationSection.Execute(() => GetCallbacksForSig(m_SigToCallback, sig));
		}

		#endregion
	}
}
