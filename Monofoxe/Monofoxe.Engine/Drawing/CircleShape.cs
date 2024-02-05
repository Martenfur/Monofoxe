using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable circle shape. Can be drawn by using static methods or be instantiated.
	/// </summary>
	public static class CircleShape
	{
		static CircleShape()
		{
			CircleVerticesCount = 16;
		}

		/// <summary>
		/// Amount of vertices in one circle. 
		/// </summary>
		public static int CircleVerticesCount 
		{
			set
			{
				if (_circleVerticesCount == value)
				{
					return;
				}
				_circleVerticesCount = value;
				_circleVectors = new Vector2[value];
				_circleVertices = new VertexPositionColorTexture[value];

				var angAdd = Math.PI * 2 / value;
				
				for(var i = 0; i < value; i += 1)
				{
					_circleVectors[i] = new Vector2((float)Math.Cos(angAdd * i), (float)Math.Sin(angAdd * i));
				}
				CreateIndexBuffers();
			}
			get => _circleVerticesCount;
		}
		private static int _circleVerticesCount = 0;


		private static Vector2[] _circleVectors = new Vector2[_circleVerticesCount];

		private static VertexPositionColorTexture[] _circleVertices = new VertexPositionColorTexture[_circleVerticesCount];

		private static short[] _filledCircleIndices = new short[_circleVerticesCount * 3];
		private static short[] _outlineCircleIndices = new short[_circleVerticesCount * 2];

		/// <summary>
		/// Draws a circle.
		/// </summary>
		public static void Draw(Vector2 p, float r, bool isOutline) =>
			Draw(p, r, isOutline, GraphicsMgr.CurrentColor);
		
		
		/// <summary>
		/// Draws a circle.
		/// </summary>
		public static void Draw(Vector2 p, float r, bool isOutline, Color color, float zDepth = 0)
		{
			
			for(var i = 0; i < _circleVerticesCount; i += 1)
			{
				_circleVertices[i].Position = new Vector3(
					p.X + r * _circleVectors[i].X, 
					p.Y + r * _circleVectors[i].Y, 
					zDepth
				);
				_circleVertices[i].Color = color;
			}
			GraphicsMgr.VertexBatch.Texture = null;
			if (isOutline)
			{
				GraphicsMgr.VertexBatch.AddPrimitive(PrimitiveType.LineList, _circleVertices, _outlineCircleIndices);
			}
			else
			{
				GraphicsMgr.VertexBatch.AddPrimitive(PrimitiveType.TriangleList, _circleVertices, _filledCircleIndices);
			}

		}
		

		private static void CreateIndexBuffers()
		{
			_filledCircleIndices = new short[_circleVerticesCount * 3];
			for (var i = 0; i < _circleVerticesCount - 1; i += 1)
			{
				_filledCircleIndices[i * 3] = 0;
				_filledCircleIndices[i * 3] = (short)i;
				_filledCircleIndices[i * 3 + 1] = (short)(i + 1);
			}


			_outlineCircleIndices = new short[_circleVerticesCount * 2];
			for (var i = 0; i < _circleVerticesCount - 1; i += 1)
			{
				_outlineCircleIndices[i * 2] = (short)i;
				_outlineCircleIndices[i * 2 + 1] = (short)(i + 1);
			}
			_outlineCircleIndices[(_circleVerticesCount - 1) * 2] = (short)(_circleVerticesCount - 1);
			_outlineCircleIndices[(_circleVerticesCount - 1) * 2 + 1] = 0;
		}

	}
}
