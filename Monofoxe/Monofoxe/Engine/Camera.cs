using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;

namespace Monofoxe.Engine
{
	class Camera
	{

		public int ViewX =     0, 
		           ViewY =     0,
		           ViewportX = 0, 
		           ViewportY = 0,
							 Rotation =  0;

		public float ScaleX = 1,
		             ScaleY = 1;

		public RenderTarget2D ViewSurface;

		public Camera(int x, int y, int w, int h)
		{
			ViewSurface = new RenderTarget2D(DrawCntrl.Device, w, h);
			ViewX = x;
			ViewY = y;
			DrawCntrl.Cameras.Add(this);
		}

		public Matrix CreateTranslationMatrix()
		{
			return Matrix.CreateTranslation(new Vector3(ViewX, ViewY, 0)) *
                                          Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) *
                                          Matrix.CreateScale(new Vector3(ScaleX, ScaleY, 1)) *
                                          Matrix.CreateTranslation(new Vector3(0, 0, 0)); 
		}



	}
}
