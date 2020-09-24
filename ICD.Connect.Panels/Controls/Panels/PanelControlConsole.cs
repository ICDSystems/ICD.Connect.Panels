using System;
using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Panels.Controls.Panels
{
	public static class PanelControlConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(IPanelControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield break;
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="addRow"></param>
		public static void BuildConsoleStatus(IPanelControl instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(IPanelControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield return new GenericConsoleCommand<uint, uint, ushort>("SendSoInputAnalog", "SendSoInputAnalog <SMARTOBJECT> <Number> <Value>", (id, n, v) => instance.SmartObjects[id].SendInputAnalog(n, v));
			yield return new GenericConsoleCommand<uint, uint, bool>("SendSoInputDigital", "SendSoInputDigital <SMARTOBJECT> <Number> <Value>", (id, n, v) => instance.SmartObjects[id].SendInputDigital(n, v));
			yield return new GenericConsoleCommand<uint, uint, string>("SendSoInputSerial", "SendSoInputSerial <SMARTOBJECT> <Number> <Value>", (id, n, v) => instance.SmartObjects[id].SendInputSerial(n, v));
		}
	}
}
