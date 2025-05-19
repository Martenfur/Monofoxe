using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	public class Text
	{
		public Vector2 Position;

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


		public void Draw()
		{
			var oldColor = GraphicsMgr.CurrentColor;
			GraphicsMgr.CurrentColor = Color;
			Draw(String, Position, Scale, Origin, Rotation);
			GraphicsMgr.CurrentColor = oldColor;
		}
		
		/// <summary>
		/// Draws text in specified coordinates.
		/// </summary>
		public static void Draw(string text, Vector2 position)
		{
			if (CurrentFont == null)
			{
				throw new NullReferenceException("CurrentFont is null! Did you forget to set a font?");
			}
			
			CurrentFont.Draw(text, position, HorAlign, VerAlign);
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
				Matrix.CreateTranslation(-origin.ToVector3())          // Origin.
				* Matrix.CreateRotationZ(-rotation.RadiansF)		       // Rotation.
				* Matrix.CreateScale(new Vector3(scale.X, scale.Y, 1)) // Scale.
				* Matrix.CreateTranslation(position.ToVector3());      // Position.

			GraphicsMgr.VertexBatch.PushViewMatrix();
			GraphicsMgr.VertexBatch.View = transformMatrix * GraphicsMgr.VertexBatch.View;
			
			CurrentFont.Draw(text, Vector2.Zero, HorAlign, VerAlign);
			
			GraphicsMgr.VertexBatch.PopViewMatrix();
		}
		
	}
}
