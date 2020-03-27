using System.Collections.Generic;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge;
using ICD.Connect.Routing;

namespace ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge
{
	public sealed class DmDge200CStreamSwitcherControl : AbstractDgeStreamSwitcherControl<DmDge200CAdapter>
	{
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
			return input == DM_INPUT_ADDRESS || base.ContainsInput(input);
		}

		/// <summary>
		/// Returns the inputs.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<ConnectorInfo> GetInputs()
		{
			foreach (ConnectorInfo input in GetBaseInputs())
				yield return input;

			yield return GetInput(DM_INPUT_ADDRESS);
		}

		private IEnumerable<ConnectorInfo> GetBaseInputs()
		{
			return base.GetInputs();
		}
	}
}