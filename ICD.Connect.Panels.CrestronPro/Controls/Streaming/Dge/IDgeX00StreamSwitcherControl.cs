using System;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Routing;

namespace ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge
{
	public delegate bool RouteDelegate(RouteOperation info);

	public delegate bool ClearOutputDelegate();

	public delegate bool SetStreamDelegate(int input, Uri stream);

	public interface IDgeX00StreamSwitcherControl : IDeviceControl
	{
		RouteDelegate UiRoute { get; set; }

		ClearOutputDelegate UiClearOutput { get; set; }

		SetStreamDelegate UiSetStream { get; set; }

		void SetInputActive(int? input);
	}
}