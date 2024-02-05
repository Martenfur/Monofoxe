using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.CustomCollections;

namespace Monofoxe.Engine.Collisions.Shapes
{
	/// <summary>
	/// Defines a perfect circle with a positive radius.
	/// </summary>
	public class Circle : IShape, IPoolable
	{
		public ShapeType Type => ShapeType.Circle;


		/// <summary>
		/// Radius of the circle, measured in meters.
		/// </summary>
		public float Radius;

		/// <summary>
		/// Absolute position of the shape, measured in meters.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// NOTE: It is recommended to use ShapePool to get new instances of this class.
		/// </summary>
		public Circle() { }

		public AABB GetBoundingBox()
		{
			return new AABB(Position - new Vector2(Radius, Radius), Position + new Vector2(Radius, Radius));
		}

		/// <inheritdoc/>
		public bool InPool { get; set; }

		/// <inheritdoc/>
		public void OnTakenFromPool() {}

		/// <inheritdoc/>
		public void OnReturnedToPool()
		{
			Position = Vector2.Zero;
			Radius = 0;		
		}
	}
}
