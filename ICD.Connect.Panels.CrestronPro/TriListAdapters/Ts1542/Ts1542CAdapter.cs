using ICD.Connect.Settings.Attributes;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Ts1542
{
#if SIMPLSHARP
	public sealed class Ts1542CAdapter : AbstractTs1542Adapter<global::Crestron.SimplSharpPro.UI.Ts1542C, Ts1542CAdapterSettings>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public Ts1542CAdapter()
		{
			Controls.Add(new Ts1542CRouteDestinationControl(this, 0));
		}

		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.Ts1542C InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.Ts1542C(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts1542CAdapter : AbstractTs1542Adapter<Ts1542CAdapterSettings>
	{
	}
#endif

	[KrangSettings("Ts1542C", typeof(Ts1542CAdapter))]
	public sealed class Ts1542CAdapterSettings : AbstractTs1542AdapterSettings
	{
	}
}
