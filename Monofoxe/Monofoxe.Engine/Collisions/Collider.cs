using Microsoft.Xna.Framework;
using Monofoxe.Engine.Collisions.Shapes;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.CustomCollections;
using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.Collisions
{
	/// <summary>
	/// Defines a collection of convex shapes that act as a single shape.
	/// </summary>
	public class Collider : IPoolable
	{
		private List<IShape> _shapes = new List<IShape>();


		/// <summary>
		/// Absolute position of the collider, measured in meters.
		/// NOTE: You should always call UpdateTransform() after changing this field.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Origin point for the collider rotation, measured in meters.
		/// NOTE: You should always call UpdateTransform() after changing this field.
		/// </summary>
		public Vector2 Origin;
		public Vector2 _rotatedOrigin;

		/// <summary>
		/// Rotation of the collider, measured in degreen.
		/// NOTE: You should always call UpdateTransform() after changing this field.
		/// </summary>
		public float Rotation;
		public float _cachedRotation = 0;
		private Vector2 _rotationCosSin = new Vector2(1, 0);

		/// <summary>
		/// NOTE: It is recommended to use ColliderPool to get new instances of this class.
		/// </summary>
		public Collider() { }

		public int ShapesCount => _shapes.Count;

		private const float _radianConversion = MathF.PI / 180f;

		
		public void AddShape(IShape shape)
		{
			_shapes.Add(shape);
		}


		public void RemoveShape(IShape shape)
		{
			var index = _shapes.IndexOf(shape);
			if (index != -1) 
			{
				_shapes.RemoveAt(index);
			}
		}


		public IShape GetShape(int index = 0)
		{
			return _shapes[index];
		}


		/// <summary>
		/// Applies Position, Rotation, Origin to the shapes. 
		/// Must be called after any changes to the collider have been made.
		/// </summary>
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
						circle.Position = _rotatedOrigin + Position;
						break;
					case ShapeType.Polygon:
						var poly = (Polygon)_shapes[i];
						poly.Position = _rotatedOrigin + Position;
						poly.Rotation = Rotation;
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


		/// <inheritdoc/>
		public bool InPool { get; set; }


		/// <inheritdoc/>
		public void OnReturnedToPool()
		{
			_shapes.Clear();
			for(var i = 0; i < _shapes.Count; i += 1)
			{
				ShapePool.Return(_shapes[i]);
			}
		}


		/// <inheritdoc/>
		public void OnTakenFromPool() {}
	}
}
