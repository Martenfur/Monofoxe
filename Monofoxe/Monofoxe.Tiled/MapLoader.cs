using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Utils.Tilemaps;


namespace Monofoxe.Tiled
{
	public class MapLoader
	{
		
		
		public static Scene LoadMap(TiledMap map)
		{
			var scene = SceneMgr.CreateScene("New map");//map.Name);
			//map.ObjectLayers
			var tilesets = ConvertTilesets(map.Tilesets);

			// TODO: Add image layer support? I guess?

			foreach(var tileLayer in map.TileLayers)
			{
				var layer = scene.CreateLayer(tileLayer.Name);
				try
				{
					layer.Priority = int.Parse(tileLayer.Properties["priority"]);
				}
				catch(Exception)
				{
					layer.Priority = 0;
				}
				Console.WriteLine(layer.Priority);

				var tilemap = new BasicTilemapComponent(tileLayer.Width, tileLayer.Height, tileLayer.TileWidth, tileLayer.TileHeight);
				for(var y = 0; y < tilemap.Height; y += 1)	
				{
					for(var x = 0; x < tilemap.Width; x += 1)
					{		
						var tileNum = y * tilemap.Width + x;

						var tileIndex = tileLayer.Tiles[x][y].GID;

						tilemap.SetTile(
							x, y, 
							new BasicTile(
								tileIndex, 
								GetTilesetFromIndex(tileIndex, tilesets),
								tileLayer.Tiles[x][y].FlipHor,
								tileLayer.Tiles[x][y].FlipVer,
								tileLayer.Tiles[x][y].FlipDiag
							)
						);
					}
				}
				//return tilemap;

				var tilemapEntity = new Entity(layer, tilemap.Tag);
				tilemapEntity.AddComponent(tilemap);
				//tilemap.Offset = Vector2.One * 32;
			}

			return scene;
		}

		static List<Tileset> ConvertTilesets(TiledMapTileset[] tilesets)
		{
			var convertedTilesets = new List<Tileset>();

			foreach(var tileset in tilesets)
			{
				// Creating sprite from raw texture.
				var frames = new List<Frame>();
				for(var y = 0; y < tileset.Height; y += 1)
				{
					for(var x = 0; x < tileset.Width; x += 1)
					{		
						var tile = tileset.Tiles[y * tileset.Width + x];
						var frame = new Frame(tileset.Textures[tile.TextureID], tile.TexturePosition, Vector2.Zero, tileset.TileWidth, tileset.TileHeight);
						
						frames.Add(frame);
					}
				}

				var tiles = new Sprite(frames.ToArray(), Vector2.Zero);
				tiles.Origin = Vector2.UnitY * tiles.H; // Tileset origins in Tiled are in the left bottom corner. Derp.
				// Creating sprite from raw texture.

				convertedTilesets.Add(new Tileset(tiles, tileset.FirstGID));
			}

			return convertedTilesets;
		}

		static Tileset GetTilesetFromIndex(int index, List<Tileset> tilesets)
		{
			foreach(var tileset in tilesets)
			{
				if (index >= tileset.StartingIndex && index < tileset.StartingIndex + tileset.Count)
				{
					return tileset;
				}
			}

			return null;
		}
	}
}
