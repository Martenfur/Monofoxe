using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;


namespace PipelineExt
{
	[ContentProcessor(DisplayName = "Texture Atlass Processor")]
	public class AtlassProcessor : ContentProcessor<AtlassFrames, AtlassSprites>
	{
		public override AtlassSprites Process(AtlassFrames input, ContentProcessorContext context)
		{
			AtlassSprites atlassSprites = new AtlassSprites();
			atlassSprites.Texture = input.Texture;


			var previousFrameId = -1;
			var previousFrameKey = "";

			List<Frame> frameList = new List<Frame>();
			
			foreach(Frame frame in input.Frames)
			{
				int frameIdPos = frame.Name.LastIndexOf('_');
				int frameId = Int32.Parse(frame.Name.Substring(frameIdPos + 1));
				
				string frameKey = frame.Name.Remove(frameIdPos, frame.Name.Length - frameIdPos);

				if (previousFrameKey.Length == 0)
				{
					previousFrameKey = frameKey;
				}

				// Creating frame.
				
				// If current frame index is lesser than previous, we got new sprite sheet.
				if (frameId <= previousFrameId && frameList.Count > 0) 
				{			
					// Adding frame array to dictionary with corresponding key and clearing buffer list.
					atlassSprites.Add(new Sprite(previousFrameKey, frameList.ToArray()));
					previousFrameKey = frameKey;
					frameList.Clear();
					// Adding frame array to dictionary with corresponding key and clearing buffer list.
				}
				
				previousFrameId = frameId;
				frameList.Add(frame);
			}

			if (frameList.Count > 0) // If there are any frames left -- we need them too.
			{
				atlassSprites.Add(new Sprite(previousFrameKey, frameList.ToArray()));
			}

			return atlassSprites;
		}
	}
}
