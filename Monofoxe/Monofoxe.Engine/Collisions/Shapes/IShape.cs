using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Collisions.Shapes
{
	public interface IShape
	{
		ShapeType Type { get; }

		AABB GetBoundingBox();
	}
}
