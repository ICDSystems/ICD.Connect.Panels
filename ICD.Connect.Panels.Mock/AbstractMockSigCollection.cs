﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Mock
{
	public abstract class AbstractMockSigCollection<T> : ISigCollectionBase<T>
		where T : ISig
	{
		private readonly Dictionary<uint, T> m_SigCache;
		private readonly SafeCriticalSection m_SigCacheSection;

		#region Properties

		/// <summary>
		/// Returns the number of sig objects in the collection.
		/// </summary>
		[PublicAPI]
		public int NumberOfSigs { get { return m_SigCacheSection.Execute(() => m_SigCache.Count); } }

		/// <summary>
		/// Get the sig with the specified number.
		/// </summary>
		/// <param name="sigNumber">Number of the sig to return.</param>
		/// <returns/>
		/// <exception cref="T:System.IndexOutOfRangeException">Invalid Sig Number specified.</exception>
		public T this[uint sigNumber]
		{
			get
			{
				m_SigCacheSection.Enter();

				try
				{
					T sig;
					if (!m_SigCache.TryGetValue(sigNumber, out sig))
					{
						sig = InstantiateSig(sigNumber);
						m_SigCache[sigNumber] = sig;
					}

					return sig;
				}
				finally
				{
					m_SigCacheSection.Leave();
				}
			}
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractMockSigCollection()
		{
			m_SigCache = new Dictionary<uint, T>();
			m_SigCacheSection = new SafeCriticalSection();
		}

		[PublicAPI]
		public void AddSig(ushort number, T sig)
		{
			m_SigCacheSection.Execute(() => m_SigCache[number] = sig);
		}

		protected abstract T InstantiateSig(uint sigNumber);

		public IEnumerator<T> GetEnumerator()
		{
			return m_SigCacheSection.Execute(() => m_SigCache.Values.ToList()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
