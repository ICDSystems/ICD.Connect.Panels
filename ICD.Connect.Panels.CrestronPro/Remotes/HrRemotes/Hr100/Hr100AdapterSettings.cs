using ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Abstracts;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Hr100
{
	[KrangSettings("Hr100Adapter", typeof(Hr100Adapter))]
	public sealed class Hr100AdapterSettings : AbstractHrRemoteAdapterSettings
	{
	}
}