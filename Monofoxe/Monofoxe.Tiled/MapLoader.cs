using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.ECS;
using Monofoxe.Utils.Tilemaps;


namespace Monofoxe.Tiled
{
	public class MapLoader
	{
		public static Scene LoadMap(TiledMap map)
		{
			var scene = SceneMgr.CreateScene(map.Name);
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

						var tileIndex = tileLayer.Tiles[tileNum].GlobalIdentifier;
						if (x == 0 && y == 0)
						{
							Console.WriteLine("INDEX:" + GetTilesetFromIndex(tileIndex, tilesets).StartingIndex + " " + tileIndex);
						}

						tilemap.SetTile(
							x, y, 
							new BasicTile(
								tileIndex, 
								GetTilesetFromIndex(tileIndex, tilesets),
								tileLayer.Tiles[tileNum].IsFlippedHorizontally,
								tileLayer.Tiles[tileNum].IsFlippedVertically
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

		static List<Tileset> ConvertTilesets(ReadOnlyCollection<TiledMapTileset> tilesets)
		{
			var convertedTilesets = new List<Tileset>();

			foreach(var tileset in tilesets)
			{
				// Creating sprite from raw texture.
				var framesW = tileset.Columns;
				var framesH = tileset.TileCount / tileset.Columns;
				
				var frames = new List<Frame>();
				Console.WriteLine("MARGIN:" + tileset.Margin + " " + tileset.Name + " " + framesW + ";" + framesH);
				for(var y = 0; y < framesH; y += 1)	
				{
					for(var x = 0; x < framesW; x += 1)
					{		
						var texPos = new Rectangle(
							tileset.Margin + x * (tileset.TileWidth + tileset.Spacing), 
							tileset.Margin + y * (tileset.TileHeight + tileset.Spacing), 
							tileset.TileWidth, 
							tileset.TileHeight
						);
						Console.WriteLine(texPos + " " + tileset.TileWidth + " " + tileset.TileHeight);
						var frame = new Frame(tileset.Texture, texPos, Vector2.Zero, tileset.TileWidth, tileset.TileHeight);

						frames.Add(frame);
					}
				}

				var tiles = new Sprite(frames.ToArray(), Vector2.Zero);
				tiles.Origin = Vector2.UnitY * tiles.H; // Tileset origins in Tiled are in the left bottom corner. Derp.
				// Creating sprite from raw texture.

				convertedTilesets.Add(new Tileset(tiles, tileset.FirstGlobalIdentifier));
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
