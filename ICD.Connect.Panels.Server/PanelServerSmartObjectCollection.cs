using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.Panels.Server
{
	public sealed class PanelServerSmartObjectCollection : ISmartObjectCollection
	{
		/// <summary>
		/// Raised when a SmartObject is added to the collection.
		/// </summary>
		public event SmartObjectCallback OnSmartObjectAdded;

		/// <summary>
		/// Raised when a SmartObject is removed from the collection.
		/// </summary>
		public event SmartObjectCallback OnSmartObjectRemoved;

		private readonly IPanelServerDevice m_Device;

		private readonly Dictionary<uint, PanelServerSmartObject> m_SmartObjects;
		private readonly SafeCriticalSection m_SmartObjectsSection;

		/// <summary>
		/// Gets the SmartObject with the given id.
		/// </summary>
		/// <param name="id">The SmartObject id.</param>
		/// <returns>The SmartObject with the given id.</returns>
		ISmartObject ISmartObjectCollection.this[uint id] { get { return this[id]; } }

		/// <summary>
		/// Gets the SmartObject with the given id.
		/// </summary>
		/// <param name="id">The SmartObject id.</param>
		/// <returns>The SmartObject with the given id.</returns>
		public PanelServerSmartObject this[uint id] { get { return LazyLoadSmartObject((ushort)id); } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="device"></param>
		public PanelServerSmartObjectCollection(IPanelServerDevice device)
		{
			m_SmartObjects = new Dictionary<uint, PanelServerSmartObject>();
			m_SmartObjectsSection = new SafeCriticalSection();

			m_Device = device;
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
				foreach (KeyValuePair<uint, PanelServerSmartObject> item in m_SmartObjects.ToArray())
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
				return m_SmartObjects.Select(kvp => new KeyValuePair<uint, ISmartObject>(kvp.Key, kvp.Value))
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
		private PanelServerSmartObject LazyLoadSmartObject(ushort key)
		{
			m_SmartObjectsSection.Enter();

			try
			{
				PanelServerSmartObject smartObject;
				if (!m_SmartObjects.TryGetValue(key, out smartObject))
				{
					smartObject = new PanelServerSmartObject(m_Device, key);
					m_SmartObjects[key] = smartObject;

					if (OnSmartObjectAdded != null)
						OnSmartObjectAdded(this, smartObject);
				}

				return smartObject;
			}
			finally
			{
				m_SmartObjectsSection.Leave();
			}
		}

		#endregion
	}
}
