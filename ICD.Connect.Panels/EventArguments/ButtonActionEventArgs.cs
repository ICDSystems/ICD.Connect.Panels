using ICD.Common.Utils.EventArguments;
using ICD.Connect.Panels.HardButtons;

namespace ICD.Connect.Panels.EventArguments
{
	public sealed class ButtonActionEventArgs : GenericEventArgs<ButtonActionEventData>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public ButtonActionEventArgs(ButtonActionEventData data)
			: base(data)
		{
		}

		public ButtonActionEventArgs(eHardButton button, eButtonAction action)
			: base(new ButtonActionEventData(button, action))
		{
		}
	}

	public sealed class ButtonActionEventData
	{
		private readonly eHardButton m_Button;
		private readonly eButtonAction m_Action;
		public eHardButton Button { get { return m_Button; } }
		public eButtonAction Action {get { return m_Action; }}

		public ButtonActionEventData(eHardButton button, eButtonAction action)
		{
			m_Button = button;
			m_Action = action;
		}
	}
}