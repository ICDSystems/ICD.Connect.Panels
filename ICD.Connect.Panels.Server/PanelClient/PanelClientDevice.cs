#if NETFRAMEWORK
extern alias RealNewtonsoft;
using RealNewtonsoft.Newtonsoft.Json;
#else
using Newtonsoft.Json;
#endif
using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Common.Utils.Services.Logging;
using ICD.Common.Utils.Timers;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.Server.PanelServer;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Network.Ports.Tcp;
using ICD.Connect.Protocol.SerialBuffers;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.Settings;

namespace ICD.Connect.Panels.Server.PanelClient
{
	public sealed class PanelClientDevice : AbstractDevice<PanelClientDeviceSettings>
	{
		// How often to check the connection and reconnect if necessary.
		private const long CONNECTION_CHECK_MILLISECONDS = 30 * 1000;

		public const char DELIMITER = (char)0xFF;

		private readonly IcdTcpClient m_Client;
		private readonly ISerialBuffer m_Buffer;
		private readonly SafeTimer m_ConnectionTimer;

		private IPanelDevice m_Panel;

		public string Address { get { return m_Client.Address; } set { m_Client.Address = value; } }

		public ushort Port { get { return m_Client.Port; } set { m_Client.Port = value; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		public PanelClientDevice()
		{
			m_Client = new IcdTcpClient
			{
				Name = GetType().Name
			};
			m_Buffer = new DelimiterSerialBuffer(DELIMITER);

			m_ConnectionTimer = new SafeTimer(ConnectionTimerCallback, 0, CONNECTION_CHECK_MILLISECONDS);

			Subscribe(m_Buffer);
			Subscribe(m_Client);
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			m_ConnectionTimer.Dispose();

			base.DisposeFinal(disposing);

			SetPanel(null);

			Unsubscribe(m_Buffer);
			Unsubscribe(m_Client);

			m_Client.Dispose();
		}

		#region Methods

		/// <summary>
		/// Clears the assigned input sigs.
		/// </summary>
		public void Clear()
		{
			if (m_Panel != null)
				m_Panel.Clear();
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

			UpdateCachedOnlineStatus();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Called periodically to maintain connection to the device.
		/// </summary>
		private void ConnectionTimerCallback()
		{
			if (m_Client != null && !m_Client.IsConnected)
				Connect();
		}

		protected override bool GetIsOnlineStatus()
		{
			return m_Client != null && m_Client.IsOnline;
		}

		protected override void UpdateCachedOnlineStatus()
		{
			base.UpdateCachedOnlineStatus();

			UpdatePanelOfflineJoin();
		}

		/// <summary>
		/// Sets the panel offline join high when there is no connection to the server.
		/// </summary>
		private void UpdatePanelOfflineJoin()
		{
			if (m_Panel == null)
				return;

			m_Panel.SendInputDigital(CommonJoins.DIGITAL_OFFLINE_JOIN, !IsOnline);
		}

		#endregion

		#region Settings

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
					Logger.Log(eSeverity.Error, "No panel with id {0}", settings.Panel.Value);
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

		#endregion

		#region Panel Callbacks

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		/// <param name="panel"></param>
		private void Subscribe(IPanelDevice panel)
		{
			if (panel == null)
				return;

			panel.OnAnyOutput += PanelOnAnyOutput;
		}

		/// <summary>
		/// Unsubscribe from the panel events.
		/// </summary>
		/// <param name="panel"></param>
		private void Unsubscribe(IPanelDevice panel)
		{
			if (panel == null)
				return;

			panel.OnAnyOutput -= PanelOnAnyOutput;
		}

		/// <summary>
		/// Called when we receive an input sig from the panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void PanelOnAnyOutput(object sender, SigInfoEventArgs args)
		{
			string json = JsonUtils.SerializeMessage(args.Data, PanelServerDevice.SIG_MESSAGE);
			m_Client.Send(json + DELIMITER);
		}

		#endregion

		#region Client Callbacks

		/// <summary>
		/// Subscribe to the client events.
		/// </summary>
		/// <param name="client"></param>
		private void Subscribe(IcdTcpClient client)
		{
			client.OnIsOnlineStateChanged += ClientOnIsOnlineStateChanged;
			client.OnSerialDataReceived += ClientOnSerialDataReceived;
		}

		/// <summary>
		/// Unsubscribe from the client events.
		/// </summary>
		/// <param name="client"></param>
		private void Unsubscribe(IcdTcpClient client)
		{
			client.OnIsOnlineStateChanged -= ClientOnIsOnlineStateChanged;
			client.OnSerialDataReceived -= ClientOnSerialDataReceived;
		}

		/// <summary>
		/// Called when the client goes online/offline
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ClientOnIsOnlineStateChanged(object sender, DeviceBaseOnlineStateApiEventArgs args)
		{
			if (!args.Data)
				Clear();

			UpdateCachedOnlineStatus();
		}

		/// <summary>
		/// Called when we receive data from the server.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ClientOnSerialDataReceived(object sender, StringEventArgs args)
		{
			m_Buffer.Enqueue(args.Data);
		}

		#endregion

		#region Buffer Callbacks

		/// <summary>
		/// Subscribe to the buffer events.
		/// </summary>
		/// <param name="buffer"></param>
		private void Subscribe(ISerialBuffer buffer)
		{
			buffer.OnCompletedSerial += BufferOnCompletedSerial;
		}

		/// <summary>
		/// Unsubscribe from the buffer events.
		/// </summary>
		/// <param name="buffer"></param>
		private void Unsubscribe(ISerialBuffer buffer)
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

			try
			{
				JsonUtils.DeserializeMessage((r, m) => DeserializeJson(r, m), args.Data);
			}
			catch (JsonReaderException e)
			{
				Logger.Log(eSeverity.Error, "Failed to parse JSON - {0}{1}{2}", e.Message, IcdEnvironment.NewLine,
				           JsonUtils.Format(args.Data));
			}
		}

		/// <summary>
		/// Deserialize incoming json messages.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="messageName"></param>
		/// <returns></returns>
		private object DeserializeJson(JsonReader reader, string messageName)
		{
			switch (messageName)
			{
				// Received an input sig from the server
				case (PanelServerDevice.SIG_MESSAGE):
					SigInfo sigInfo = reader.ReadAsObject<SigInfo>();
					HandleInputSig(sigInfo.SmartObject == 0 ? (ISigInputOutput)m_Panel : m_Panel.SmartObjects[sigInfo.SmartObject],
					               sigInfo);
					return sigInfo;

				// Server informs us to use the given smart object
				case (PanelServerDevice.SMART_OBJECT_MESSAGE):
					uint smartObject = (uint)reader.GetValueAsInt();

// ReSharper disable once UnusedVariable
					ISmartObject so = m_Panel.SmartObjects[smartObject];

					return smartObject;

				default:
					return null;
			}
		}

		private static void HandleInputSig(ISigInputOutput device, SigInfo sig)
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

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			yield return new ConsoleCommand("Connect", "", () => Connect());
			yield return new ConsoleCommand("Disconnect", "", () => Disconnect());
			yield return new GenericConsoleCommand<string>("SetAddress", "SetAddress <hostname>", h => Address = h);
			yield return new GenericConsoleCommand<ushort>("SetPort", string.Format("SetPort <{0} - {1}>", 0, ushort.MaxValue), p => Port = p);
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
