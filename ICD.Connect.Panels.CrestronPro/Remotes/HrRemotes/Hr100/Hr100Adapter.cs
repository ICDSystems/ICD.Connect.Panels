using System;
using Crestron.SimplSharpPro;
using ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Abstracts;

namespace ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Hr100
{
#if !NETSTANDARD
	public sealed class Hr100Adapter : AbstractHrRemoteAdapter<global::Crestron.SimplSharpPro.Remotes.Hr100,Hr100AdapterSettings>
#else
	public sealed class Hr100Adapter : AbstractHrRemoteAdapter<Hr100AdapterSettings>
#endif
	{

#if !NETSTARNDARD
		protected override global::Crestron.SimplSharpPro.Remotes.Hr100 InstantiateDevice(byte rfid, GatewayBase gateway)
		{
			return new global::Crestron.SimplSharpPro.Remotes.Hr100(rfid, gateway);
		}
#endif

	}
}