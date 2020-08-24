using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Monofoxe.Tiled.MapStructure;

namespace Monofoxe.Pipeline.Tiled
{
	/// <summary>
	/// Loads and builds external references to textures.
	/// </summary>
	[ContentProcessor(DisplayName = "Tiled Map Processor - Monofoxe")]
	public class TiledMapProcessor : ContentProcessor<TiledMap, TiledMap>
	{
		public static Dictionary<string, ExternalReference<Texture2DContent>> TextureReferences;	

		public override TiledMap Process(TiledMap map, ContentProcessorContext context)
		{
			try
			{
				LoadTextureReferences(map, context);
			}
			catch(System.Exception e)
			{
				throw new System.Exception("Failed to process the map! " + e.Message + " " + e.StackTrace);
			}
			return map;
		}

		public void LoadTextureReferences(TiledMap map, ContentProcessorContext context)
		{
			TextureReferences = new Dictionary<string, ExternalReference<Texture2DContent>>();
			foreach(var tileset in map.Tilesets)
			{
				if (
					tileset.Properties.ContainsKey(TilesetParser.IgnoreTilesetTextureFlag) 
					&& bool.Parse(tileset.Properties[TilesetParser.IgnoreTilesetTextureFlag])
				)
				{
					continue; // Skip texture, if we won't need it.
				}
				foreach(var path in tileset.TexturePaths)
				{
					if (TextureReferences.ContainsKey(path))
					{
						continue;
					}
					//var assetReference = new ExternalReference<Texture2DContent>(TiledMapImporter.TmxRootDir + "/" + path);
					//TextureReferences.Add(path, context.BuildAsset<Texture2DContent, Texture2DContent>(assetReference, "", null, "", ""));
				}
			}

			foreach(var imageLayer in map.ImageLayers)
			{
				if (TextureReferences.ContainsKey(imageLayer.TexturePath))
				{
					continue;
				}
				var assetReference = new ExternalReference<Texture2DContent>(TiledMapImporter.TmxRootDir + "/" + imageLayer.TexturePath, new ContentIdentity(TiledMapImporter.TmxFilename));
				TextureReferences.Add(imageLayer.TexturePath, context.BuildAsset<Texture2DContent, Texture2DContent>(assetReference, "", null, "", ""));
			}

		}

	}
}
