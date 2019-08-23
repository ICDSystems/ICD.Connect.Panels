using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Json;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices;
using ICD.Connect.Panels.Controls;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.Server.PanelClient;
using ICD.Connect.Panels.SmartObjectCollections;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.EventArguments;
using ICD.Connect.Protocol.Network.Ports.Tcp;
using ICD.Connect.Protocol.SerialBuffers;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.Settings;
using Newtonsoft.Json;

namespace ICD.Connect.Panels.Server
{
	/// <summary>
	/// The AbstractPanelServerDevice wraps a TCPServer to emulate how existing Crestron panels work.
	/// </summary>
	public abstract class AbstractPanelServerDevice<TSettings> : AbstractDeviceBase<TSettings>, IPanelServerDevice
		where TSettings : IPanelServerDeviceSettings, new()
	{
		public const string SIG_MESSAGE = "S";
		public const string SMART_OBJECT_MESSAGE = "So";
		private const string HEARTBEAT_MESSAGE = "H";
		private const string TIME_MESSAGE = "T";

		/// <summary>
		/// Raised when the user interacts with the panel.
		/// </summary>
		public event EventHandler<SigInfoEventArgs> OnAnyOutput;

		private readonly IcdTcpServer m_Server;
		private readonly TcpServerBufferManager m_Buffers;
		private readonly SigCache m_Cache;
		private readonly SafeCriticalSection m_CacheSection;
		private readonly SigCallbackManager m_SigCallbacks;
		private readonly PanelServerSmartObjectCollection m_SmartObjects;

		#region Properties

		/// <summary>
		/// Gets/sets the port for the server.
		/// </summary>
		[PublicAPI]
		public ushort Port
		{
			get { return m_Server.Port; }
			set
			{
				if (value == m_Server.Port)
					return;

				m_Server.Port = value;

				if (m_Server.Enabled)
					m_Server.Restart();

				UpdateCachedOnlineStatus();
			}
		}

		/// <summary>
		/// Gets the time that the user last interacted with the panel.
		/// </summary>
		public DateTime? LastOutput { get; private set; }

		/// <summary>
		/// Collection containing the loaded SmartObjects of this device.
		/// </summary>
		public ISmartObjectCollection SmartObjects { get { return m_SmartObjects; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractPanelServerDevice()
		{
			m_Cache = new SigCache();
			m_CacheSection = new SafeCriticalSection();
			m_SigCallbacks = new SigCallbackManager();

			m_SmartObjects = new PanelServerSmartObjectCollection(this);
			Subscribe(m_SmartObjects);

			m_Server = new IcdTcpServer
			{
				Name = GetType().Name,
				MaxNumberOfClients = IcdTcpServer.MAX_NUMBER_OF_CLIENTS_SUPPORTED
			};
			Subscribe(m_Server);

			m_Buffers = new TcpServerBufferManager(() => new DelimiterSerialBuffer(PanelClientDevice.DELIMITER));
			m_Buffers.SetServer(m_Server);
			Subscribe(m_Buffers);

			Controls.Add(new PanelControl(this, 1));
		}

		#region Methods

		/// <summary>
		/// Caches and sends the sig.
		/// </summary>
		/// <param name="sigInfo"></param>
		public void SendSig(SigInfo sigInfo)
		{
			// No change
			if (!m_CacheSection.Execute(() => m_Cache.Add(sigInfo)))
				return;

			if (m_Server.NumberOfClients == 0)
				return;

			string serial = JsonUtils.SerializeMessage(sigInfo, SIG_MESSAGE);
			SendData(serial);
		}

		/// <summary>
		/// Clears the assigned input sig values.
		/// </summary>
		public void Clear()
		{
			m_CacheSection.Execute(() => m_Cache.Clear());
		}

		/// <summary>
		/// Registers the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public void RegisterOutputSigChangeCallback(uint number, eSigType type,
		                                            Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			m_SigCallbacks.RegisterSigChangeCallback(number, type, callback);
		}

		/// <summary>
		/// Unregisters the callback for output sig change events.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="callback"></param>
		public void UnregisterOutputSigChangeCallback(uint number, eSigType type,
		                                              Action<SigCallbackManager, SigInfoEventArgs> callback)
		{
			m_SigCallbacks.UnregisterSigChangeCallback(number, type, callback);
		}

		/// <summary>
		/// Sends the serial data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="text"></param>
		public void SendInputSerial(uint number, string text)
		{
			SendSig(new SigInfo(number, 0, text));
		}

		/// <summary>
		/// Sends the analog data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public void SendInputAnalog(uint number, ushort value)
		{
			SendSig(new SigInfo(number, 0, value));
		}

		/// <summary>
		/// Sends the digital data to the panel.
		/// </summary>
		/// <param name="number"></param>
		/// <param name="value"></param>
		public void SendInputDigital(uint number, bool value)
		{
			SendSig(new SigInfo(number, 0, value));
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			Port = 0;
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.Port = Port;
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			Port = settings.Port;
			m_Server.Start();
			UpdateCachedOnlineStatus();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			base.DisposeFinal(disposing);

			Unsubscribe(m_SmartObjects);

			Unsubscribe(m_Buffers);
			m_Buffers.Dispose();

			Unsubscribe(m_Server);
			m_Server.Dispose();
		}

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			return m_Server != null && m_Server.Listening;
		}

		/// <summary>
		/// Sends the given data to the given connected client.
		/// </summary>
		/// <param name="clientId"></param>
		/// <param name="data"></param>
		private void SendData(uint clientId, string data)
		{
			m_Server.Send(clientId, data + PanelClientDevice.DELIMITER);
		}

		/// <summary>
		/// Sends the given data to all connected clients.
		/// </summary>
		/// <param name="data"></param>
		private void SendData(string data)
		{
			m_Server.Send(data + PanelClientDevice.DELIMITER);
		}

		#endregion

		#region Server Callbacks

		/// <summary>
		/// Subscribe to the server events.
		/// </summary>
		/// <param name="server"></param>
		private void Subscribe(IcdTcpServer server)
		{
			server.OnSocketStateChange += ServerOnSocketStateChange;
		}

		/// <summary>
		/// Unsubscribe from the server events.
		/// </summary>
		/// <param name="server"></param>
		private void Unsubscribe(IcdTcpServer server)
		{
			server.OnSocketStateChange -= ServerOnSocketStateChange;
		}

		/// <summary>
		/// Called when a client state changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ServerOnSocketStateChange(object sender, SocketStateEventArgs args)
		{
			if (args.SocketState != SocketStateEventArgs.eSocketStatus.SocketStatusConnected)
				return;

			InitializeClient(args.ClientId);
		}

		/// <summary>
		/// Updates the client to match the current state of the panel.
		/// </summary>
		/// <param name="clientId"></param>
		private void InitializeClient(uint clientId)
		{
			if (!m_Server.ClientConnected(clientId))
				return;

			// Inform the client of the processor time in ISO8601
			string localTimeMessage = JsonUtils.SerializeMessage(IcdEnvironment.GetLocalTime().ToString("O"), TIME_MESSAGE);
			SendData(clientId, localTimeMessage);

			List<string> messages = new List<string>();

			m_CacheSection.Enter();

			try
			{
				// Send all of the cached sigs to the new client.
				foreach (SigInfo sig in m_Cache)
				{
					string sigMessage = JsonUtils.SerializeMessage(sig, SIG_MESSAGE);
					messages.Add(sigMessage);
				}

				// Inform the client of used smartobjects
				foreach (KeyValuePair<uint, ISmartObject> kvp in m_SmartObjects)
				{
					string soMessage = JsonUtils.SerializeMessage(kvp.Key, SMART_OBJECT_MESSAGE);
					messages.Add(soMessage);
				}
			}
			finally
			{
				m_CacheSection.Leave();
			}

			foreach (string message in messages)
				SendData(clientId, message);
		}

		#endregion

		#region Buffer Manager Callbacks

		/// <summary>
		/// Subscribe to the buffer manager events.
		/// </summary>
		/// <param name="bufferManager"></param>
		private void Subscribe(TcpServerBufferManager bufferManager)
		{
			bufferManager.OnClientCompletedSerial += BuffersOnClientCompletedSerial;
		}

		/// <summary>
		/// Unsubscribe from the buffer manager events.
		/// </summary>
		/// <param name="bufferManager"></param>
		private void Unsubscribe(TcpServerBufferManager bufferManager)
		{
			bufferManager.OnClientCompletedSerial -= BuffersOnClientCompletedSerial;
		}

		/// <summary>
		/// Called when we get a complete serial from a client.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="clientId"></param>
		/// <param name="data"></param>
		private void BuffersOnClientCompletedSerial(TcpServerBufferManager sender, uint clientId, string data)
		{
			try
			{
				object parsedData = JsonUtils.DeserializeMessage<object>(DeserializeJson, data);
				if (parsedData is string && parsedData.Equals(HEARTBEAT_MESSAGE))
					m_Server.Send(clientId,
					              JsonUtils.SerializeMessage("pong", HEARTBEAT_MESSAGE) +
					              PanelClientDevice.DELIMITER);
			}
			catch (JsonReaderException e)
			{
				Log(eSeverity.Error, "Failed to parse JSON - {0}{1}{2}", e.Message, IcdEnvironment.NewLine, JsonUtils.Format(data));
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
				case SIG_MESSAGE:
					SigInfo sigInfo = reader.ReadAsObject<SigInfo>();

					if (sigInfo.SmartObject == 0)
						m_SigCallbacks.RaiseSigChangeCallback(sigInfo);
					else
						m_SmartObjects[sigInfo.SmartObject].HandleOutputSig(sigInfo);

					LastOutput = IcdEnvironment.GetLocalTime();


					OnAnyOutput.Raise(this, new SigInfoEventArgs(sigInfo));

					return sigInfo;
				case HEARTBEAT_MESSAGE:
					return HEARTBEAT_MESSAGE;
				default:
					return null;
			}
		}

		#endregion

		#region SmartObject Callbacks

		private void Subscribe(ISmartObjectCollection smartObjects)
		{
			smartObjects.OnSmartObjectAdded += SmartObjectsOnSmartObjectAdded;
		}

		private void Unsubscribe(ISmartObjectCollection smartObjects)
		{
			smartObjects.OnSmartObjectAdded -= SmartObjectsOnSmartObjectAdded;
		}

		/// <summary>
		/// Subscribes to SmartObject Touch Events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="smartObject"></param>
		private void SmartObjectsOnSmartObjectAdded(object sender, ISmartObject smartObject)
		{
			// When a SmartObject is added to the collection we need to inform the clients that we are
			// using the SmartObject so they can handle output events.
			SendSmartObjectId(smartObject.SmartObjectId);
		}

		/// <summary>
		/// Sends a message to the clients informing of a smart object being added.
		/// </summary>
		/// <param name="smartObjectId"></param>
		private void SendSmartObjectId(uint smartObjectId)
		{
			if (m_Server.NumberOfClients == 0)
				return;

			string serial = JsonUtils.SerializeMessage(smartObjectId, SMART_OBJECT_MESSAGE);
			SendData(serial);
		}

		#endregion

		#region Console

		public override IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			foreach (IConsoleNodeBase node in GetBaseConsoleNodes())
				yield return node;

			yield return m_Server;
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleNodeBase> GetBaseConsoleNodes()
		{
			return base.GetConsoleNodes();
		}

		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("Port", Port);
		}

		#endregion
	}
}
