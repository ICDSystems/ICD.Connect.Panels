using ICD.Connect.Devices;
using ICD.Connect.Panels.SmartObjectCollections;

namespace ICD.Connect.Panels.Controls
{
	public abstract class AbstractPanelControl<TParent> : AbstractSigControl<TParent>, IPanelControl
		where TParent : IDeviceBase
	{
		/// <summary>
		/// Collection containing the loaded SmartObjects of this device.
		/// </summary>
		public abstract ISmartObjectCollection SmartObjects { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractPanelControl(TParent parent, int id)
			: base(parent, id)
		{
		}
	}
}
