using ICD.Connect.Devices;

namespace ICD.Connect.Panels
{
	public abstract class AbstractSigDevice<TSettings> : AbstractSigDeviceBase<TSettings>, IDevice
		where TSettings : IDeviceSettings, new()
	{
	}
}
