using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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
				else
				{
					throw(new Exception("To use this variable, all frame sizes must be identical!"));
				}
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
				else
				{
					throw(new Exception("To use this variable, all frame sizes must be identical!"));
				}
			}
		}

		public Vector2 Origin;

		/// <summary>
		/// True, is size of every frame is identical.
		/// </summary>
		public bool SingleFrameSize;

		public Sprite(Frame[] frames, int originX, int originY)
		{
			Frames = new Frame[frames.Count()];
			Array.Copy(frames, Frames, frames.Count());
			Origin = new Vector2(originX, originY);
			
			SingleFrameSize = CheckIdenticalFrameSizes(frames);
		}

		public Sprite(Frame[] frames, Vector2 origin)
		{
			Array.Copy(frames, Frames, frames.Count());
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
			for(var i = 1; i < Frames.Count(); i += 1)
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
