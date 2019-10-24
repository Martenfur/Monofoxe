using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	public class Text : IDrawable
	{
		public Vector2 Position {get; set;}
		
		public Vector2 Scale;

		public Vector2 Origin;

		public Angle Rotation;

		public Color Color;

		public string String;

		public Text(string str, Vector2 position, Vector2 scale, Vector2 origin, Angle rotation)
		{
			String = str;
			Position = position;
			Scale = scale;
			Origin = origin;
			Rotation = rotation;

			Color = GraphicsMgr.CurrentColor;
		}

		// Text.
		public static IFont CurrentFont;

		public static TextAlign HorAlign = TextAlign.Left;
		public static TextAlign VerAlign = TextAlign.Top;
		// Text.


		public void Draw() =>
			Draw(String, Position, Scale, Origin, Rotation);
		
		/// <summary>
		/// Draws text in specified coordinates.
		/// </summary>
		public static void Draw(string text, Vector2 position)
		{
			if (CurrentFont == null)
			{
				throw new NullReferenceException("CurrentFont is null! Did you forgot to set a font?");
			}

			/*
			 * Font is a wrapper for MG's SpriteFont, which uses non-premultiplied alpha.
			 * Using GraphicsMode.Sprites will result in black pixels everywhere.
			 * TextureFont, on the other hand, is just a bunch of regular sprites, 
			 * so it's fine to draw with sprite mode.
			 */
			if (CurrentFont is Font)
			{
				GraphicsMgr.SwitchGraphicsMode(GraphicsMode.SpritesNonPremultiplied);
			}
			else
			{
				GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);	
			}
			CurrentFont.Draw(GraphicsMgr._batch, text, position, HorAlign, VerAlign);
		}

		/// <summary>
		/// Draws text in specified coordinates with rotation, scale and origin.
		/// </summary>
		public static void Draw(
			string text,
			Vector2 position, 
			Vector2 scale, Vector2 origin,
			Angle rotation
		)
		{
			if (CurrentFont == null)
			{
				throw new NullReferenceException("CurrentFont is null! Did you forgot to set a font?");
			}

			var transformMatrix = 
				Matrix.CreateTranslation(new Vector3(-origin.X, -origin.Y, 0)) *  // Origin.
				Matrix.CreateRotationZ(-rotation.RadiansF) *		                  // Rotation.
				Matrix.CreateScale(new Vector3(scale.X, scale.Y, 1)) *	          // Scale.
				Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0)); // Position.
			
			GraphicsMgr.AddTransformMatrix(transformMatrix);
			
			/*
			 * Font is a wrapper for MG's SpriteFont, which uses non-premultiplied alpha.
			 * Using GraphicsMode.Sprites will result in black pixels everywhere.
			 * TextureFont, on the other hand, is just regular sprites, so it's fine to 
			 * draw with sprite mode.
			 */
			if (CurrentFont is Font)
			{
				GraphicsMgr.SwitchGraphicsMode(GraphicsMode.SpritesNonPremultiplied);
			}
			else
			{
				GraphicsMgr.SwitchGraphicsMode(GraphicsMode.Sprites);	
			}

			CurrentFont.Draw(GraphicsMgr._batch, text, Vector2.Zero, HorAlign, VerAlign);
			
			GraphicsMgr.ResetTransformMatrix();
		}
		
	}
}
