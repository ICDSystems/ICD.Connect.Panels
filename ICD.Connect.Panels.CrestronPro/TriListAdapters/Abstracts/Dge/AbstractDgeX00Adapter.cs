using ICD.Connect.Panels.Crestron.Devices.Dge;
#if SIMPLSHARP
using System;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DM;
using Crestron.SimplSharpPro.UI;
using ICD.Connect.Misc.CrestronPro.Devices;
#endif

namespace ICD.Connect.Panels.CrestronPro.TriListAdapters.Abstracts.Dge
{
#if SIMPLSHARP
	public abstract class AbstractDgeX00Adapter<TPanel, TSettings> : AbstractTriListAdapter<TPanel, TSettings>, IDgeX00Adapter<TPanel>
		where TPanel : Dge100
#else
	public abstract class AbstractDgeX00Adapter<TSettings> : AbstractTriListAdapter<TSettings>, IDgeX00Adapter
#endif
		where TSettings : IDgeX00AdapterSettings, new()
	{
#if SIMPLSHARP
		public TPanel Dge { get { return Panel; } }
#endif

		#region Port Parent


#if SIMPLSHARP
		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public ComPort GetComPort(int address)
		{
			if (Dge == null)
				throw new InvalidOperationException("No device instantiated");

			if (address >= 1 && address <= Dge.NumberOfComPorts)
				return Dge.ComPorts[(uint)address];

			string message = string.Format("No {0} at address {1}", typeof(ComPort).Name, address);
			throw new InvalidOperationException(message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public IROutputPort GetIrOutputPort(int address)
		{
			if (Dge == null)
				throw new InvalidOperationException("No device instantiated");

			if (address >= 1 && address <= Dge.NumberOfIROutputPorts)
				return Dge.IROutputPorts[(uint)address];

			string message = string.Format("No {0} at address {1}", typeof(IROutputPort).Name, address);
			throw new InvalidOperationException(message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Relay GetRelayPort(int address)
		{
			string message = string.Format("{0} has no {1}", this, typeof(Relay).Name);
			throw new ArgumentOutOfRangeException("address", message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public Versiport GetIoPort(int address)
		{
			string message = string.Format("{0} has no {1}", this, typeof(Versiport).Name);
			throw new ArgumentOutOfRangeException("address", message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public DigitalInput GetDigitalInputPort(int address)
		{
			string message = string.Format("{0} has no {1}", this, typeof(DigitalInput).Name);
			throw new ArgumentOutOfRangeException("address", message);
		}

		/// <summary>
		/// Gets the port at the given address.
		/// </summary>
		/// <param name="io"></param>
		/// <param name="address"></param>
		/// <returns></returns>
		public virtual Cec GetCecPort(eInputOuptut io, int address)
		{

			if (Dge == null)
				throw new InvalidOperationException("No device instantiated");

			if (io == eInputOuptut.Output && address == 1)
				return Dge.HdmiOut.StreamCec;
			if (io == eInputOuptut.Input)
			{
				switch (address)
				{
					case 4:
						return Dge.HdmiIn.StreamCec;
				}
			}

			string message = string.Format("No CecPort at address {1}:{2} for device {0}", this, io, address);
			throw new InvalidOperationException(message);
		}

#endif

		#endregion
	}

	public abstract class AbstractDgeX00AdapterSettings : AbstractTriListAdapterSettings, IDgeX00AdapterSettings
	{
	}

#if SIMPLSHARP
	public interface IDgeX00Adapter<TPanel> : IDgeX00Adapter, ITriListAdapter, IPortParent
		where TPanel : Dge100
	{
		TPanel Dge { get; }
	}
#endif

	public interface IDgeX00AdapterSettings : ITriListAdapterSettings
	{
	}
}
