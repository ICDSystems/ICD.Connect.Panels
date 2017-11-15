using ICD.Connect.Settings;

namespace ICD.Connect.Panels
{
	/// <summary>
	/// Base class for panel settings.
	/// </summary>
	public abstract class AbstractPanelDeviceSettings : AbstractDeviceBaseSettings, IPanelDeviceSettings
	{
		public const string PANEL_ELEMENT = "Panel";

		/// <summary>
		/// Gets the xml element.
		/// </summary>
		protected override string Element { get { return PANEL_ELEMENT; } }
	}
}
