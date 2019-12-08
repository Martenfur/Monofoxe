using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	public class Surface : IDrawable, IDisposable
	{
		public Vector2 Position {get; set;}
		
		public Vector2 Scale = Vector2.One;

		public Vector2 Origin;

		public Angle Rotation;

		public Color Color = Color.White;

		public RenderTarget2D RenderTarget {get; private set;}

		public int Width => RenderTarget.Width;
		public int Height => RenderTarget.Height;


		public Surface(int w, int h, Vector2 position, Vector2 scale, Vector2 origin, Angle rotation)
		{
			Position = position;
			Scale = scale;
			Origin = origin;
			Rotation = rotation;

			RenderTarget = CreateRenderTarget(w, h);

			Color = GraphicsMgr.CurrentColor;
		}

		public Surface(int w, int h)
		{
			RenderTarget = CreateRenderTarget(w, h);
			Color = GraphicsMgr.CurrentColor;
		}
		
		public Surface(RenderTarget2D renderTarget)
		{
			RenderTarget = renderTarget;
			Color = GraphicsMgr.CurrentColor;
		}

		public void Resize(int w, int h)
		{
			RenderTarget.Dispose();
			RenderTarget = CreateRenderTarget(w, h);
		}


		private RenderTarget2D CreateRenderTarget(int w, int h)
		{
			return new RenderTarget2D(
				GraphicsMgr.Device, 
				w, h, 
				false,
				GraphicsMgr.Device.PresentationParameters.BackBufferFormat,
				GraphicsMgr.Device.PresentationParameters.DepthStencilFormat, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			
		}


		public void Draw() =>
			Draw(Position, Origin, Scale, Rotation, Color);
		
		
		// Vectors.

		public void Draw(Vector2 position) =>
			Draw(position, Origin, Scale, Rotation, Color);
		
		
		public void Draw(Vector2 position, Vector2 origin, Vector2 scale, Angle rotation, Color color)
		{
			var mirroring = SpriteFlipFlags.None;

			// Proper negative scaling.
			var scaleOffset = Vector2.Zero;

			if (scale.X < 0)
			{
				mirroring = mirroring | SpriteFlipFlags.FlipHorizontally;
				scale.X *= -1;
				scaleOffset.X = Width;
			}

			if (scale.Y < 0)
			{
				mirroring = mirroring | SpriteFlipFlags.FlipVertically;
				scale.Y *= -1;
				scaleOffset.Y = Height;
			}
			// Proper negative scaling.

			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.DrawQuad(
				position,
				Vector2.Zero,
				new Vector2(RenderTarget.Width, RenderTarget.Height),
				color,
				rotation.RadiansF,
				scaleOffset + origin,
				scale,
				mirroring,
				0
			);

			
		}

		// Vectors.

		// Rectangles.

		public void Draw(Rectangle destRect)
		{
			//TODO: Remove rectangles.
			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.DrawQuad(
				new Vector2(destRect.X, destRect.Y),
				new Vector2(destRect.X + destRect.Width, destRect.Y + destRect.Height),
				Vector2.Zero,
				new Vector2(RenderTarget.Width, RenderTarget.Height),
				Color
			);
		}

		public void Draw(Rectangle destRect, Angle rotation, Color color)
		{
			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.DrawQuad(
				new Vector2(destRect.X, destRect.Y),
				new Vector2(destRect.X + destRect.Width, destRect.Y + destRect.Height),
				Vector2.Zero,
				new Vector2(RenderTarget.Width, RenderTarget.Height),
				color,
				rotation.RadiansF,
				Vector2.Zero,
				SpriteFlipFlags.None,
				0
			);
		}

		public void Draw(Rectangle destRect, Rectangle srcRect)
		{
			srcRect.X += RenderTarget.Bounds.X;
			srcRect.Y += RenderTarget.Bounds.Y;
			
			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.DrawQuad(
				new Vector2(destRect.X, destRect.Y),
				new Vector2(destRect.X + destRect.Width, destRect.Y + destRect.Height),
				new Vector2(srcRect.X, srcRect.Y),
				new Vector2(srcRect.X + srcRect.Width, srcRect.Y + srcRect.Height),
				Color
			);
		}
		
		public void Draw(Rectangle destRect, Rectangle srcRect, Angle rotation, Color color)
		{
			srcRect.X += RenderTarget.Bounds.X;
			srcRect.Y += RenderTarget.Bounds.Y;

			GraphicsMgr.VertexBatch.DrawQuad(
				new Vector2(destRect.X, destRect.Y),
				new Vector2(destRect.X + destRect.Width, destRect.Y + destRect.Height),
				new Vector2(srcRect.X, srcRect.Y),
				new Vector2(srcRect.X + srcRect.Width, srcRect.Y + srcRect.Height),
				color,	
				rotation.RadiansF, 
				Vector2.Zero,
				SpriteFlipFlags.None, 
				0
			);
		}

		// Rectangles.
		


		public void Dispose() =>
			RenderTarget.Dispose();
		
	}
}
