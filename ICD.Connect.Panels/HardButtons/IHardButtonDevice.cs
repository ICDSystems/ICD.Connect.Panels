using System;
using ICD.Connect.Devices;
using ICD.Connect.Panels.EventArguments;

namespace ICD.Connect.Panels.HardButtons
{
	
	
	public interface IHardButtonDevice : IDevice
	{
		event EventHandler<ButtonActionEventArgs> OnButtonAction;
	}
}