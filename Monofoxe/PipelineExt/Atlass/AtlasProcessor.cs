using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;


namespace PipelineExt
{
	/// <summary>
	/// Gets frame array from AtlasImporter, batches them into sprites according to their names
	/// and passes array of sprites to AtlasWriter.
	/// </summary>
	[ContentProcessor(DisplayName = "Atlas Processor - Monofoxe")]
	public class AtlasProcessor : ContentProcessor<AtlasContainer<Frame>, AtlasContainer<Sprite>>
	{
		public override AtlasContainer<Sprite> Process(AtlasContainer<Frame> input, ContentProcessorContext context)
		{
			var atlasSprites = new AtlasContainer<Sprite>();
			atlasSprites.Texture = input.Texture;
			
			var previousFrameId = -1;
			var previousFrameKey = "";
			var frameList = new List<Frame>();
			
			foreach(Frame frame in input.Items)
			{
				int frameIdPos = frame.Name.LastIndexOf('_');
				int frameId = Int32.Parse(frame.Name.Substring(frameIdPos + 1));
				
				string frameKey = frame.Name.Remove(frameIdPos, frame.Name.Length - frameIdPos);

				if (previousFrameKey.Length == 0)
				{
					previousFrameKey = frameKey;
				}

				// If current frame index is lesser than previous, we got new sprite sheet.
				if (frameId <= previousFrameId && frameList.Count > 0) 
				{			
					// Adding frame array to dictionary with corresponding key and clearing buffer list.
					atlasSprites.Add(new Sprite(previousFrameKey, frameList.ToArray()));
					previousFrameKey = frameKey;
					frameList.Clear();
					// Adding frame array to dictionary with corresponding key and clearing buffer list.
				}
				
				previousFrameId = frameId;
				frameList.Add(frame);
			}

			if (frameList.Count > 0) // If there are any frames left -- we need them too.
			{
				atlasSprites.Add(new Sprite(previousFrameKey, frameList.ToArray()));
			}

			return atlasSprites;
		}
	}
}
