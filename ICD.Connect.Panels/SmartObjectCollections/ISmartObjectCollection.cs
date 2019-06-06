using System.Collections.Generic;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.Panels.SmartObjectCollections
{
	public delegate void SmartObjectCallback(object sender, ISmartObject smartObject);

	public interface ISmartObjectCollection : IEnumerable<KeyValuePair<uint, ISmartObject>>
	{
		/// <summary>
		/// Raised when a SmartObject is added to the collection.
		/// </summary>
		event SmartObjectCallback OnSmartObjectAdded;

		/// <summary>
		/// Raised when a SmartObject is removed from the collection.
		/// </summary>
		event SmartObjectCallback OnSmartObjectRemoved;

		/// <summary>
		/// Gets the SmartObject with the given id.
		/// </summary>
		/// <param name="id">The SmartObject id.</param>
		/// <returns>The SmartObject with the given id.</returns>
		ISmartObject this[uint id] { get; }

		/// <summary>
		/// Clears the cached smart objects.
		/// </summary>
		void Clear();
	}
}
