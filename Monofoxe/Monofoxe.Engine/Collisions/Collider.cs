using Microsoft.Xna.Framework;
using Monofoxe.Engine.Collisions.Shapes;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.CustomCollections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Monofoxe.Engine.Collisions
{
	public class Collider : IPoolable
	{
		public bool InPool { get ; set; }

		private List<IShape> _shapes = new List<IShape>();
		private List<Vector2> _relativeShapePositions = new List<Vector2>();


		public Vector2 Position;
		public Vector2 Origin;
		public Vector2 _rotatedOrigin;
		public float Rotation;
		public float _cachedRotation = 0;
		private Vector2 _rotationCosSin = new Vector2(1, 0);


		public int ShapesCount => _shapes.Count;

		private const float _radianConversion = MathF.PI / 180f;

		public void AddShape(Vector2 relativePosition, IShape shape)
		{
			_shapes.Add(shape);
			_relativeShapePositions.Add(relativePosition);
		}

		public void RemoveShape(IShape shape)
		{
			var index = _shapes.IndexOf(shape);
			if (index != -1) 
			{
				_shapes.RemoveAt(index);
				_relativeShapePositions.RemoveAt(index);
			}
		}


		public IShape GetShape(int index = 0)
		{
			return _shapes[index];
		}


		public Vector2 GetShapeRelativePosition(int index = 0)
		{
			return _relativeShapePositions[index];
		}


		public void SetShapeRelativePosition(Vector2 relativePosition, int index = 0)
		{
			_relativeShapePositions[index] = relativePosition;
		}


		public void UpdateTransform()
		{
			if (Rotation != _cachedRotation)
			{
				_rotationCosSin.X = MathF.Cos(Rotation * _radianConversion);
				_rotationCosSin.Y = MathF.Sin(Rotation * _radianConversion);
				_cachedRotation = Rotation;
			}
			
			_rotatedOrigin = new Vector2(
				-Origin.X * _rotationCosSin.X + Origin.Y * _rotationCosSin.Y,
				-Origin.X * _rotationCosSin.Y - Origin.Y * _rotationCosSin.X
			);
			

			for (var i = 0; i < _shapes.Count; i += 1)
			{
				switch(_shapes[i].Type)
				{
					case ShapeType.Circle:
						var circle = (Circle)_shapes[i];
						// TODO: Add rotation.
						circle.Position = _rotatedOrigin + Position;
						break;
					case ShapeType.Polygon:
						var poly = (Polygon)_shapes[i];
						poly.Position = _rotatedOrigin + Position + _relativeShapePositions[i];
						poly.Rotation = Rotation;
						//poly.Origin = _relativeShapePositions[i];
						poly.UpdateTransform();
						break;
				}
			}
		}

		public AABB GetBoundingBox()
		{
			// TODO: Add AABB caching?
			var aabb = _shapes[0].GetBoundingBox();
			for(var i = 1; i < _shapes.Count; i += 1)
			{
				var aabb1 = _shapes[i].GetBoundingBox();
				aabb.Combine(ref aabb1);
			}

			return aabb;
		}


		public void OnReturnedToPool()
		{
			_shapes.Clear();
			for(var i = 0; i < _shapes.Count; i += 1)
			{
				ShapePool.Return(_shapes[i]);
			}
			_relativeShapePositions.Clear();
		}

		public void OnTakenFromPool() {}
	}
}
