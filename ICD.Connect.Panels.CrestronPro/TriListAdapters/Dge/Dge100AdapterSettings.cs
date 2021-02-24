﻿using ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge;
using ICD.Connect.Settings.Attributes;

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Dge
{
	[KrangSettings("Dge100", typeof(Dge100Adapter))]
	public sealed class Dge100AdapterSettings : AbstractDgeX00AdapterSettings
	{
	}
}
