using System;
using ICD.Connect.Routing;
using ICD.Connect.Routing.Controls;

namespace ICD.Connect.Panels.Crestron.Controls.Streaming.Dge
{
	public delegate bool RouteDelegate(RouteOperation info);

	public delegate bool ClearOutputDelegate();

	public delegate bool SetStreamDelegate(int input, Uri stream);

	public interface IDgeX00StreamSwitcherControl : IRouteSwitcherControl
	{
		RouteDelegate UiRoute { get; set; }

		ClearOutputDelegate UiClearOutput { get; set; }

		SetStreamDelegate UiSetStream { get; set; }

		void SetInputActive(int? input);
	}
}