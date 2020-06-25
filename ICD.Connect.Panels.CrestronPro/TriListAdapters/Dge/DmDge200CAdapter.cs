using System;
using ICD.Connect.Misc.CrestronPro.Devices;
using ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
using Crestron.SimplSharpPro.DM;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public sealed class DmDge200CAdapter : AbstractDgeX00Adapter<DmDge200C, DmDge200CAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override DmDge200C InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new DmDge200C(ipid, controlSystem);
		}

		public DmDge200CAdapter()
		{
			Controls.Add(new DmDge200CStreamSwitcherControl(this, 0));
		}

		#region IPortParent

		public override Cec GetCecPort(eInputOuptut io, int address)
		{

			if (Dge == null)
				throw new InvalidOperationException("No device instantiated");

			if (io == eInputOuptut.Input && address == 5)
			{
					return Dge.DmIn.StreamCec;
			}

			return base.GetCecPort(io, address);
		}

		#endregion
	}
#else
	public sealed class DmDge200CAdapter : AbstractDgeX00Adapter<DmDge200CAdapterSettings>
	{
	}
#endif
}
