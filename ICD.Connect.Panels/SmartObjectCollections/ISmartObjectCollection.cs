using System.Collections.Generic;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.Panels.SmartObjectCollections
{
	public interface ISmartObjectCollection : IEnumerable<KeyValuePair<uint, ISmartObject>>
	{
		/// <summary>
		/// Get the object at the specified number.
		/// 
		/// </summary>
		/// <param name="paramKey">the key of the value to get.</param>
		/// <returns>
		/// Object stored at the key specified.
		/// </returns>
		/// <exception cref="T:System.IndexOutOfRangeException">Invalid Index Number specified.</exception>
		ISmartObject this[uint paramKey] { get; }

		/// <summary>
		/// Clears the cached smart objects.
		/// </summary>
		void Clear();
	}
}
