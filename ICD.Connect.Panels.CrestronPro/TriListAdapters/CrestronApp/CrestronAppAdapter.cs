using ICD.Connect.API.Nodes;
using ICD.Connect.Settings.Core;
#if SIMPLSHARP
using Crestron.SimplSharpPro;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.CrestronApp
{
#if SIMPLSHARP
	public sealed class CrestronAppAdapter :
		AbstractTriListAdapter<Crestron.SimplSharpPro.UI.CrestronApp, CrestronAppAdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Crestron.SimplSharpPro.UI.CrestronApp InstantiateTriList(byte ipid,
		                                                                            CrestronControlSystem controlSystem)
		{
			return new Crestron.SimplSharpPro.UI.CrestronApp(ipid, controlSystem);
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(CrestronAppAdapterSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.ProjectName = Panel == null ? null : Panel.ParameterProjectName.Value;
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(CrestronAppAdapterSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			if (Panel != null)
				Panel.ParameterProjectName.Value = settings.ProjectName;
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("ProjectName", Panel == null ? null : Panel.ParameterProjectName.Value);
		}
	}
#else
	public sealed class CrestronAppAdapter : AbstractTriListAdapter<CrestronAppAdapterSettings>
	{
	}
#endif
}
