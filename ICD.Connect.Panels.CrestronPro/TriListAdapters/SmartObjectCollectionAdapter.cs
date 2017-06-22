using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro;
using ICD.Common.Utils;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.SimplSharp.Common.UiPro.TriListAdapters
{
	public sealed class SmartObjectCollectionAdapter : ISmartObjectCollection
	{
		private readonly SmartObjectCollection m_Collection;

		private readonly Dictionary<uint, ISmartObject> m_SmartObjects;
		private readonly SafeCriticalSection m_SmartObjectsSection;

		/// <summary>
		/// Get the object at the specified number.
		/// 
		/// </summary>
		/// <param name="paramKey">the key of the value to get.</param>
		/// <returns>
		/// Object stored at the key specified.
		/// </returns>
		/// <exception cref="T:System.IndexOutOfRangeException">Invalid Index Number specified.</exception>
		public ISmartObject this[uint paramKey]
		{
			get
			{
				m_SmartObjectsSection.Enter();

				try
				{
					return LazyLoadSmartObject(paramKey);
				}
				finally
				{
					m_SmartObjectsSection.Leave();
				}
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="collection"></param>
		public SmartObjectCollectionAdapter(SmartObjectCollection collection)
		{
			m_SmartObjects = new Dictionary<uint, ISmartObject>();
			m_SmartObjectsSection = new SafeCriticalSection();

			m_Collection = collection;
		}

		#region Methods

		/// <summary>
		/// Clears the cached smart objects.
		/// </summary>
		public void Clear()
		{
			m_SmartObjectsSection.Enter();

			try
			{
				m_SmartObjects.Clear();
			}
			finally
			{
				m_SmartObjectsSection.Leave();
			}
		}

		public IEnumerator<KeyValuePair<uint, ISmartObject>> GetEnumerator()
		{
			m_SmartObjectsSection.Enter();

			try
			{
				return m_Collection.Select(s => new KeyValuePair<uint, ISmartObject>(s.Key, LazyLoadSmartObject(s.Key)))
				                   .ToList()
				                   .GetEnumerator();
			}
			finally
			{
				m_SmartObjectsSection.Leave();
			}
		}

		#endregion

		#region Private Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the smart object wrapper for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private ISmartObject LazyLoadSmartObject(uint key)
		{
			if (!m_SmartObjects.ContainsKey(key))
				m_SmartObjects[key] = new SmartObjectAdapter(m_Collection[key]);
			return m_SmartObjects[key];
		}

		#endregion
	}
}
