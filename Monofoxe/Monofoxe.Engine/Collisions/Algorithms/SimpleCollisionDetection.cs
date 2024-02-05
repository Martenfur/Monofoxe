using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace Monofoxe.Engine.Collisions.Algorithms
{
	internal static class SimpleCollisionDetection
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool CheckCircleCircle(Vector2 center1, float r1, Vector2 center2, float r2)
		{
			return (center1 - center2).LengthSquared() <= (r1 + r2) * (r1 + r2);
		}
	}
}
