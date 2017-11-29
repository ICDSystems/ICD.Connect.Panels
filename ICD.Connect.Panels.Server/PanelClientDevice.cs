using System;
using System.Net;
using ICD.Common.Services.Logging;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Network.Tcp;
using ICD.Connect.Protocol.SerialBuffers;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Panels.Server
{
	public sealed class PanelClientDevice : AbstractDevice<PanelClientDeviceSettings>
	{
		private readonly AsyncTcpClient m_Client;
		private readonly JsonSerialBuffer m_Buffer;

		private IPanelDevice m_Panel;

		public string Address { get { return m_Client.Address; } set { m_Client.Address = value; } }

		public ushort Port { get { return m_Client.Port; } set { m_Client.Port = value; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		public PanelClientDevice()
		{
			m_Client = new AsyncTcpClient();
			m_Buffer = new JsonSerialBuffer();

			Subscribe(m_Buffer);
			Subscribe(m_Client);
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(PanelClientDeviceSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			Port = settings.Port;
			Address = settings.Address;

			IPanelDevice panel = null;
			if (settings.Panel != null)
			{
				panel = factory.GetOriginatorById(settings.Panel.Value) as IPanelDevice;
				if (panel == null)
				{
					Logger.AddEntry(eSeverity.Error, "No panel with id {0}", settings.Panel.Value);
				}
			}

			SetPanel(panel);
		}

		protected override void CopySettingsFinal(PanelClientDeviceSettings settings)
		{
			base.CopySettingsFinal(settings);
			settings.Panel = m_Panel != null ? m_Panel.Id : (int?)null;
			settings.Port = Port;
			settings.Address = Address;
		}

		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();
			SetPanel(null);
			Port = 0;
			Address = null;
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			base.DisposeFinal(disposing);

			SetPanel(null);

			Unsubscribe(m_Buffer);
			Unsubscribe(m_Client);

			m_Client.Dispose();
		}

		/// <summary>
		/// Connect to the panel server.
		/// </summary>
		public void Connect()
		{
			m_Client.Connect();
		}

		/// <summary>
		/// Disconnect from the panel server.
		/// </summary>
		public void Disconnect()
		{
			m_Client.Disconnect();
		}

		/// <summary>
		/// Sets the wrapped panel.
		/// </summary>
		/// <param name="panel"></param>
		public void SetPanel(IPanelDevice panel)
		{
			if (panel == m_Panel)
				return;

			Disconnect();

			Unsubscribe(m_Panel);
			m_Panel = panel;
			Subscribe(m_Panel);

			if (m_Panel != null)
				Connect();
		}

		#region Panel Callbacks

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		/// <param name="panel"></param>
		private void Subscribe(IPanelDevice panel)
		{
			if (panel == null)
				return;

			panel.OnAnyOutput += PanelOnOnAnyOutput;
		}

		/// <summary>
		/// Unsubscribe from the panel events.
		/// </summary>
		/// <param name="panel"></param>
		private void Unsubscribe(IPanelDevice panel)
		{
			if (panel == null)
				return;

			panel.OnAnyOutput -= PanelOnOnAnyOutput;
		}

		/// <summary>
		/// Called when we receive an input sig from the panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void PanelOnOnAnyOutput(object sender, SigInfoEventArgs args)
		{
			m_Client.Send(args.Data.Serialize());
		}

		#endregion

		protected override bool GetIsOnlineStatus()
		{
			return m_Client != null && m_Client.IsOnline;
		}

		#region Client Callbacks

		/// <summary>
		/// Subscribe to the client events.
		/// </summary>
		/// <param name="client"></param>
		private void Subscribe(AsyncTcpClient client)
		{
			client.OnIsOnlineStateChanged += ClientOnOnIsOnlineStateChanged;
			client.OnSerialDataReceived += ClientOnOnSerialDataReceived;
		}

		/// <summary>
		/// Unsubscribe from the client events.
		/// </summary>
		/// <param name="client"></param>
		private void Unsubscribe(AsyncTcpClient client)
		{
			client.OnIsOnlineStateChanged -= ClientOnOnIsOnlineStateChanged;
			client.OnSerialDataReceived -= ClientOnOnSerialDataReceived;
		}

		/// <summary>
		/// Called when the client goes online/offline
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ClientOnOnIsOnlineStateChanged(object sender, BoolEventArgs args)
		{
			UpdateCachedOnlineStatus();
		}

		/// <summary>
		/// Called when we receive data from the server.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ClientOnOnSerialDataReceived(object sender, StringEventArgs args)
		{
			m_Buffer.Enqueue(args.Data);
		}

		#endregion

		#region Buffer Callbacks

		/// <summary>
		/// Subscribe to the buffer events.
		/// </summary>
		/// <param name="buffer"></param>
		private void Subscribe(JsonSerialBuffer buffer)
		{
			buffer.OnCompletedSerial += BufferOnCompletedSerial;
		}

		/// <summary>
		/// Unsubscribe from the buffer events.
		/// </summary>
		/// <param name="buffer"></param>
		private void Unsubscribe(JsonSerialBuffer buffer)
		{
			buffer.OnCompletedSerial -= BufferOnCompletedSerial;
		}

		/// <summary>
		/// Called when we receive a complete message from the server.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void BufferOnCompletedSerial(object sender, StringEventArgs args)
		{
			if (m_Panel == null)
				return;

			SigInfo sig = SigInfo.Deserialize(args.Data);

			HandleInputSig(sig.SmartObject == 0 ? (ISigInputOutput)m_Panel : m_Panel.SmartObjects[sig.SmartObject], sig);
		}

		private void HandleInputSig(ISigInputOutput device, SigInfo sig)
		{
			switch (sig.Type)
			{
				case eSigType.Digital:
					device.SendInputDigital(sig.Number, sig.GetBoolValue());
					break;
				case eSigType.Analog:
					device.SendInputAnalog(sig.Number, sig.GetUShortValue());
					break;
				case eSigType.Serial:
					device.SendInputSerial(sig.Number, sig.GetStringValue());
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion

		#region Console

		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("Address", Address);
			addRow("Port", Port);
			addRow("Panel", m_Panel);
		}

		#endregion
	}
}