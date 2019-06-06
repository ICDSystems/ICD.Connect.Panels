using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.Panels.Mock
{
	public sealed class MockSmartObjectCollection : ISmartObjectCollection
	{
		/// <summary>
		/// Raised when a SmartObject is added to the collection.
		/// </summary>
		public event SmartObjectCallback OnSmartObjectAdded;

		/// <summary>
		/// Raised when a SmartObject is removed from the collection.
		/// </summary>
		public event SmartObjectCallback OnSmartObjectRemoved;

	    private readonly SafeCriticalSection m_SmartObjectsSection;
        private readonly Dictionary<uint, ISmartObject> m_SmartObjects;

		/// <summary>
		/// Gets the SmartObject with the given id.
		/// </summary>
		/// <param name="id">The SmartObject id.</param>
		/// <returns>The SmartObject with the given id.</returns>
		public ISmartObject this[uint id]
		{
			get
			{
				m_SmartObjectsSection.Enter();

				try
				{
					ISmartObject smartObject;
					if (!m_SmartObjects.TryGetValue(id, out smartObject))
					{
						smartObject = new MockSmartObject();
						m_SmartObjects.Add(id, smartObject);

						if (OnSmartObjectAdded != null)
							OnSmartObjectAdded(this, smartObject);
					}

					return m_SmartObjects[id];
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
		public MockSmartObjectCollection()
		{
			m_SmartObjects = new Dictionary<uint, ISmartObject>();
		    m_SmartObjectsSection = new SafeCriticalSection();
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
		    return m_SmartObjectsSection.Execute(() => m_SmartObjects.ToList().GetEnumerator());
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
