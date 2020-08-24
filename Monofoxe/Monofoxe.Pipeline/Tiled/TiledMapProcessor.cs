using Microsoft.Xna.Framework.Content.Pipeline;
using Monofoxe.Tiled.MapStructure;

namespace Monofoxe.Pipeline.Tiled
{
  /// <summary>
  /// Loads and builds external references to textures.
  /// </summary>
  [ContentProcessor(DisplayName = "Tiled Map Processor - Monofoxe")]
	public class TiledMapProcessor : ContentProcessor<TiledMap, TiledMap>
	{
		
		public override TiledMap Process(TiledMap map, ContentProcessorContext context)
		{
			try
			{
				// TODO: Remove.
			}
			catch(System.Exception e)
			{
				throw new System.Exception("Failed to process the map! " + e.Message + " " + e.StackTrace);
			}
			return map;
		}

	}
}
