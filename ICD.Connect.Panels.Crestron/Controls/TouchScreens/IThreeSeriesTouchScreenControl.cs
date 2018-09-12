using System;
using ICD.Connect.Misc.Keypads;
using ICD.Connect.Panels.Controls;

namespace ICD.Connect.Panels.Crestron.Controls.TouchScreens
{
	public interface IThreeSeriesTouchScreenControl : IPanelControl
	{
		/// <summary>
		/// Raised when a button state changes.
		/// </summary>
		event EventHandler<KeypadButtonPressedEventArgs> OnButtonStateChange;
	}
}
