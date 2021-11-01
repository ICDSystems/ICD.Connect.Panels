#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif
using ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Abstracts;

namespace ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Hr150
{
#if !NETSTANDARD
	public sealed class Hr150Adapter : AbstractHrRemoteAdapter<global::Crestron.SimplSharpPro.Remotes.Hr150,Hr150AdapterSettings>
#else
	public sealed class Hr150Adapter : AbstractHrRemoteAdapter<Hr150AdapterSettings>
#endif
	{
#if !NETSTANDARD
		protected override global::Crestron.SimplSharpPro.Remotes.Hr150 InstantiateDevice(byte rfid, GatewayBase gateway)
		{
			return new global::Crestron.SimplSharpPro.Remotes.Hr150(rfid, gateway);
		}
#endif
	}
}