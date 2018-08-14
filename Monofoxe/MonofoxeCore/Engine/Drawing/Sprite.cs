using System;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	public class Sprite
	{
		public readonly Frame[] Frames;
		public int W 
		{
			get 
			{
				if (SingleFrameSize)
				{
					return Frames[0].W;
				}	
				throw(new Exception("To use this variable, all frame sizes must be identical!"));
			}
		}
		
		public int H
		{
			get 
			{
				if (SingleFrameSize)
				{
					return Frames[0].H;
				}
				throw(new Exception("To use this variable, all frame sizes must be identical!"));
			}
		}

		public Vector2 Origin;

		/// <summary>
		/// True, is size of every frame is identical.
		/// </summary>
		public bool SingleFrameSize;

		public Sprite(Frame[] frames, int originX, int originY)
		{
			Frames = new Frame[frames.Length];
			Array.Copy(frames, Frames, frames.Length);
			Origin = new Vector2(originX, originY);
			
			SingleFrameSize = CheckIdenticalFrameSizes(frames);
		}

		public Sprite(Frame[] frames, Vector2 origin)
		{
			Frames = new Frame[frames.Length];
			Array.Copy(frames, Frames, frames.Length);
			Origin = origin;
			SingleFrameSize = CheckIdenticalFrameSizes(frames);
		}

		public Sprite(Frame frame, int originX, int originY)
		{
			Frames = new Frame[]{frame};
			Origin = new Vector2(originX, originY);
			
			SingleFrameSize = true;
		}
		

		/// <summary>
		/// Checks if all frames got identical sizes.
		/// </summary>
		/// <param name="frames">Array of frames to check.</param>
		/// <returns></returns>
		private bool CheckIdenticalFrameSizes(Frame[] frames)
		{
			for(var i = 1; i < Frames.Length; i += 1)
			{
				if (Frames[0].W != Frames[i].W || Frames[0].H != Frames[i].H)
				{
					return false;
				}
			}

			return true;
		}

	}
}
