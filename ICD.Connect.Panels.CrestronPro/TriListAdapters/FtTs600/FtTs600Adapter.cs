using ICD.Connect.Conferencing.Controls.Dialing;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.FtTs600
{
#if SIMPLSHARP
	public sealed class FtTs600Adapter :
		AbstractFt5ButtonAdapter<global::Crestron.SimplSharpPro.UI.FtTs600, FtTs600AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override global::Crestron.SimplSharpPro.UI.FtTs600 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new global::Crestron.SimplSharpPro.UI.FtTs600(ipid, controlSystem);
		}

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of dialing control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IDialingDeviceControl InstantiateDialingControl(int id)
		{
			return new TswFt5ButtonDialingControl(this, id);
		}

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of backlight control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IPowerDeviceControl InstantiateBacklightControl(int id)
		{
			return new TswFt5ButtonBacklightControl(this, id);
		}
	}
#else
    public sealed class FtTs600Adapter : AbstractTriListAdapter<FtTs600AdapterSettings>
    {
    }
#endif
}
