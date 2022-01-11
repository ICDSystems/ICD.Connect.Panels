using System;
using ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Abstracts;
#if !NETSTANDARD
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Hr100
{
#if !NETSTANDARD
	public sealed class Hr100Adapter : AbstractHrRemoteAdapter<global::Crestron.SimplSharpPro.Remotes.Hr100,Hr100AdapterSettings>
#else
	public sealed class Hr100Adapter : AbstractHrRemoteAdapter<Hr100AdapterSettings>
#endif
	{
#if !NETSTANDARD
		protected override global::Crestron.SimplSharpPro.Remotes.Hr100 InstantiateDevice(byte rfid, GatewayBase gateway)
		{
			return new global::Crestron.SimplSharpPro.Remotes.Hr100(rfid, gateway);
		}
#endif
	}
}