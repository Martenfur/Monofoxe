
namespace Monofoxe.Tiled.MapStructure.Objects
{
	/// <summary>
	/// Used internally by Reader and Writer. 
	/// </summary>
	internal enum TiledObjectType : byte
	{
		None = 0,
		Tile = 1,
		Point = 2,
		Rectangle = 3,
		Ellipse = 4,
		Polygon = 5,
		Text = 6,
	}
}
