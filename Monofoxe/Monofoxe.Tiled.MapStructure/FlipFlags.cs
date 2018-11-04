
namespace Monofoxe.Tiled.MapStructure
{
	/// <summary>
	/// Tile flip flags are stored in the tile value itself as 3 highest bits.
	/// 100 - Horizontal flip.
	/// 010 - Vertical flip.
	/// 001 - Diagonal flip.
	/// </summary>
	internal enum FlipFlags : uint
	{
		FlipDiag = 536870912,
		FlipVer = 1073741824,
		FlipHor = 2147483648,
		All = 3758096384,
	}
}
