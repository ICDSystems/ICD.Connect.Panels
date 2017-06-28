using ICD.Common.Properties;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Panels.Extensions
{
	public static class DeviceFactoryExtensions
	{
		/// <summary>
		/// Lazy-loads the panel with the given id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[PublicAPI]
		public static IPanelDevice GetPanelById(this IDeviceFactory factory, int id)
		{
			return factory.GetOriginatorById<IPanelDevice>(id);
		}
	}
}