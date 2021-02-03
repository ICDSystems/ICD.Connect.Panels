using ICD.Connect.Conferencing.Controls.Dialing;
using ICD.Connect.Panels.Controls.Backlight;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Backlight;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Controls.Voip;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.TswFt5Buttons;
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
		protected override ITraditionalConferenceDeviceControl InstantiateDialingControl(int id)
		{
			return new TswFt5ButtonTraditionalConferenceControl(this, id);
		}

		/// <summary>
		/// Called from constructor.
		/// Override to control the type of backlight control to instantiate.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected override IBacklightDeviceControl InstantiateBacklightControl(int id)
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
