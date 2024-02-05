using Monofoxe.Engine.Collisions.Shapes;

namespace Monofoxe.Engine.Collisions.Colliders
{
	/// <summary>
	/// Represents a perfect circle.
	/// </summary>
	public class CircleCollider : Collider
	{
		private Circle _circle => (Circle)_shapes[0];

		public float Radius
		{
			get => _circle.Radius;
			set => _circle.Radius = value;
		}
	}
}
