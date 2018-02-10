using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Server
{
	public interface IPanelServerDevice : IPanelDevice
	{
		/// <summary>
		/// Caches and sends the sig.
		/// </summary>
		/// <param name="sigInfo"></param>
		void SendSig(SigInfo sigInfo);
	}
}
