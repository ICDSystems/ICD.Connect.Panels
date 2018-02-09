using System;
using ICD.Connect.Settings.Attributes;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Ts1542
{
#if SIMPLSHARP
	public sealed class Ts1542Adapter : AbstractTs1542Adapter<Crestron.SimplSharpPro.UI.Ts1542, Ts1542AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.Ts1542 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.Ts1542(ipid, controlSystem);
		}
	}
#else
	public sealed class Ts1542Adapter : AbstractTs1542Adapter<Ts1542AdapterSettings>
	{
	}
#endif

	[KrangSettings(FACTORY_NAME)]
	public sealed class Ts1542AdapterSettings : AbstractTs1542AdapterSettings
	{
		private const string FACTORY_NAME = "Ts1542";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Ts1542Adapter); } }
	}
}
