using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Info;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Proxies.Controls;
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Panels.Controls.Backlight;
using ICD.Connect.Panels.EventArguments;

namespace ICD.Connect.Panels.Proxies.Controls.Backlight
{
	public sealed class ProxyBacklightDeviceControl : AbstractProxyDeviceControl, IBacklightDeviceControl
	{
		/// <summary>
		/// Raised when the backlight state changes.
		/// </summary>
		public event EventHandler<BacklightDeviceControlBacklightStateApiEventArgs> OnBacklightStateChanged;

		private eBacklightState m_BacklightState;

		/// <summary>
		/// Gets the backlight state of the device.
		/// </summary>
		public eBacklightState BacklightState
		{
			get { return m_BacklightState; }
			[UsedImplicitly]
			private set
			{
				if (value == m_BacklightState)
					return;

				m_BacklightState = value;

				OnBacklightStateChanged.Raise(this, new BacklightDeviceControlBacklightStateApiEventArgs(m_BacklightState));
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public ProxyBacklightDeviceControl(IProxyDevice parent, int id)
			: base(parent, id)
		{
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnBacklightStateChanged = null;

			base.DisposeFinal(disposing);
		}

		#region Methods

		/// <summary>
		/// Turns on the backlight.
		/// </summary>
		public void BacklightOn()
		{
			CallMethod(BacklightDeviceControlApi.METHOD_BACKLIGHT_ON);
		}

		/// <summary>
		/// Turns off the backlight.
		/// </summary>
		public void BacklightOff()
		{
			CallMethod(BacklightDeviceControlApi.METHOD_BACKLIGHT_OFF);
		}

		#endregion

		#region API

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
							 .SubscribeEvent(BacklightDeviceControlApi.EVENT_BACKLIGHT_STATE)
							 .GetProperty(BacklightDeviceControlApi.PROPERTY_BACKLIGHT_STATE)
							 .Complete();
		}

		/// <summary>
		/// Updates the proxy with a property result.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		protected override void ParseProperty(string name, ApiResult result)
		{
			base.ParseProperty(name, result);

			switch (name)
			{
				case BacklightDeviceControlApi.PROPERTY_BACKLIGHT_STATE:
					BacklightState = result.GetValue<eBacklightState>();
					break;

				default:
					base.ParseProperty(name, result);
					break;
			}
		}

		/// <summary>
		/// Updates the proxy with event feedback info.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		protected override void ParseEvent(string name, ApiResult result)
		{
			base.ParseEvent(name, result);

			switch (name)
			{
				case BacklightDeviceControlApi.EVENT_BACKLIGHT_STATE:
					BacklightState = result.GetValue<eBacklightState>();
					break;

				default:
					base.ParseEvent(name, result);
					break;
			}
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			foreach (IConsoleNodeBase node in GetBaseConsoleNodes())
				yield return node;

			foreach (IConsoleNodeBase node in BacklightDeviceControlConsole.GetConsoleNodes(this))
				yield return node;
		}

		/// <summary>
		/// Wrokaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleNodeBase> GetBaseConsoleNodes()
		{
			return base.GetConsoleNodes();
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			BacklightDeviceControlConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in BacklightDeviceControlConsole.GetConsoleCommands(this))
				yield return command;
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
