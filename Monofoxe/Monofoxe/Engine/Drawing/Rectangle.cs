using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class Rectangle: Shape
	{
		public Vector2 P1, P2;

		private static readonly short[] indexesFill = {0, 1, 3, 1, 2, 3};
		private static readonly short[] indexesOutline = {0, 1, 1, 2, 2, 3, 3, 0};

		public static void Draw(int x1, int  y1, int x2, int y2, bool isOutline)
		{Draw(x1, y1, x2, y2, isOutline, DrawCntrl.CurrentColor, DrawCntrl.CurrentColor, DrawCntrl.CurrentColor, DrawCntrl.CurrentColor);}

		public static void Draw(int x1, int y1, int x2, int y2, bool isOutline, Color c1, Color c2, Color c3, Color c4)
		{
			short[] indexArray;
			PrimitiveType prType;
			int prAmount;
			if (isOutline)
			{
				indexArray = indexesOutline;
				prType = PrimitiveType.LineList;
				prAmount = 4;
			}
			else
			{
				indexArray = indexesFill;
				prType = PrimitiveType.TriangleList;
				prAmount = 2;
			}


			var vertices = new VertexPositionColor[4];
			
			vertices[0] = new VertexPositionColor(new Vector3(x1, y1, 0), c1);
			vertices[1] = new VertexPositionColor(new Vector3(x2, y1, 0), c2);
			vertices[2] = new VertexPositionColor(new Vector3(x2, y2, 0), c3);
			vertices[3] = new VertexPositionColor(new Vector3(x1, y2, 0), c4);

			DrawPrimitive(prType, vertices, indexArray, prAmount);
		}
	}
}
