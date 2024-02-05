using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.CustomCollections;
using System;
using System.Runtime.CompilerServices;

namespace Monofoxe.Engine.Collisions.Shapes
{
	/// <summary>
	/// Defines a convex polygon with clockwise winding (assuming the up vector is -1;0). Minimum possible number of vertices is 2.
	/// </summary>
	public class Polygon : IShape, IPoolable
	{
		public ShapeType Type => ShapeType.Polygon;

		/// <summary>
		/// Absolute position of the shape, measured in meters.
		/// NOTE: You should always call UpdateTransform() after changing this field.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Polygon rotation around the local 0;0 point, measured in degrees.
		/// NOTE: You should always call UpdateTransform() after changing this field.
		/// </summary>
		public float Rotation = 0;
		public float _cachedRotation = 0;
		private Vector2 _rotationCosSin = new Vector2(1, 0);

		/// <summary>
		/// Polygon vertices in relative space, before any transformation is applied, measured in meters. 
		/// NOTE: This array is initialized with CollisionSettings.MaxPolygonVertices number of vertices and should always stay the same size. 
		/// NOTE: You should always call UpdateTransform() after changing vertices.
		/// Use the Count field to keep track of the amount of vertices in a shape.
		/// RESTRICTIONS:
		/// - Vertices must form a convex shape.
		/// - The number of vertices should not be higher than CollisionSettings.MaxPolygonVertices.
		/// - Vertices must be arranged clockwise (assuming the up vector is 0;-1)
		/// </summary>
		public readonly Vector2[] RelativeVertices = new Vector2[CollisionSettings.MaxPolygonVertices];

		/// <summary>
		/// Polygon vertices in absolute space, measured in meters. 
		/// NOTE: These vertices should ONLY be generated from RelativeVertices by calling UpdateTransform()
		/// NOTE: This array is initialized with CollisionSettings.MaxPolygonVertices number of vertices and should always stay the same size. 
		/// Use the Count field to keep track of the amount of vertices in a shape.
		/// RESTRICTIONS:
		/// - Vertices must form a convex shape.
		/// - The number of vertices should not be higher than CollisionSettings.MaxPolygonVertices.
		/// - Vertices must be arranged clockwise (assuming the up vector is 0;-1)
		/// </summary>
		public readonly Vector2[] Vertices = new Vector2[CollisionSettings.MaxPolygonVertices];

		/// <summary>
		/// Number of vertices in a shape. You should ALWAYS use this value instead of Vertices.Length, which always stays constant.
		/// </summary>
		public int Count = 0;

		private const float _radianConversion = MathF.PI / 180f;

		/// <summary>
		/// NOTE: It is recommended to use ShapePool to get new instances of this class.
		/// </summary>
		public Polygon() { }

		private AABB _cachedAABB;
		private bool _cachedAABBStale = true;

		public AABB GetBoundingBox()
		{
			if (_cachedAABBStale)
			{
				_cachedAABB = new AABB(ref Vertices[0], ref Vertices[1]);

				for (var i = 0; i < Count; i += 1)
				{
					_cachedAABB.TopLeft.X = Math.Min(Vertices[i].X, _cachedAABB.TopLeft.X);
					_cachedAABB.TopLeft.Y = Math.Min(Vertices[i].Y, _cachedAABB.TopLeft.Y);
					_cachedAABB.BottomRight.X = Math.Max(Vertices[i].X, _cachedAABB.BottomRight.X);
					_cachedAABB.BottomRight.Y = Math.Max(Vertices[i].Y, _cachedAABB.BottomRight.Y);
				}
				_cachedAABBStale = false;
			}
			return _cachedAABB;
		}


		/// <summary>
		/// Adds a new vertex 
		/// </summary>
		public void Add(Vector2 v)
		{
			if (Count >= CollisionSettings.MaxPolygonVertices)
			{
				throw new InvalidOperationException("Cannot add another vertex! Maximum allowed number of vertices per polygon is " + CollisionSettings.MaxPolygonVertices + ". Consider splitting your polygon.");
			}
			RelativeVertices[Count] = v;
			Count += 1;
		}


		/// <summary>
		/// Applies Position, Rotation to RelativeVertices and writes the result to Vertices. 
		/// Must be called after any changes to the shape have been made.
		/// </summary>
		public void UpdateTransform()
		{
			_cachedAABBStale = true;
			if (Rotation == 0)
			{
				for (var i = 0; i < Count; i += 1)
				{
					Vertices[i] = RelativeVertices[i] + Position;
				}
				_cachedRotation = Rotation;
			}
			else
			{
				if (Rotation != _cachedRotation)
				{
					_rotationCosSin.X = MathF.Cos(Rotation * _radianConversion);
					_rotationCosSin.Y = MathF.Sin(Rotation * _radianConversion);
					_cachedRotation = Rotation;
				}
				for (var i = 0; i < Count; i += 1)
				{
					Vertices[i] = RotateVertex(RelativeVertices[i]) + Position;
				}
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Vector2 RotateVertex(Vector2 p)
		{
			return new Vector2(
				p.X * _rotationCosSin.X - p.Y * _rotationCosSin.Y,
				p.X * _rotationCosSin.Y + p.Y * _rotationCosSin.X
			);
		}

		/// <inheritdoc/>
		public bool InPool { get; set; }


		/// <inheritdoc/>
		public void OnTakenFromPool() {}


		/// <inheritdoc/>
		public void OnReturnedToPool() 
		{
			Count = 0;
			_cachedAABBStale = true;
			Position = Vector2.Zero;
			Rotation = 0;
		}
	}
}
