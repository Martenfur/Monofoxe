using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable frame.
	/// </summary>
	public class Frame : IDrawable, ICloneable
	{
		/// <summary>
		/// Texture atlas where frame is stored.
		/// </summary>
		public readonly Texture2D Texture;

		/// <summary>
		/// Position of the frame on the atlas.
		/// </summary>
		public readonly Rectangle TexturePosition;

		/// <summary>
		/// Width of the frame.
		/// </summary>
		public int Width => TexturePosition.Width;

		/// <summary>
		/// Height of the frame.
		/// </summary>
		public int Height => TexturePosition.Height;
		
		

		public Vector2 Position {get; set;}
		
		public Vector2 Scale = Vector2.One;
		
		public Vector2 Origin;

		public Angle Rotation;

		public Color Color = Color.White;

		

		/// <summary>
		/// Frame's parent sprite.
		/// </summary>
		public Sprite ParentSprite 
		{
			get => _parentSprite;
			internal set
			{
				if (_parentSprite != null)
				{
					throw new Exception("This frame already belongs to a sprite!");
				}
				_parentSprite = value;
			}
		}
		private Sprite _parentSprite = null;

		public Frame(Texture2D texture, Rectangle texturePosition, Vector2 origin)
		{
			Texture = texture;
			TexturePosition = texturePosition;
			
			Origin = origin;
		}

		public void Draw() =>
			Draw(Position, Origin, Scale, Rotation, Color);
		

		public void Draw(
			Vector2 position, 
			Vector2 origin, 
			Vector2 scale, 
			Angle rotation, 
			Color color, 
			SpriteFlipFlags flipFlags
		)
		{
			GraphicsMgr.VertexBatch.Texture = Texture;
			
			GraphicsMgr.VertexBatch.DrawQuad(
				position,
				new Vector2(TexturePosition.X, TexturePosition.Y),
				new Vector2(TexturePosition.X + TexturePosition.Width, TexturePosition.Y + TexturePosition.Height),
				color,
				rotation.RadiansF,
				origin,
				scale,
				flipFlags,
				0
			);
			
		}

		public void Draw(Vector2 position) =>
			Draw(position, Origin, Scale, Rotation, Color);

		public void Draw(Vector2 position, Vector2 origin) =>
			Draw(position, origin, Scale, Rotation, Color);

		public void Draw(Vector2 position, Vector2 origin, Vector2 scale, Angle rotation, Color color)
		{
			var flipFlags = SpriteFlipFlags.None;

			// Proper negative scaling.
			if (scale.X < 0)
			{
				flipFlags = flipFlags | SpriteFlipFlags.FlipHorizontally;
				scale.X *= -1;
				origin.X = Width - origin.X;
			}

			if (scale.Y < 0)
			{
				flipFlags = flipFlags | SpriteFlipFlags.FlipVertically;
				scale.Y *= -1;
				origin.Y = Height - origin.Y;
			}
			// Proper negative scaling.

			Draw(position, origin, scale, rotation, color, flipFlags);
		}

		
		public void Draw(Rectangle destRect, Angle rotation, Color color)
		{
			GraphicsMgr.VertexBatch.Texture = Texture;
			//TODO: Get rid of Rectangle.
			GraphicsMgr.VertexBatch.DrawQuad(
				new Vector2(destRect.X, destRect.Y),
				new Vector2(destRect.X + destRect.Width, destRect.Y + destRect.Height),

				new Vector2(TexturePosition.X, TexturePosition.Y),
				new Vector2(TexturePosition.X + TexturePosition.Width, TexturePosition.Y + TexturePosition.Height),
				color,
				rotation.RadiansF,
				Vector2.Zero,
				SpriteFlipFlags.None,
				0
			);
		}

		public void Draw(Rectangle destRect, Rectangle srcRect, Angle rotation, Color color)
		{
			srcRect.X += TexturePosition.X;
			srcRect.Y += TexturePosition.Y;
			
			GraphicsMgr.VertexBatch.Texture = Texture;
			//TODO: Get rid of Rectangle.
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



		public object Clone()
		{
			var frame = new Frame(Texture, TexturePosition, Origin);
			frame.Position = Position;
			frame.Scale = Scale;
			frame.Rotation = Rotation;
			frame.Color = Color;
			return frame;
		}

	}
}
