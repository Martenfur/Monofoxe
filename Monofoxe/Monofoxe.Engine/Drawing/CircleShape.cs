using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable circle shape. Can be drawn by using static methods or be instantiated.
	/// </summary>
	public class CircleShape : IDrawable
	{
		/// <summary>
		/// Center point of a circle.
		/// </summary>
		public Vector2 Position {get; set;}

		public float Radius = 1;

		/// <summary>
		/// If false, circle will be filled with solid color. If true, only outline will be drawn.
		/// </summary>
		public bool IsOutline = false;

		public Color Color = Color.White;

		public void Draw() =>
			Draw(Position, Radius, IsOutline, Color);
		
		

		/// <summary>
		/// Amount of vertices in one circle. 
		/// </summary>
		public static int CircleVerticesCount 
		{
			set
			{
				_circleVectors = new List<Vector2>();
			
				var angAdd = Math.PI * 2 / value;
				
				for(var i = 0; i < value; i += 1)
				{
					_circleVectors.Add(new Vector2((float)Math.Cos(angAdd * i), (float)Math.Sin(angAdd * i)));
				}
			}
			get => _circleVectors.Count;
		}
		private static List<Vector2> _circleVectors; 

		
		/// <summary>
		/// Draws a circle.
		/// </summary>
		public static void Draw(Vector2 p, float r, bool isOutline) =>
			Draw(p, r, isOutline, GraphicsMgr.CurrentColor);
		
		
		/// <summary>
		/// Draws a circle.
		/// </summary>
		public static void Draw(Vector2 p, float r, bool isOutline, Color color)
		{
			short[] indexArray;
			PrimitiveType prType;
			if (isOutline)
			{
				indexArray = new short[CircleVerticesCount * 2];
				prType = PrimitiveType.LineList;
				
				for(var i = 0; i < CircleVerticesCount - 1; i += 1)
				{
					indexArray[i * 2] = (short)i;
					indexArray[i * 2 + 1] = (short)(i + 1);
				}
				indexArray[(CircleVerticesCount - 1) * 2] = (short)(CircleVerticesCount - 1);
				indexArray[(CircleVerticesCount - 1) * 2 + 1] = 0;
			}
			else
			{
				indexArray = new short[CircleVerticesCount * 3];
				prType = PrimitiveType.TriangleList;

				for(var i = 0; i < CircleVerticesCount - 1; i += 1)
				{
					indexArray[i * 3] = 0;
					indexArray[i * 3] = (short)i;
					indexArray[i * 3 + 1] = (short)(i + 1);
				}

			}

			var vertices = new List<VertexPositionColorTexture>();
			
			for(var i = 0; i < CircleVerticesCount; i += 1)
			{
				vertices.Add(
					new VertexPositionColorTexture(
						new Vector3(
							p.X + r * _circleVectors[i].X, 
							p.Y + r * _circleVectors[i].Y, 
							0
						), 
						color, 
						Vector2.Zero
					)
				);
			}
			GraphicsMgr.VertexBatch.Texture = null;
			GraphicsMgr.VertexBatch.DrawPrimitive(prType, vertices.ToArray(), indexArray);
		}

	}
}
