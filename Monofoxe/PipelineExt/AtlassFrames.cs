using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace PipelineExt
{
	public class AtlassFrames
	{
		public List<Frame> Frames;
		public TextureContent Texture;

		public AtlassFrames() =>
			Frames = new List<Frame>();
		
		public void Add(Frame frame) =>
			Frames.Add(frame);
	}
}
