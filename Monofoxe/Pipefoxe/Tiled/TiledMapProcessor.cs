using Microsoft.Xna.Framework.Content.Pipeline;
using Monofoxe.Tiled.MapStructure;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Collections.Generic;
using System;

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
				foreach(var path in tileset.TexturePaths)
				{
					var asserReference = new ExternalReference<Texture2DContent>(TiledMapImporter.RootDir + "/" + path);
					TextureReferences.Add(path, context.BuildAsset<Texture2DContent, Texture2DContent>(asserReference, "", null, "", ""));
				}
			}

		}

	}
}
