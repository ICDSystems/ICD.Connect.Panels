using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.Server.Osd
{
	[KrangSettings("OsdPanel", typeof(OsdPanelDevice))]
	public sealed class OsdPanelDeviceSettings : AbstractPanelServerDeviceSettings
	{
	}
}
