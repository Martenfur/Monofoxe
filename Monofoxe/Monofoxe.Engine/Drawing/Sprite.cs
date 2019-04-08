using System;
using Microsoft.Xna.Framework;

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
		

	}
}
