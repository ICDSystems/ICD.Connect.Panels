﻿using ICD.Common.Properties;
using ICD.Connect.Panels.Controls.Sigs;
using ICD.Connect.Panels.SmartObjectCollections;

namespace ICD.Connect.Panels.Controls.Panels
{
	public interface IPanelControl : ISigControl
	{
		/// <summary>
		/// Collection containing the loaded SmartObjects of this device.
		/// </summary>
		[PublicAPI]
		ISmartObjectCollection SmartObjects { get; }
	}
}
