using System;

namespace Monofoxe.Tiled.MapStructure
{
	[Flags]
	internal enum FlipFlags : uint
	{
		None = 0,
		FlipDiag = 536870912,
		FlipVer = 1073741824,
		FlipHor = 2147483648,
		All = 3758096384
	}
}
