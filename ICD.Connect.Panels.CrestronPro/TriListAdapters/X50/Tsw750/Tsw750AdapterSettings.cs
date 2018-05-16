using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw750
{
	/// <summary>
	/// Settings for the Tsw750Adapter panel device.
	/// </summary>
	[KrangSettings("Tsw750", typeof(Tsw750Adapter))]
	public sealed class Tsw750AdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings
	{
	}
}
