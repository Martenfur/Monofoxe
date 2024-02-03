using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.CustomCollections;
using System.Runtime.InteropServices;

namespace Monofoxe.Engine.Collisions.Shapes
{
	public class Circle : IShape, IPoolable
	{
		public ShapeType Type => ShapeType.Circle;


		public float Radius;
		public Vector2 Position;

		public AABB GetBoundingBox()
		{
			var a =  new AABB(Position - new Vector2(Radius, Radius), Position + new Vector2(Radius, Radius));
			return a;
		}

		public bool InPool { get; set; }

		public void OnTakenFromPool() {}

		public void OnReturnedToPool() 
		{
			Position = Vector2.Zero;
			Radius = 0;
		}
	}
}
