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
			SpriteEffects effect
		)
		{
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);

			GraphicsMgr.Batch.Draw(
				Texture, 
				position, 
				TexturePosition, 
				color, 
				rotation.RadiansF, 
				origin,
				scale, 
				effect, 
				0
			);
		}

		public void Draw(Vector2 position) =>
			Draw(position, Origin, Scale, Rotation, Color);

		public void Draw(Vector2 position, Vector2 origin) =>
			Draw(position, origin, Scale, Rotation, Color);

		public void Draw(Vector2 position, Vector2 origin, Vector2 scale, Angle rotation, Color color)
		{
			var mirroring = SpriteEffects.None;

			// Proper negative scaling.
			if (scale.X < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipHorizontally;
				scale.X *= -1;
				origin.X = Width - origin.X;
			}

			if (scale.Y < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipVertically;
				scale.Y *= -1;
				origin.Y = Height - origin.Y;
			}
			// Proper negative scaling.

			Draw(position, origin, scale, rotation, color, mirroring);
		}

		
		public void Draw(Rectangle destRect, Angle rotation, Color color)
		{
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);
			
			GraphicsMgr.Batch.Draw(
				Texture, 
				destRect, 
				TexturePosition, 
				color, 
				rotation.RadiansF,
				// NOTE: Offsets are bugged in 3.6 and mess everything up. Disabled them for now.
				Vector2.Zero, // offset,
				SpriteEffects.None, 
				0
			);
		}

		public void Draw(Rectangle destRect, Rectangle srcRect, Angle rotation, Color color)
		{
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);
			
			srcRect.X += TexturePosition.X;
			srcRect.Y += TexturePosition.Y;

			GraphicsMgr.Batch.Draw(
				Texture,
				destRect, 
				srcRect, 
				color, 
				rotation.RadiansF, 
				// NOTE: Offsets are bugged in 3.6 and mess everything up. Disabled them for now.
				Vector2.Zero, // offset,
				SpriteEffects.None, 
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
