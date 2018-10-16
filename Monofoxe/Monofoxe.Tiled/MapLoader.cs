using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Utils.Tilemaps;


namespace Monofoxe.Tiled
{
	public class MapLoader
	{
		public static Tilemap LoadMap(TiledMap map)
		{
			var scene = SceneMgr.CreateScene(map.Name);

			var tilesets =  ConvertTilesets(map.Tilesets);//scene;

			foreach(var layer in map.TileLayers)
			{
				var tilemap = new Tilemap((uint)layer.Width, (uint)layer.Height, (uint)layer.TileWidth, (uint)layer.TileHeight);
				for(var y = 0; y < tilemap.Height; y += 1)	
				{
					for(var x = 0; x < tilemap.Width; x += 1)
					{		
						var tileNum = y * tilemap.Width + x;

						var tileIndex = layer.Tiles[(int)tileNum].GlobalIdentifier;

						tilemap.SetTile(x, y, new BasicTile((uint)tileIndex, GetTilesetFromIndex(tileIndex, tilesets)));
					}
				}
				return tilemap;
			}

			return null;
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
				Console.WriteLine("MARGIN:" + tileset.Margin + " " + tileset.Name);
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

						var frame = new Frame(tileset.Texture, texPos, Vector2.Zero, tileset.TileWidth, tileset.TileHeight);

						frames.Add(frame);
					}
				}

				var tiles = new Sprite(frames.ToArray(), Vector2.Zero);
				// Creating sprite from raw texture.

				convertedTilesets.Add(new Tileset(tiles, (uint)tileset.FirstGlobalIdentifier));
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
