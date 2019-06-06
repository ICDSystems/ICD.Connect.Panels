#if SIMPLSHARP
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro;
using ICD.Common.Utils;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters
{
	public sealed class SmartObjectCollectionAdapter : ISmartObjectCollection
	{
		/// <summary>
		/// Raised when a SmartObject is added to the collection.
		/// </summary>
		public event SmartObjectCallback OnSmartObjectAdded;

		/// <summary>
		/// Raised when a SmartObject is removed from the collection.
		/// </summary>
		public event SmartObjectCallback OnSmartObjectRemoved;

		private readonly Dictionary<uint, ISmartObject> m_SmartObjects;
		private readonly SafeCriticalSection m_SmartObjectsSection;

		private SmartObjectCollection m_Collection;

		/// <summary>
		/// Gets the SmartObject with the given id.
		/// </summary>
		/// <param name="id">The SmartObject id.</param>
		/// <returns>The SmartObject with the given id.</returns>
		public ISmartObject this[uint id] { get { return LazyLoadSmartObject(id); } }

		/// <summary>
		/// Constructor.
		/// </summary>
		public SmartObjectCollectionAdapter()
			: this(null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public SmartObjectCollectionAdapter(SmartObjectCollection collection)
		{
			m_SmartObjects = new Dictionary<uint, ISmartObject>();
			m_SmartObjectsSection = new SafeCriticalSection();

			SetSmartObjects(collection);
		}

		#region Methods

		/// <summary>
		/// Initializes smart objects
		/// </summary>
		/// <param name="collection"></param>
		public void SetSmartObjects(SmartObjectCollection collection)
		{
			m_SmartObjectsSection.Enter();

			try
			{
				if (collection == m_Collection)
					return;

				m_Collection = collection;

				Clear();
			}
			finally
			{
				m_SmartObjectsSection.Leave();
			}
		}

		/// <summary>
		/// Clears the cached smart objects.
		/// </summary>
		public void Clear()
		{
			m_SmartObjectsSection.Enter();

			try
			{
				foreach (KeyValuePair<uint, ISmartObject> item in m_SmartObjects.ToArray())
				{
					m_SmartObjects.Remove(item.Key);

					if (OnSmartObjectRemoved != null)
						OnSmartObjectRemoved(this, item.Value);
				}
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
				if (m_Collection == null)
					throw new InvalidOperationException("No internal collection");

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
			m_SmartObjectsSection.Enter();

			try
			{
				if (m_Collection == null)
					throw new InvalidOperationException("No internal collection");

				ISmartObject adapter;
				if (!m_SmartObjects.TryGetValue(key, out adapter))
				{
					adapter = new SmartObjectAdapter(m_Collection[key]);
					m_SmartObjects.Add(key, adapter);

					if (OnSmartObjectAdded != null)
						OnSmartObjectAdded(this, adapter);
				}

				return adapter;
			}
			finally
			{
				m_SmartObjectsSection.Leave();
			}
		}

		#endregion
	}
}

#endif
