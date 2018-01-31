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
		public static Camera CurrentCamera {get; private set;} = null;
		public static BasicEffect BasicEffect;

		public static Color CurrentColor;

		public static void Init(GraphicsDevice device, SpriteBatch batch)
		{
			Batch = batch;
			Device = device;
			Cameras = new List<Camera>();
			BasicEffect = new BasicEffect(Device);
			BasicEffect.VertexColorEnabled = true;

			CurrentColor = Color.White;
		}


		public static void Update()
		{
				
			var depthSortedObjects = Objects.GameObjects.OrderByDescending(o => o.Depth);
			
			// Main draw events.
			foreach(Camera camera in Cameras)
			{
				CurrentCamera = camera;
				BasicEffect.World = camera.CreateTranslationMatrix();
				BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, camera.W, camera.H, 0, 0, 1);


				if (camera.Enabled)
				{
					Device.SetRenderTarget(camera.ViewSurface);
					Device.Clear(camera.BackgroundColor);
					Batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.CreateTranslationMatrix());
					
					foreach(GameObj obj in depthSortedObjects)
					{
						if (obj.Active)
						{obj.DrawBegin();}
					}

					foreach(GameObj obj in depthSortedObjects)
					{
						if (obj.Active)
						{obj.Draw();}
					}
			
					foreach(GameObj obj in depthSortedObjects)
					{
						if (obj.Active)
						{obj.DrawEnd();}
					}
					Batch.End();
				}
			}
			// Main draw events.

			CurrentCamera = null;
			BasicEffect.World = Matrix.CreateTranslation(0, 0, 0);
			BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, 800, 480, 0, 0, 1);


			Device.SetRenderTarget(null);
			
			
			// Drawing camera surfaces.
			Device.Clear(Color.TransparentBlack);
			
			Batch.Begin();
			foreach(Camera camera in Cameras)
			{
				if (camera.Autodraw && camera.Enabled)
				{Batch.Draw(camera.ViewSurface, new Vector2(camera.PortX, camera.PortY), Color.White);}
			}
			Batch.End();
			// Drawing camera surfaces.


			// Drawing GUI stuff.
			Batch.Begin();
			foreach(GameObj obj in depthSortedObjects)
			{
				if (obj.Active)
				{obj.DrawGUI();}
			}
			Batch.End();
			// Drawing GUI stuff.
		}



		public static void DrawSprite(Texture2D texture, int x, int y, Color color)
		{
			Batch.Draw(texture, new Vector2(x, y), color);
		}	
		
		public static void DrawSurface(RenderTarget2D surf, int x, int y, Color color)
		{
			Batch.Draw(surf, new Vector2(x, y), color);
		}	
		
		public static void DrawPrimitive(VertexBuffer buffer)
		{
			Batch.End();

			Device.SetVertexBuffer(buffer);
			
			
			RasterizerState rasterizerState = new RasterizerState();
			//rasterizerState.CullMode = CullMode.None;
			//rasterizerState.FillMode = FillMode.WireFrame;

			Device.RasterizerState = rasterizerState;
			//basicEffect.Alpha = 0.5f;
			foreach(EffectPass pass in BasicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 1);
			}
			
			Batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Cameras[0].CreateTranslationMatrix());
			

		}
		
	}
}
