using ICD.Common.Utils.Xml;
using ICD.Connect.Protocol.Network.Settings;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Ts1542
{
	public abstract class AbstractTs1542AdapterSettings : AbstractTriListAdapterSettings, ITs1542AdapterSettings
	{
		#region Fields

		private readonly SecureNetworkProperties m_NetworkProperties;

		#endregion

		#region Network

		/// <summary>
		/// Gets/sets the configurable network address.
		/// </summary>
		public string NetworkAddress
		{
			get { return m_NetworkProperties.NetworkAddress; }
			set { m_NetworkProperties.NetworkAddress = value; }
		}

		/// <summary>
		/// Gets/sets the configurable network port.
		/// </summary>
		public ushort? NetworkPort
		{
			get { return m_NetworkProperties.NetworkPort; }
			set { m_NetworkProperties.NetworkPort = value; }
		}

		/// <summary>
		/// Gets/sets the configurable network username.
		/// </summary>
		public string NetworkUsername
		{
			get { return m_NetworkProperties.NetworkUsername; }
			set { m_NetworkProperties.NetworkUsername = value; }
		}

		/// <summary>
		/// Gets/sets the configurable network password.
		/// </summary>
		public string NetworkPassword
		{
			get { return m_NetworkProperties.NetworkPassword; }
			set { m_NetworkProperties.NetworkPassword = value; }
		}

		/// <summary>
		/// Clears the configured values.
		/// </summary>
		void INetworkProperties.ClearNetworkProperties()
		{
			m_NetworkProperties.ClearNetworkProperties();
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractTs1542AdapterSettings()
		{
			m_NetworkProperties = new SecureNetworkProperties();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			m_NetworkProperties.WriteElements(writer);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			m_NetworkProperties.ParseXml(xml);
		}

		#endregion
	}
}