using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	public class Surface : IDisposable
	{
		public Vector2 Position;

		public Vector2 Scale = Vector2.One;

		public Vector2 Origin;

		public Angle Rotation;

		public Color Color = Color.White;

		public Vector4 ZDepth = Vector4.Zero;

		public RenderTarget2D RenderTarget {get; private set;}

		public Vector2 Size => new Vector2(RenderTarget.Width, RenderTarget.Height);


		internal static bool SurfaceStackEmpty => _surfaceStack.Count == 0;

		/// <summary>
		/// We can set surface targets inside another surfaces.
		/// </summary>
		private static Stack<Surface> _surfaceStack = new Stack<Surface>();
		private static Surface _currentSurface;


		public Surface(Vector2 size, Vector2 position, Vector2 scale, Vector2 origin, Angle rotation)
		{
			Position = position;
			Scale = scale;
			Origin = origin;
			Rotation = rotation;

			RenderTarget = CreateRenderTarget(size);

			Color = GraphicsMgr.CurrentColor;
		}

		public Surface(Vector2 size)
		{
			RenderTarget = CreateRenderTarget(size);
			Color = GraphicsMgr.CurrentColor;
		}
		
		public Surface(RenderTarget2D renderTarget)
		{
			RenderTarget = renderTarget;
			Color = GraphicsMgr.CurrentColor;
		}

		public void Resize(Vector2 size)
		{
			RenderTarget.Dispose();
			RenderTarget = CreateRenderTarget(size);
		}


		private RenderTarget2D CreateRenderTarget(Vector2 size)
		{
			return new RenderTarget2D(
				GraphicsMgr.Device, 
				(int)size.X, (int)size.Y, 
				false,
				GraphicsMgr.Device.PresentationParameters.BackBufferFormat,
				GraphicsMgr.Device.PresentationParameters.DepthStencilFormat, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			
		}


		public void Draw() =>
			Draw(Position, Origin, Scale, Rotation, Color, ZDepth);
		
		
		// Vectors.

		public void Draw(Vector2 position) =>
			Draw(position, Origin, Scale, Rotation, Color, ZDepth);


		public void Draw(
			Vector2 position,
			Vector2 origin,
			Vector2 scale,
			Angle rotation,
			Color color
		) =>
			Draw(position, origin, scale, rotation, color, ZDepth);
		

		public void Draw(
			Vector2 position,
			Vector2 scale,
			Angle rotation
		) =>
			Draw(position, Origin, scale, rotation, Color, ZDepth);
		
		
		public void Draw(
			Vector2 position, 
			Vector2 origin, 
			Vector2 scale, 
			Angle rotation, 
			Color color,
			Vector4 zDepth
		)
		{
			var mirroring = SpriteFlipFlags.None;

			// Proper negative scaling.
			var scaleOffset = Vector2.Zero;

			if (scale.X < 0)
			{
				mirroring = mirroring | SpriteFlipFlags.FlipHorizontally;
				scale.X *= -1;
				scaleOffset.X = Size.X;
			}

			if (scale.Y < 0)
			{
				mirroring = mirroring | SpriteFlipFlags.FlipVertically;
				scale.Y *= -1;
				scaleOffset.Y = Size.Y;
			}
			// Proper negative scaling.

			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.AddQuad(
				position,
				new RectangleF(0, 0, RenderTarget.Width, RenderTarget.Height),
				color,
				rotation.RadiansF,
				scaleOffset + origin,
				scale,
				mirroring,
				zDepth
			);			
		}

		// Vectors.

		// Rectangles.

		public void Draw(RectangleF destRect)
		{
			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.AddQuad(
				destRect,
				new RectangleF(0, 0, RenderTarget.Width, RenderTarget.Height),
				Color
			);
		}

		public void Draw(RectangleF destRect, Angle rotation, Color color)
		{
			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.AddQuad(
				destRect,
				new RectangleF(0, 0, RenderTarget.Width, RenderTarget.Height),
				color,
				rotation.RadiansF,
				Origin,
				SpriteFlipFlags.None,
				ZDepth
			);
		}

		public void Draw(RectangleF destRect, RectangleF srcRect)
		{
			srcRect.X += RenderTarget.Bounds.X;
			srcRect.Y += RenderTarget.Bounds.Y;
			
			GraphicsMgr.VertexBatch.Texture = RenderTarget;
			GraphicsMgr.VertexBatch.AddQuad(destRect, srcRect, Color);
		}

		public void Draw(RectangleF destRect, RectangleF srcRect, Angle rotation, Color color) =>
			Draw(destRect, srcRect, rotation, color, ZDepth);

		public void Draw(RectangleF destRect, RectangleF srcRect, Angle rotation, Color color, Vector4 zDepth)
		{
			srcRect.X += RenderTarget.Bounds.X;
			srcRect.Y += RenderTarget.Bounds.Y;

			GraphicsMgr.VertexBatch.AddQuad(
				destRect, 
				srcRect, 
				color,	
				rotation.RadiansF, 
				Origin,
				SpriteFlipFlags.None, 
				zDepth
			);
		}

		// Rectangles.


		
		/// <summary>
		/// Sets surface as a render target.
		/// </summary>
		public static void SetTarget(Surface surf) =>
			SetTarget(surf, Matrix.CreateTranslation(Vector3.Zero));

		/// <summary>
		/// Sets surface as a render target.
		/// </summary>
		public static void SetTarget(Surface surf, Matrix view) =>
			SetTarget(surf, view, Matrix.CreateOrthographicOffCenter(0, surf.Size.X, surf.Size.Y, 0, 0, 1));
		

		/// <summary>
		/// Sets surface as a render target.
		/// </summary>
		public static void SetTarget(Surface surf, Matrix view, Matrix projection)
		{
			GraphicsMgr.VertexBatch.FlushBatch();
			GraphicsMgr.VertexBatch.PushViewMatrix(view);
			GraphicsMgr.VertexBatch.PushProjectionMatrix(projection);

			_surfaceStack.Push(_currentSurface);
			_currentSurface = surf;


			GraphicsMgr.Device.SetRenderTarget(_currentSurface.RenderTarget);
		}

		/// <summary>
		/// Resets render target to a previous surface.
		/// </summary>
		public static void ResetTarget()
		{
			GraphicsMgr.VertexBatch.FlushBatch();
			GraphicsMgr.VertexBatch.PopViewMatrix();
			GraphicsMgr.VertexBatch.PopProjectionMatrix();

			if (_surfaceStack.Count == 0)
			{
				throw new InvalidOperationException("Surface stack is empty! Did you forgot to set a surface somewhere?");
			}
			_currentSurface = _surfaceStack.Pop();
			
			if (_currentSurface != null)
			{
				GraphicsMgr.Device.SetRenderTarget(_currentSurface.RenderTarget);
			}
			else
			{
				GraphicsMgr.Device.SetRenderTarget(null);
			}
		}



		public void Dispose() =>
			RenderTarget.Dispose();
		
	}
}
