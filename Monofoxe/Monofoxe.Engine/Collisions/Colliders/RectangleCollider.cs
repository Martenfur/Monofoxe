using Microsoft.Xna.Framework;
using Monofoxe.Engine.Collisions.Shapes;

namespace Monofoxe.Engine.Collisions.Colliders
{
	/// <summary>
	/// Creates a rectangle with its center inthe middle, consisting of four vertices that form the following shape:
	/// 0---1
	/// | \ |
	/// 3---2
	/// NOTE: While it is possible to directly manipulate vertices of this collider, it is NOT recommended to do unless you really know what you are doing. Use base Collider instead.
	/// </summary>
	public class RectangleCollider : Collider
	{
		private Polygon _rectangle => (Polygon)_shapes[0];
		
		/// <summary>
		/// Rectangle size measured in meters.
		/// </summary>
		public Vector2 Size
		{
			get
			{
				return new Vector2(
					_rectangle.RelativeVertices[1].X - _rectangle.RelativeVertices[0].X,
					_rectangle.RelativeVertices[3].Y - _rectangle.RelativeVertices[0].Y
				);
			}

			set
			{
				var halfSize = value / 2;
				
				_rectangle.RelativeVertices[0].X = -halfSize.X;
				_rectangle.RelativeVertices[0].Y = -halfSize.Y;

				_rectangle.RelativeVertices[1].X =  halfSize.X;
				_rectangle.RelativeVertices[1].Y = -halfSize.Y;

				_rectangle.RelativeVertices[2].X =  halfSize.X;
				_rectangle.RelativeVertices[2].Y =  halfSize.Y;

				_rectangle.RelativeVertices[3].X = -halfSize.X;
				_rectangle.RelativeVertices[3].Y =  halfSize.Y;
			}
		}
	}
}
