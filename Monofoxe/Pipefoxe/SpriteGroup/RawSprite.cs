using System.Collections.Generic;
using System.Drawing;

namespace Pipefoxe.SpriteGroup
{
	public class RawSprite
	{
		public string Name = "NONE";
		public int FramesH = 1;
		public int FramesV = 1;
		public Point Offset = new Point(0, 0);
		public Image RawTexture; 
		public List<Frame> Frames = new List<Frame>();
		/// <summary>
		/// Tells how many frames were already rendered to the atlas.
		/// </summary>
		public int RenderedFrames = 0; 
	}
}
