namespace ICD.Connect.Panels.Crestron.Controls.TouchScreens
{
	public interface IMPC3x201TouchScreenControl : IMPC3x101TouchScreenControl
	{
	}

	public static class MPC3x201TouchScreenButtons
	{
		public const uint BUTTON_MUTE = 1;
		public const uint BUTTON_VOLUME_DOWN = 2;
		public const uint BUTTON_VOLUME_UP = 3;
		public const uint BUTTON_POWER = 4;

		public const uint BUTTON_ACTION_1 = 5;
		public const uint BUTTON_ACTION_2 = 6;
		public const uint BUTTON_ACTION_3 = 7;
		public const uint BUTTON_ACTION_4 = 8;
		public const uint BUTTON_ACTION_5 = 9;
		public const uint BUTTON_ACTION_6 = 10;
	}
}