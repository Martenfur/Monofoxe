using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class Circle: Shape
	{
		public static int BonesAm = 16;

		public static void Draw(int x, int  y, int r, bool isOutline)
		{
			short[] indexArray;
			PrimitiveType prType;
			int prAmount;
			if (isOutline)
			{
				indexArray = new short[BonesAm * 2];
				prType = PrimitiveType.LineList;
				prAmount = BonesAm;
				
				for(var i = 0; i< BonesAm - 1; i += 1)
				{
					indexArray[i * 2] = (short)i;
					indexArray[i * 2 + 1] = (short)(i + 1);
				}
				indexArray[(BonesAm - 1) * 2] = (short)(BonesAm - 1);
				indexArray[(BonesAm - 1) * 2 + 1] = 0;
			}
			else
			{
				
				prAmount = BonesAm;	
				indexArray = new short[prAmount * 3];
				prType = PrimitiveType.TriangleList;

				for(var i = 1; i < prAmount; i += 1)
				{
					indexArray[i * 3] = 0;
					indexArray[i * 3] = (short)i;
					indexArray[i * 3 + 1] = (short)(i + 1);
				}

			}

			var vertices = new VertexPositionColor[BonesAm];

			for(var i = 0; i < BonesAm; i += 1)
			{
				vertices[i] = new VertexPositionColor(new Vector3((float)(x + r * Math.Cos(Math.PI * 2 / (BonesAm) * i)), 
																													(float)(y + r * Math.Sin(Math.PI * 2 / (BonesAm) * i)), 0), DrawCntrl.CurrentColor);
			}
			DrawPrimitive(prType, vertices, indexArray, prAmount);
		}
	}
}
