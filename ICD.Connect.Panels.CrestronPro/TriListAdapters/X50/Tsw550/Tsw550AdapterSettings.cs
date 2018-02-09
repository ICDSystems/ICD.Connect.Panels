﻿using System;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.X50.Tsw550
{
	/// <summary>
	/// Settings for the Tsw550Adapter panel device.
	/// </summary>
	[KrangSettings(FACTORY_NAME)]
	public sealed class Tsw550AdapterSettings : AbstractTswFt5ButtonSystemAdapterSettings
	{
		private const string FACTORY_NAME = "Tsw550";

		/// <summary>
		/// Gets the originator factory name.
		/// </summary>
		public override string FactoryName { get { return FACTORY_NAME; } }

		/// <summary>
		/// Gets the type of the originator for this settings instance.
		/// </summary>
		public override Type OriginatorType { get { return typeof(Tsw550Adapter); } }
	}
}
