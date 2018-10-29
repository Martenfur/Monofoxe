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
			ReadTileLayers(input, map);

			return map;
		}

		void ReadTilesets(ContentReader input, TiledMap map)
		{
			var tilesetsCount = input.ReadInt32();
			Console.WriteLine("Tilesets: " + tilesetsCount);
			var tilesets = new TiledMapTileset[tilesetsCount];

			for(var i = 0; i < tilesetsCount; i += 1)
			{
				tilesets[i] = new TiledMapTileset();

				tilesets[i].Name = input.ReadString();
				//tilesets[i].TexturePaths = input.ReadObject<string[]>(); // TODO: Remove paths.
				if (input.ReadBoolean())
				{
					var texturesCount = input.ReadInt32();
					tilesets[i].Textures = new Texture2D[texturesCount];
					for(var k = 0; k < texturesCount; k += 1)
					{
						tilesets[i].Textures[k] = input.ReadExternalReference<Texture2D>();
					}
				}

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
		
		TiledMapTile ReadTile(ContentReader input)
		{
			var tile = new TiledMapTile();
			tile.GID = input.ReadInt32();
			tile.FlipHor = input.ReadBoolean();
			tile.FlipVer = input.ReadBoolean();
			tile.FlipDiag = input.ReadBoolean();

			return tile;
		}



		void ReadLayer(ContentReader input, TiledMapLayer layer)
		{
			layer.Name = input.ReadString();
			layer.ID = input.ReadInt32();
			layer.Visible = input.ReadBoolean();
			layer.Opacity = input.ReadSingle();
			layer.Offset = input.ReadVector2();

			layer.Properties = input.ReadObject<Dictionary<string, string>>();
		}

		void ReadTileLayers(ContentReader input, TiledMap map)
		{
			var layersCount = input.ReadInt32();
			var layers = new TiledMapTileLayer[layersCount];

			for(var i = 0; i < layersCount; i += 1)
			{
				var layer = new TiledMapTileLayer();
				ReadLayer(input, layer);
				layer.Width = input.ReadInt32();
				layer.Height = input.ReadInt32();
				layer.TileWidth = input.ReadInt32();
				layer.TileHeight = input.ReadInt32(); //TODO: Remove!
				layer.TileWidth = map.TileWidth;
				layer.TileHeight = map.TileHeight;
			
				var tiles = new TiledMapTile[layer.Width][];
				for(var x = 0; x < layer.Width; x += 1)
				{
					tiles[x] = new TiledMapTile[layer.Height];
				}
				for(var y = 0; y < layer.Height; y += 1)
				{
					for(var x = 0; x < layer.Width; x += 1)
					{
						tiles[x][y] = ReadTile(input);
					}
				}
				layer.Tiles = tiles;

				layers[i] = layer;
			}
			map.TileLayers = layers;
		}

	}
}
