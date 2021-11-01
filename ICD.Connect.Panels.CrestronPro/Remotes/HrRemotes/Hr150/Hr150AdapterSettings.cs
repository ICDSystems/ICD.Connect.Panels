using ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Abstracts;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Hr150
{
	[KrangSettings("Hr150Adapter", typeof(Hr150Adapter))]
	public sealed class Hr150AdapterSettings : AbstractHrRemoteAdapterSettings
	{
	}
}