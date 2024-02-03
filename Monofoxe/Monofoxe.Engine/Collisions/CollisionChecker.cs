using Monofoxe.Engine.Collisions.Shapes;

namespace Monofoxe.Engine.Collisions
{
	public static class CollisionChecker
	{
		public static bool CheckCollision(Collider a, Collider b)
		{
			for (var i = 0; i < a.ShapesCount; i += 1)
			{
				for (var k = 0; k < b.ShapesCount; k += 1)
				{
					if (ShapeCollisionChecker.CheckCollision(a.GetShape(i), b.GetShape(k)))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool CheckCollision(Collider a, IShape b)
		{
			for (var i = 0; i < a.ShapesCount; i += 1)
			{
				if (ShapeCollisionChecker.CheckCollision(a.GetShape(i), b))
				{
					return true;
				}
			}
			return false;
		}
	}
}
