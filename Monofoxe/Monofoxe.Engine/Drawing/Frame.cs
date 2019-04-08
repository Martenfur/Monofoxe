using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Monofoxe.Engine.Drawing
{
	public class Frame : ICloneable
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
		public readonly int Width;

		/// <summary>
		/// Height of the frame.
		/// </summary>
		public readonly int Height;
		
		/// <summary>
		/// Origin point of the frame.
		/// </summary>
		public readonly Vector2 Origin;

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

		public Frame(Texture2D texture, Rectangle texturePosition, Vector2 origin, int w, int h)
		{
			Texture = texture;
			TexturePosition = texturePosition;
			
			Origin = origin;
			Width = w;
			Height = h;
		}

		public object Clone() =>
			new Frame(Texture, TexturePosition, Origin, Width, Height);
	}
}
