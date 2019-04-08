using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class Text : IDrawable
	{
		public Vector2 Position {get; set;}
		
		public Vector2 Scale;

		public Vector2 Origin;

		public float Rotation;

		public Color Color;

		public string String;

		public Text(string str, Vector2 position, Vector2 scale, Vector2 origin, float rotation = 0)
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
		public static void Draw(string text, float x, float y) => 
			Draw(text, new Vector2(x, y));
		
		/// <summary>
		/// Draws text in specified coordinates.
		/// </summary>
		public static void Draw(string text, Vector2 pos)
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
			CurrentFont.Draw(GraphicsMgr.Batch, text, pos, HorAlign, VerAlign);
		}

		/// <summary>
		/// Draws text in specified coordinates with rotation, scale and origin.
		/// </summary>
		public static void Draw(string text, Vector2 pos, Vector2 scale, Vector2 origin, float rot = 0) => 
			Draw(text, pos.X, pos.Y, scale.X, scale.Y, origin.X, origin.Y, rot);

		/// <summary>
		/// Draws text in specified coordinates with rotation, scale and origin.
		/// </summary>
		public static void Draw(string text, float x, float y, float scaleX, float scaleY, float originX = 0, float originY = 0, float rot = 0)
		{
			if (CurrentFont == null)
			{
				throw new NullReferenceException("CurrentFont is null! Did you forgot to set a font?");
			}

			var transformMatrix = 
				Matrix.CreateTranslation(new Vector3(-originX, -originY, 0)) * // Origin.
				Matrix.CreateRotationZ(MathHelper.ToRadians(-rot)) *		       // Rotation.
				Matrix.CreateScale(new Vector3(scaleX, scaleY, 1)) *	         // Scale.
				Matrix.CreateTranslation(new Vector3(x, y, 0));                // Position.
			
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

			CurrentFont.Draw(GraphicsMgr.Batch, text, Vector2.Zero, HorAlign, VerAlign);
			
			GraphicsMgr.ResetTransformMatrix();
		}
		
	}
}
