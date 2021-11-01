using Crestron.SimplSharpPro;
using ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Abstracts;

namespace ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Hr310
{
#if !NETSTANDARD
	public sealed class Hr310Adapter : AbstractHrRemoteAdapter<global::Crestron.SimplSharpPro.Remotes.Hr310, Hr310AdapterSettings>
#else
	public sealed class Hr310Adapter : AbstractHrRemoteAdapter<Hr310AdapterSettings>
#endif
	{

#if !NETSTANDARD
		protected override global::Crestron.SimplSharpPro.Remotes.Hr310 InstantiateDevice(byte rfid, GatewayBase gateway)
		{
			return new global::Crestron.SimplSharpPro.Remotes.Hr310(rfid, gateway);
		}
#endif

	}
}