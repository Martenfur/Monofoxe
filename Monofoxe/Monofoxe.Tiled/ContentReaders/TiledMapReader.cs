using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Monofoxe.Tiled.MapStructure;
using System;
using System.Diagnostics;

namespace Monofoxe.Tiled.ContentReaders
{
	/// <summary>
	/// Reads sprite group file.
	/// </summary>
	public class TiledMapReader : ContentTypeReader<TiledMap>
	{
		protected override TiledMap Read(ContentReader input, TiledMap existingInstance)
		{
			var map = new TiledMap();
			map.BackgroundColor = input.ReadObject<Color?>();
			map.Width = input.ReadInt32();
			map.Height = input.ReadInt32();
			map.TileWidth = input.ReadInt32();
			map.TileHeight = input.ReadInt32();

			ReadTilesets(input, map);
			
			return map;
		}

		void ReadTilesets(ContentReader input, TiledMap map)
		{
			var tilesetCount = input.ReadInt32();
			var tilesets = new TiledMapTileset[tilesetCount];

			// Reader:
			//var texture = reader.ReadExternalReference<Texture2D>();

			// Writer:
			//var externalReference = _contentItem.GetExternalReference<Texture2DContent>(imageLayer.Image.Source);
			//writer.WriteExternalReference(externalReference);


			for(var i = 0; i < tilesetCount; i += 1)
			{
				tilesets[i] = new TiledMapTileset();

				tilesets[i].Name = input.ReadString();
				tilesets[i].TexturePaths = input.ReadObject<string[]>();

				tilesets[i].FirstGID = input.ReadInt32();
				tilesets[i].TileWidth = input.ReadInt32();
				tilesets[i].TileHeight = input.ReadInt32();
				tilesets[i].Spacing = input.ReadInt32();
				tilesets[i].Margin = input.ReadInt32();
				tilesets[i].TileCount = input.ReadInt32();
				tilesets[i].Columns = input.ReadInt32();
				tilesets[i].Offset = input.ReadVector2();
				
				var tiles = new TiledMapTilesetTile[tilesets[i].TileCount];
				for(var k = 0; k < tiles.Length; k += 1)
				{
					tiles[k] = ReadTilesetTile(input);
					tiles[k].Tileset = tilesets[i];
				}
				tilesets[i].Tiles = tiles;
				tilesets[i].BackgroundColor = input.ReadObject<Color?>();
				tilesets[i].Properties = input.ReadObject<Dictionary<string, string>>();
			}

			map.Tilesets = tilesets;
		}



		TiledMapTilesetTile ReadTilesetTile(ContentReader input)
		{
			var tile = new TiledMapTilesetTile();
			tile.GID = input.ReadInt32();
			tile.TextureID = input.ReadInt32();
			tile.TexturePosition = input.ReadObject<Rectangle>();
			tile.Properties = input.ReadObject<Dictionary<string, string>>();
			
			return tile;
		}


	}
}
