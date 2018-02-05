using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	class Triangle: Shape
	{
		private static readonly short[][] _indexes = {new short[]{0, 1, 1, 2, 2, 0}, new short[]{0, 1, 2}};
		private static readonly DrawCntrl.PipelineModes[] _types = {DrawCntrl.PipelineModes.OutlinePrimitives, DrawCntrl.PipelineModes.TrianglePrimitives};
		private static readonly int[] _prAmounts = {3, 1};

		public bool IsOutline;
		public int X1, Y1, X2, Y2, X3, Y3;
		public Color Color;

		public Triangle(int x1, int  y1, int x2, int y2, int x3, int y3, bool isOutline, Color color)
		{
			X1 = x1;
			Y1 = y1;
			
			X2 = x2;
			Y2 = y2;
			
			X3 = x3;
			Y3 = y3;

			IsOutline = isOutline;

			Color = color;
		}

		
		/*
		 * static:
		 * type
		 * vertex array
		 * index array
		 * amount of primitives
		 * 
		 * object:
		 * type
		 * vertex buffer
		 * index array
		 * amount of primitives
		 */ 

		public static void Draw(int x1, int  y1, int x2, int y2, int x3, int y3, bool isOutline)
		{Draw(x1, y1, x2, y2, x3, y3, isOutline, DrawCntrl.CurrentColor, DrawCntrl.CurrentColor, DrawCntrl.CurrentColor);}

		public static void Draw(int x1, int  y1, int x2, int y2, int x3, int y3, bool isOutline, Color c1, Color c2, Color c3)
		{
			int o;
			if (isOutline)
			{o = 0;}
			else
			{o = 1;}

			var vertices = new List<VertexPositionColorTexture>();

			vertices.Add(new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, new Vector2(0, 0)));
			vertices.Add(new VertexPositionColorTexture(new Vector3(x2, y2, 0), c2, new Vector2(0, 0)));
			vertices.Add(new VertexPositionColorTexture(new Vector3(x3, y3, 0), c3, new Vector2(0, 0)));
			
			DrawCntrl.AddPrimitive(_types[o], vertices, new List<short>(_indexes[o]));
		}
	}
}
