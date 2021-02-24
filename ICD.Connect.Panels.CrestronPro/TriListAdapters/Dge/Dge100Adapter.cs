using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge;
#if SIMPLSHARP
using System;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings;
using ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
#if SIMPLSHARP
	public sealed class Dge100Adapter : AbstractDgeX00Adapter<Dge100, Dge100AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Dge100 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Dge100(ipid, controlSystem);
		}

		/// <summary>
		/// Override to add controls to the device.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		/// <param name="addControl"></param>
		protected override void AddControls(Dge100AdapterSettings settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
			base.AddControls(settings, factory, addControl);

			addControl(new Dge100StreamSwitcherControl(this, 0));
		}
	}
#else
	public sealed class Dge100Adapter : AbstractDgeX00Adapter<Dge100AdapterSettings>
	{
	}
#endif
}
