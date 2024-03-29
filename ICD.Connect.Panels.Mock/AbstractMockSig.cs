﻿using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Mock
{
	public abstract class AbstractMockSig : ISig
	{
		private string m_StringValue;
		private ushort m_UShortValue;
		private bool m_BoolValue;

		#region Properties

		/// <summary>
		/// Type of data this sig uses when communicating with the device.
		/// </summary>
		public abstract eSigType Type { get; }

		/// <summary>
		/// Number of this sig.
		/// </summary>
		[PublicAPI]
		public uint Number { get; set; }

		/// <summary>
		/// Get/Set the name of this Sig.
		/// </summary>
		[PublicAPI]
		public string Name { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Get the string representation of this Sig.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Sig is in an invalid state.</exception>
		[PublicAPI]
		public bool SetStringValue(string value)
		{
			if (value == m_StringValue)
				return false;

			m_StringValue = value;

			return true;
		}

		public virtual string GetStringValue()
		{
			return m_StringValue;
		}

		/// <summary>
		/// Get the UShort representation of this Sig.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Sig is in an invalid state.</exception>
		[PublicAPI]
		public bool SetUShortValue(ushort value)
		{
			if (value == m_UShortValue)
				return false;

			m_UShortValue = value;

			return true;
		}

		public virtual ushort GetUShortValue()
		{
			return m_UShortValue;
		}

		/// <summary>
		/// Get the bool representation of this Sig.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Sig is in an invalid state.</exception>
		[PublicAPI]
		public bool SetBoolValue(bool value)
		{
			if (value == m_BoolValue)
				return false;

			m_BoolValue = value;

			return true;
		}

		public virtual bool GetBoolValue()
		{
			return m_BoolValue;
		}

		/// <summary>
		/// Gets the string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			ReprBuilder builder = new ReprBuilder(this);

			if (Number != 0)
				builder.AppendProperty("Number", Number);

			if (!string.IsNullOrEmpty(Name))
				builder.AppendProperty("Name", Name);

			switch (Type)
			{
				case eSigType.Serial:
					builder.AppendProperty("String", GetStringValue());
					break;
				case eSigType.Analog:
					builder.AppendProperty("UShort", GetUShortValue());
					break;
				case eSigType.Digital:
					builder.AppendProperty("Bool", GetBoolValue());
					break;
			}

			return builder.ToString();
		}

		#endregion
	}
}
