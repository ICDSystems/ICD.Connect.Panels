using System;
using ICD.Connect.Devices;
using ICD.Connect.Misc.Keypads;
using ICD.Connect.Panels.Controls;
using ICD.Connect.Panels.Crestron.Controls.TouchScreens;

namespace ICD.Connect.Panels.CrestronPro.Controls.TouchScreens
{
	public abstract class AbstractThreeSeriesTouchScreenControl<TParent> : AbstractPanelControl<TParent>, IThreeSeriesTouchScreenControl
		where TParent : IDeviceBase
	{
		/// <summary>
		/// Raised when a button state changes.
		/// </summary>
		public abstract event EventHandler<KeypadButtonPressedEventArgs> OnButtonStateChange;

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
