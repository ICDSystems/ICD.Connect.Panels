using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels
{
	public static class SigInputOutputConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(ISigInputOutput instance)
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
		public static void BuildConsoleStatus(ISigInputOutput instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			addRow("Last Output", instance.LastOutput.HasValue ? instance.LastOutput.Value.ToLocalTime() : (DateTime?)null);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(ISigInputOutput instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield return new ConsoleCommand("PrintInputSigs", "Prints input sigs that have a value assigned", () => PrintInputSigs(instance));
			yield return new ConsoleCommand("PrintOutputSigs", "Prints output sigs that have a value assigned", () => PrintOutputSigs(instance));

			yield return new GenericConsoleCommand<uint, ushort>("SendInputAnalog", "SendInputAnalog <Number> <Value>", (n, v) => instance.SendInputAnalog(n, v));
			yield return new GenericConsoleCommand<uint, bool>("SendInputDigital", "SendInputDigital <Number> <Value>", (n, v) => instance.SendInputDigital(n, v));
			yield return new GenericConsoleCommand<uint, string>("SendInputSerial", "SendInputSerial <Number> <Value>", (n, v) => instance.SendInputSerial(n, v));
		}

		private static string PrintInputSigs(ISigInputOutput instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			TableBuilder builder = new TableBuilder("Number", "Name", "Type", "Value");

			IEnumerable<SigInfo> inputSigs =
				instance.GetInputSigInfo()
				        .Where(s => s.HasValue() && s.GetValue() as string != string.Empty)
				        .OrderBy(s => s.Type)
				        .ThenBy(s => s.Number)
				        .ThenBy(s => s.Name);

			foreach (SigInfo item in inputSigs)
				builder.AddRow(item.Number, item.Name, item.Type, item.GetValue());

			return builder.ToString();
		}

		private static string PrintOutputSigs(ISigInputOutput instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			TableBuilder builder = new TableBuilder("Number", "Name", "Type", "Value");

			IEnumerable<SigInfo> outputSigs =
				instance.GetOutputSigInfo()
				        .Where(s => s.HasValue() && s.GetValue() as string != string.Empty)
				        .OrderBy(s => s.Type)
				        .ThenBy(s => s.Number)
				        .ThenBy(s => s.Name);

			foreach (SigInfo item in outputSigs)
				builder.AddRow(item.Number, item.Name, item.Type, item.GetValue());

			return builder.ToString();
		}
	}
}
