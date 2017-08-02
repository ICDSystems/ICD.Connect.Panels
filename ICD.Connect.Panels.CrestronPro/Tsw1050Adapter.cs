﻿using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Panels.CrestronPro.Settings;
using ICD.Connect.Panels.CrestronPro.TriListAdapters;

namespace ICD.Connect.Panels.CrestronPro
{
	public sealed class Tsw1050Adapter : AbstractTriListAdapter<Tsw1050, Tsw1050AdapterSettings>
	{
		/// <summary>
		/// Creates an instance of the wrapped trilist.
		/// </summary>
		/// <param name="ipid"></param>
		/// <param name="controlSystem"></param>
		/// <returns></returns>
		protected override Tsw1050 InstantiateTriList(byte ipid, CrestronControlSystem controlSystem)
		{
			return new Tsw1050(ipid, controlSystem);
		}
	}
}
