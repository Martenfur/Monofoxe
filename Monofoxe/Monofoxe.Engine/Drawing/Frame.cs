using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Monofoxe.Engine.Drawing
{
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

		/// <summary>
		/// Origin point of the 
		/// </summary>
		public Vector2 Origin;

		public float Rotation;

		public Color Color;

		

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
		
		public object Clone() =>
			new Frame(Texture, TexturePosition, Origin);
		

		public void Draw(
			Vector2 pos, 
			Vector2 scale, 
			float rotation, 
			Vector2 offset, 
			Color color, 
			SpriteEffects effect
		)
		{
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);

			GraphicsMgr.Batch.Draw(
				Texture, 
				pos, 
				TexturePosition, 
				color, 
				MathHelper.ToRadians(rotation), 
				offset,
				scale, 
				effect, 
				0
			);
		}


		public void Draw(Vector2 pos, Vector2 offset) =>
			Draw(pos, Vector2.One, 0, offset, GraphicsMgr.CurrentColor, SpriteEffects.None);
		
		public void Draw(Vector2 pos, Vector2 offset, Vector2 scale, float rotation, Color color)
		{
			var mirroring = SpriteEffects.None;

			// Proper negative scaling.
			if (scale.X < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipHorizontally;
				scale.X *= -1;
				offset.X = Width - offset.X;
			}

			if (scale.Y < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipVertically;
				scale.Y *= -1;
				offset.Y = Height - offset.Y;
			}
			// Proper negative scaling.

			Draw(pos, scale, rotation, offset, color, mirroring);
		}

		
		public void Draw(Rectangle destRect, float rotation, Color color)
		{
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);
			
			GraphicsMgr.Batch.Draw(
				Texture, 
				destRect, 
				TexturePosition, 
				color, 
				rotation,
				// NOTE: Offsets are bugged in 3.6 and mess everything up. Disabled them for now.
				Vector2.Zero, // offset,
				SpriteEffects.None, 
				0
			);
		}

		public void Draw(Rectangle destRect, Rectangle srcRect, float rotation, Color color)
		{
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);
			
			srcRect.X += TexturePosition.X;
			srcRect.Y += TexturePosition.Y;

			GraphicsMgr.Batch.Draw(
				Texture,
				destRect, 
				srcRect, 
				color, 
				rotation, 
				// NOTE: Offsets are bugged in 3.6 and mess everything up. Disabled them for now.
				Vector2.Zero, // offset,
				SpriteEffects.None, 
				0
			);
		}


	}
}
