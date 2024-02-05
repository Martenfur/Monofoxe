using System;

namespace Monofoxe.Engine.Cameras
{
	[Flags]
	public enum RenderMask
	{
		None = 0x00000000,
		Default = 0x00000001,
		Mask1 = 0x00000001,
		Mask2 = 0x00000002,
		Mask3 = 0x00000004,
		Mask4 = 0x00000008,
		Mask5 = 0x00000010,
		Mask6 = 0x00000020,
		Mask7 = 0x00000040,
		Mask8 = 0x00000080,
		Mask9 = 0x00000100,
		Mask10 = 0x00000200,
		Mask11 = 0x00000400,
		Mask12 = 0x00000800,
		Mask13 = 0x00001000,
		Mask14 = 0x00002000,
		Mask15 = 0x00004000,
		Mask16 = 0x00008000,
		Mask17 = 0x00010000,
		Mask18 = 0x00020000,
		Mask19 = 0x00040000,
		Mask20 = 0x00080000,
		Mask21 = 0x00100000,
		Mask22 = 0x00200000,
		Mask23 = 0x00400000,
		Mask24 = 0x00800000,
		Mask25 = 0x01000000,
		Mask26 = 0x02000000,
		Mask27 = 0x04000000,
		Mask28 = 0x08000000,
		Mask29 = 0x10000000,
		Mask30 = 0x20000000,
		Mask31 = 0x40000000,
		All = int.MaxValue,
	}
}
