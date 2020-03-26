using System.Collections.Generic;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge;
using ICD.Connect.Routing;

namespace ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge
{
	public sealed class DmDge200CStreamSwitcherControl : AbstractDgeStreamSwitcherControl<DmDge200CAdapter>
	{
		private const int STREAM_INPUT_ADDRESS = 1;
		private const int HDMI_INPUT_ADDRESS = 2;
		private const int DM_INPUT_ADDRESS = 3;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public DmDge200CStreamSwitcherControl(DmDge200CAdapter parent, int id) 
			: base(parent, id)
		{
		}

		/// <summary>
		/// Returns true if the destination contains an input at the given address.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override bool ContainsInput(int input)
		{
			return input == STREAM_INPUT_ADDRESS || input == HDMI_INPUT_ADDRESS || input == DM_INPUT_ADDRESS;
		}

		/// <summary>
		/// Returns the inputs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetInputs()
		{
			yield return GetInput(STREAM_INPUT_ADDRESS);
			yield return GetInput(HDMI_INPUT_ADDRESS);
			yield return GetInput(DM_INPUT_ADDRESS);
		}
	}
}