using Microsoft.Xna.Framework.Content.Pipeline;
using Monofoxe.Tiled.MapStructure;

namespace Pipefoxe.Tiled
{
	/// <summary>
	/// Encrypts json string with basic encryption.
	/// This won't prevent anyone from hacking into files, but
	/// entity templates won't be left as plain text files anymore.
	/// </summary>
	[ContentProcessor(DisplayName = "Tiled Map Processor - Monofoxe")]
	public class TiledMapProcessor : ContentProcessor<TiledMap, TiledMap>
	{
		public override TiledMap Process(TiledMap map, ContentProcessorContext context)
		{
			return map;
		}
	}
}
