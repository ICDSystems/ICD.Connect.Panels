using System.Collections.Generic;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Server
{
	public interface IPanelServerDevice : IPanelDevice
	{
		/// <summary>
		/// Gets all of the cached input sigs.
		/// </summary>
		/// <returns></returns>
		IEnumerable<SigInfo> GetCachedInputSigs();

		/// <summary>
		/// Caches and sends the sig.
		/// </summary>
		/// <param name="sigInfo"></param>
		void SendSig(SigInfo sigInfo);
	}
}
