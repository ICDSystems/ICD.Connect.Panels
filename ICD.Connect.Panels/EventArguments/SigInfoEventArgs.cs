using ICD.Common.Utils.EventArguments;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.EventArguments
{
	public sealed class SigInfoEventArgs : GenericEventArgs<SigInfo>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="sigInfo"></param>
		public SigInfoEventArgs(SigInfo sigInfo)
			: base(sigInfo)
		{
		}
	}
}
