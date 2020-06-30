#if SIMPLSHARP
using System.Collections.Generic;
using Crestron.SimplSharpPro.DM.Endpoints;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge;
using ICD.Connect.Routing;
using ICD.Connect.Routing.Connections;

namespace ICD.Connect.Panels.CrestronPro.Controls.Streaming.Dge
{
	public sealed class DmDge200CStreamSwitcherControl : AbstractDgeStreamSwitcherControl<DmDge200CAdapter, DmDge200C>
	{
		private const int DM_INPUT_ADDRESS = 5;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public DmDge200CStreamSwitcherControl(DmDge200CAdapter parent, int id) 
			: base(parent, id)
		{
		}

		#region Methods

		private bool GetDmInputSyncState()
		{
			return Dge != null && Dge.DmIn.SyncDetectedFeedback.BoolValue;
		}

		#region Abstract Overrides

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

		protected override void SetInitialInputDetectState()
		{
			base.SetInitialInputDetectState();

			Cache.SetSourceDetectedState(DM_INPUT_ADDRESS, eConnectionType.Audio | eConnectionType.Video, GetDmInputSyncState());
		}

		#endregion

		#endregion

		#region DGE Callbacks

		protected override void Subscribe(DmDge200C dge)
		{
			base.Subscribe(dge);

			if (dge == null)
				return;

			dge.DmIn.InputStreamChange += DmInOnInputStreamChange;
		}

		protected override void Unsubscribe(DmDge200C dge)
		{
			base.Subscribe(dge);

			if (dge == null)
				return;

			dge.DmIn.InputStreamChange -= DmInOnInputStreamChange;
		}

		private void DmInOnInputStreamChange(EndpointInputStream inputStream, EndpointInputStreamEventArgs args)
		{
			switch (args.EventId)
			{
				case EndpointInputStreamEventIds.SyncDetectedFeedbackEventId:
					Cache.SetSourceDetectedState(DM_INPUT_ADDRESS,eConnectionType.Audio | eConnectionType.Video, GetDmInputSyncState());
					break;
			}
		}

		#endregion
	}
}
#endif