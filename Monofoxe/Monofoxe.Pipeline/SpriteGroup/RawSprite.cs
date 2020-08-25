using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Monofoxe.Pipeline.SpriteGroup
{
	public class RawSprite
	{
		public string Name = "NONE";
		public int FramesH = 1;
		public int FramesV = 1;
		public Point Offset = new Point(0, 0);
		public Bmp RawTexture; 
		public List<Frame> Frames = new List<Frame>();

		/// <summary>
		/// Tells how many frames were already rendered to the atlas.
		/// </summary>
		public int RenderedFrames = 0; 
	}
}
