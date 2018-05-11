using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw550
{
	/// <summary>
	/// Settings for the Tsw550Adapter panel device.
	/// </summary>
	[KrangSettings("Tsw550", typeof(Tsw550Adapter))]
	public sealed class Tsw550AdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings
	{
	}
}
