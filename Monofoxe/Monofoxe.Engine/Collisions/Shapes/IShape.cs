using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Collisions.Shapes
{
	/// <summary>
	/// Defines a concave shape for the collision system to use.
	/// </summary>
	public interface IShape
	{
		ShapeType Type { get; }

		AABB GetBoundingBox();
	}
}
