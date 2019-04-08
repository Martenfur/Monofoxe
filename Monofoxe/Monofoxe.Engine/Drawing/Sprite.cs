using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class Sprite : IDrawable
	{
		public readonly Frame[] Frames;

		public int Width 
		{
			get
			{
				if (SingleFrameSize)
				{
					return Frames[0].Width;
				}	
				throw new Exception("To use this variable, all frame sizes must be identical!");
			}
		}
		
		public int Height
		{
			get
			{
				if (SingleFrameSize)
				{
					return Frames[0].Height;
				}
				throw new Exception("To use this variable, all frame sizes must be identical!");
			}
		}

		public Vector2 Position {get; set;}
		
		public Vector2 Scale = Vector2.One;

		public Vector2 Origin;

		public float Rotation;

		public double Animation;

		public Color Color;

		/// <summary>
		/// True, if size of every frame is identical.
		/// </summary>
		public bool SingleFrameSize;

		public Sprite(Frame[] frames, int originX, int originY)
		{
			Frames = new Frame[frames.Length];
			foreach(var frame in frames)
			{
				frame.ParentSprite = this;
			}
			Array.Copy(frames, Frames, frames.Length);
			Origin = new Vector2(originX, originY);
			
			SingleFrameSize = CheckIdenticalFrameSizes(frames);
		}

		public Sprite(Frame[] frames, Vector2 origin)
		{
			Frames = new Frame[frames.Length];
			foreach(var frame in frames)
			{
				frame.ParentSprite = this;
			}
			Array.Copy(frames, Frames, frames.Length);
			Origin = origin;
			SingleFrameSize = CheckIdenticalFrameSizes(frames);
		}

		public Sprite(Frame frame, int originX, int originY)
		{
			Frames = new Frame[]{frame};
			frame.ParentSprite = this;
			Origin = new Vector2(originX, originY);
			
			SingleFrameSize = true;
		}
		

		/// <summary>
		/// Checks if all frames got identical sizes.
		/// </summary>
		/// <param name="frames">Array of frames to check.</param>
		private bool CheckIdenticalFrameSizes(Frame[] frames)
		{
			for(var i = 1; i < Frames.Length; i += 1)
			{
				if (Frames[0].Width != Frames[i].Width || Frames[0].Height != Frames[i].Height)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns sprite frame based on an animation value from 0 to 1.
		/// </summary>
		private Frame GetFrame(double animation) =>
			Frames[Math.Max(0, Math.Min(Frames.Length - 1, (int)(animation * Frames.Length)))];
		
		
		public void Draw() =>
			GetFrame(Animation).Draw(Position, Origin, Scale, Rotation, Color);
		
		// Vectors.
		// TODO: Ad frame origins.
		public void Draw(Vector2 pos, Vector2 origin) =>
			Frames[0].Draw(pos, Vector2.One, 0, origin, GraphicsMgr.CurrentColor, SpriteEffects.None);
		
		public void Draw(double animation, Vector2 pos, Vector2 origin) =>
			GetFrame(animation).Draw(pos, origin);
		
		public void Draw(double animation, Vector2 pos, Vector2 origin, Vector2 scale, float rotation, Color color) =>
			GetFrame(animation).Draw(pos, origin, scale, rotation, color);

		// Vectors.
		
		
		// Rectangles.

		public void Draw(double animation, Rectangle destRect) =>
			GetFrame(animation).Draw(destRect, 0, GraphicsMgr.CurrentColor);

		public void Draw(double animation, Rectangle destRect, float rotation, Color color) =>
			GetFrame(animation).Draw(destRect, rotation, color);

		public void Draw(double animation, Rectangle destRect, Rectangle srcRect) => 
			GetFrame(animation).Draw(destRect, srcRect, 0, GraphicsMgr.CurrentColor);
		

		public void Draw(double animation, Rectangle destRect, Rectangle srcRect, float rotation, Color color) =>
			GetFrame(animation).Draw(destRect, srcRect, rotation, color);

		// Rectangles.


		

	}
}
