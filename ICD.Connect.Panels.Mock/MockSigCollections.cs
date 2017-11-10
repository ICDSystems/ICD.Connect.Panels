using ICD.Common.Properties;
using ICD.Connect.Panels.SigCollections;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.Panels.Mock
{
	[PublicAPI]
	public sealed class MockStringInputCollection :
		AbstractMockSigCollection<IStringInputSig>, IDeviceStringInputCollection
	{
		protected override IStringInputSig InstantiateSig(uint sigNumber)
		{
			return new MockStringInputSig { Number = sigNumber };
		}
	}

	[PublicAPI]
	public sealed class MockStringOutputCollection :
		AbstractMockSigCollection<IStringOutputSig>, IDeviceStringOutputCollection
	{
		protected override IStringOutputSig InstantiateSig(uint sigNumber)
		{
			return new MockStringOutputSig { Number = sigNumber };
		}
	}

	[PublicAPI]
	public sealed class MockUShortInputCollection :
		AbstractMockSigCollection<IUShortInputSig>, IDeviceUShortInputCollection
	{
		protected override IUShortInputSig InstantiateSig(uint sigNumber)
		{
			return new MockUShortInputSig { Number = sigNumber };
		}
	}

	[PublicAPI]
	public sealed class MockUShortOutputCollection :
		AbstractMockSigCollection<IUShortOutputSig>, IDeviceUShortOutputCollection
	{
		protected override IUShortOutputSig InstantiateSig(uint sigNumber)
		{
			return new MockUShortOutputSig { Number = sigNumber };
		}
	}

	[PublicAPI]
	public sealed class MockBooleanInputCollection :
		AbstractMockSigCollection<IBoolInputSig>, IDeviceBooleanInputCollection
	{
		protected override IBoolInputSig InstantiateSig(uint sigNumber)
		{
			return new MockBoolInputSig { Number = sigNumber };
		}
	}

	[PublicAPI]
	public sealed class MockBooleanOutputCollection :
		AbstractMockSigCollection<IBoolOutputSig>, IDeviceBooleanOutputCollection
	{
		protected override IBoolOutputSig InstantiateSig(uint sigNumber)
		{
			return new MockBoolOutputSig { Number = sigNumber };
		}
	}
}
