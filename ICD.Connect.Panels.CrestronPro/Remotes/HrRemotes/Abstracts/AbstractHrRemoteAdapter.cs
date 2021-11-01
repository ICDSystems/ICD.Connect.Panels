using System;
using System.Collections.Generic;
#if !NETSTANDARD
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.Remotes;
#endif
using ICD.Common.Utils.Extensions;
using ICD.Connect.Misc.CrestronPro.InfinetEx;
using ICD.Connect.Panels.Crestron.Devices.Hr;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.HardButtons;

namespace ICD.Connect.Panels.CrestronPro.Remotes.HrRemotes.Abstracts
{
#if !NETSTANDARD
	public abstract class AbstractHrRemoteAdapter<TDevice, TSettings> : AbstractInfinetExAdapter<TDevice,TSettings>, IHrRemoteAdapter
		where TSettings : IHrRemoteAdapterSettings, new()
		where TDevice : Hr1x0WirelessRemoteBase
#else
	public abstract class AbstractHrRemoteAdapter<TSettings> : AbstractInfinetExAdapter<TSettings>, IHrRemoteAdapter
		where TSettings : IHrRemoteAdapterSettings, new()
#endif
	{

#if !NETSTANDARD
		protected override void Subscribe(TDevice device)
		{
			base.Subscribe(device);

			if (device == null)
				return;

			device.ButtonStateChange += DeviceOnButtonStateChange;
		}

		protected override void Unsubscribe(TDevice device)
		{
			base.Subscribe(device);

			if (device == null)
				return;

			device.ButtonStateChange -= DeviceOnButtonStateChange;
		}

		private void DeviceOnButtonStateChange(GenericBase device, ButtonEventArgs args)
		{
			eHardButton hardButton;
			if (TryGetButtonFromButtonNumber((int)args.Button.Number, out hardButton))
			{
				OnButtonAction.Raise(this, new ButtonActionEventArgs(hardButton, CrestronRemoteUtils.ButtonStateToButtonAction(args.NewButtonState)));
			}
		}
#endif

		public event EventHandler<ButtonActionEventArgs> OnButtonAction;

		protected virtual bool TryGetButtonFromButtonNumber(int buttonNumber, out eHardButton hardButton)
		{
			return CrestronRemoteUtils.TryGetHrHardButtonFromButtonNumber(buttonNumber, out hardButton);
		}
	}
}