using Monofoxe.Engine.Collisions.Shapes;

namespace Monofoxe.Engine.Collisions.Colliders
{
	/// <summary>
	/// Represents a simple line that consists of two vertices that form the following shape with its center at vertex 0:
	/// 0---1
	/// 
	/// NOTE: While it is possible to directly manipulate vertices of this collider, it is NOT recommended to do unless you really know what you are doing. Use base Collider instead.
	/// </summary>
	public class LineCollider : Collider
	{
		private Polygon _line => (Polygon)_shapes[0];
		
		/// <summary>
		/// Length of the line, measured in meters.
		/// </summary>
		public float Length
		{
			get
			{
				return (_line.RelativeVertices[1] - _line.RelativeVertices[0]).Length();
			}

			set
			{
				var e = (_line.RelativeVertices[1] - _line.RelativeVertices[0]).SafeNormalize();
				_line.RelativeVertices[1] = _line.RelativeVertices[0] + e * value;
			}
		}
	}
}
