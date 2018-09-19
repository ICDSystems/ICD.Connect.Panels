using System.Collections;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.Panels.Mock
{
	public sealed class MockSmartObjectCollection : ISmartObjectCollection
	{
		public event AddSmartObject OnSmartObjectSubscribe;
		public event RemoveSmartObject OnSmartObjectUnsubscribe;

	    private readonly SafeCriticalSection m_SmartObjectsSection;
        private readonly Dictionary<uint, ISmartObject> m_SmartObjects;

		/// <summary>
		/// Constructor.
		/// </summary>
		public MockSmartObjectCollection()
		{
			m_SmartObjects = new Dictionary<uint, ISmartObject>();
		    m_SmartObjectsSection = new SafeCriticalSection();
        }

		public IEnumerator<KeyValuePair<uint, ISmartObject>> GetEnumerator()
		{
		    return m_SmartObjects.GetEnumerator();
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

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
		            if (!m_SmartObjects.ContainsKey(paramKey))
		                m_SmartObjects[paramKey] = new MockSmartObject();
		            
		            if (OnSmartObjectSubscribe != null)
		                OnSmartObjectSubscribe(this, m_SmartObjects[paramKey]);

		            return m_SmartObjects[paramKey];
                }
		        finally
		        {
		            m_SmartObjectsSection.Leave();
		        }
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
		        foreach (KeyValuePair<uint, ISmartObject> item in m_SmartObjects)
		        {
		            if (OnSmartObjectUnsubscribe != null)
		                OnSmartObjectUnsubscribe(this, item.Value);
		        }
		        m_SmartObjects.Clear();
		    }
		    finally
		    {
		        m_SmartObjectsSection.Leave();
		    }
        }
    }
}
