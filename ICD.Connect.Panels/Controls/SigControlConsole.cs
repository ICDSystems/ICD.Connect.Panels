using System;
using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Panels.Controls
{
	public static class SigControlConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(ISigControl instance)
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
		public static void BuildConsoleStatus(ISigControl instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			addRow("Last Output", instance.LastOutput);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(ISigControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			//yield return new ConsoleCommand("PrintSigs", "Prints sigs that have a value assigned", () => PrintSigs(instance));

			yield return new GenericConsoleCommand<uint, ushort>("SendInputAnalog", "SendInputAnalog <Number> <Value>", (n, v) => instance.SendInputAnalog(n, v));
			yield return new GenericConsoleCommand<uint, bool>("SendInputDigital", "SendInputDigital <Number> <Value>", (n, v) => instance.SendInputDigital(n, v));
			yield return new GenericConsoleCommand<uint, string>("SendInputSerial", "SendInputSerial <Number> <Value>", (n, v) => instance.SendInputSerial(n, v));
		}

		/*
		private static string PrintSigs(ISigControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			TableBuilder builder = new TableBuilder("Number", "Name", "Type", "Value");

			IEnumerable<ISig> sigs = BooleanInput.Cast<ISig>()
												 .Concat(UShortInput.Cast<ISig>())
												 .Concat(StringInput.Cast<ISig>()
																	.Where(s => !string.IsNullOrEmpty(s.GetStringValue())))
												 .Where(s => s.HasValue())
												 .OrderBy(s => s.Type)
												 .ThenBy(s => s.Number)
												 .ThenBy(s => s.Name);

			foreach (ISig item in sigs)
				builder.AddRow(item.Number, item.Name, item.Type, item.GetValue());

			return builder.ToString();
		}
		 */
	}
}
