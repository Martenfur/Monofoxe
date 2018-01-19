using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace Monofoxe.Engine
{
	class DrawCntrl
	{
		
		public static SpriteBatch Batch;
		public static GraphicsDevice Device;

		public static List<Camera> Cameras;

		public static void Init(GraphicsDevice device, SpriteBatch batch)
		{
			Batch = batch;
			Device = device;
			Cameras = new List<Camera>();
		}

		public static void DrawSprite(Texture2D texture, int x, int y, Color color)
		{
			Batch.Draw(texture, new Vector2(x, y), color);
		}	
		
		public static void DrawSurface(RenderTarget2D surf, int x, int y, Color color)
		{
			Batch.Draw(surf, new Vector2(x, y), color);
		}	
		
		
	}
}
