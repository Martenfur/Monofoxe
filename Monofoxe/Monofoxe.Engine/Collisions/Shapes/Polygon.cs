using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.CustomCollections;
using System;

namespace Monofoxe.Engine.Collisions.Shapes
{
	public class Polygon : IShape, IPoolable
	{
		public ShapeType Type => ShapeType.Polygon;

		public Vector2 Position;
		public Vector2 Origin;
		public float Rotation = 0;
		public float _cachedRotation = 0;
		private Vector2 _rotationCosSin = new Vector2(1, 0);

		/// <summary>
		/// Polygon verts in relative space, before any transformation gets applied.
		/// </summary>
		public readonly Vector2[] RelativeVertices = new Vector2[Settings.MaxPolygonVertices];

		/// <summary>
		/// Polygon verts in absolute space. 
		/// NOTE: These vertices should ONLY be generated from RelativeVertices by calling UpdateTransform()
		/// </summary>
		public readonly Vector2[] Vertices = new Vector2[Settings.MaxPolygonVertices];

		public int Count = 0;

		private const float _radianConversion = MathF.PI / 180f;


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


		public void Add(Vector2 v)
		{
			if (Count >= Settings.MaxPolygonVertices)
			{
				throw new InvalidOperationException("Cannot add another vertex! Maximum allowed number of vertices per polygon is " + Settings.MaxPolygonVertices + ". Consider splitting your polygon.");
			}
			RelativeVertices[Count] = v;
			Count += 1;
		}


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
					Vertices[i] = RotateAroundOrigin(RelativeVertices[i]) + Position;
				}
			}
		}


		private Vector2 RotateAroundOrigin(Vector2 p)
		{
			p -= Origin;

			p = new Vector2(
				p.X * _rotationCosSin.X - p.Y * _rotationCosSin.Y,
				p.X * _rotationCosSin.Y + p.Y * _rotationCosSin.X
			) + Origin;

			return p;
		}

		public bool InPool { get; set; }

		public void OnTakenFromPool()
		{
		}

		public void OnReturnedToPool()
		{
			Count = 0;
			_cachedAABBStale = true;
			Position = Vector2.Zero;
			Origin = Vector2.Zero;
			Rotation = 0;
		}
	}
}
