using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Monofoxe.Tiled.MapStructure;

namespace Pipefoxe.Tiled
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
			LoadTextureReferences(map, context);
			
			return map;
		}

		public void LoadTextureReferences(TiledMap map, ContentProcessorContext context)
		{
			TextureReferences = new Dictionary<string, ExternalReference<Texture2DContent>>();
			foreach(var tileset in map.Tilesets)
			{
				if (
					tileset.Properties.ContainsKey(TilesetParser.IgnoreTilesetTextureFlag) 
					&& tileset.Properties[TilesetParser.IgnoreTilesetTextureFlag].ToLower() == "true"
				)
				{
					continue; // Skip texture, if we won't need it.
				}
				foreach(var path in tileset.TexturePaths)
				{
					var asserReference = new ExternalReference<Texture2DContent>(TiledMapImporter.RootDir + "/" + path);
					TextureReferences.Add(path, context.BuildAsset<Texture2DContent, Texture2DContent>(asserReference, "", null, "", ""));
				}
			}

			foreach(var imageLayer in map.ImageLayers)
			{
				if (imageLayer.ImagePath != "")
				{
					var asserReference = new ExternalReference<Texture2DContent>(TiledMapImporter.RootDir + "/" + imageLayer.ImagePath);
					TextureReferences.Add(imageLayer.ImagePath, context.BuildAsset<Texture2DContent, Texture2DContent>(asserReference, "", null, "", ""));
				}
			}

		}

	}
}
