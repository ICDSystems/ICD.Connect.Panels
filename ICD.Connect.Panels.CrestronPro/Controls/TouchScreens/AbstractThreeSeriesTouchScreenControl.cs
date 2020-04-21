using ICD.Connect.Devices;
using ICD.Connect.Panels.Controls;
using ICD.Connect.Panels.Crestron.Controls.TouchScreens;

namespace ICD.Connect.Panels.CrestronPro.Controls.TouchScreens
{
	public abstract class AbstractThreeSeriesTouchScreenControl<TParent> : AbstractPanelControl<TParent>, IThreeSeriesTouchScreenControl
		where TParent : IDevice
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractThreeSeriesTouchScreenControl(TParent parent, int id)
			: base(parent, id)
		{
		}
	}
}
