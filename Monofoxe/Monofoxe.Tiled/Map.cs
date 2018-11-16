using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Tilemaps;
using Monofoxe.Tiled.MapStructure;

namespace Monofoxe.Tiled
{
	public class Map
	{
		public readonly TiledMap TiledMap;
		public Scene MapScene {get; protected set;}
		public bool Loaded {get; protected set;} = false;


		public Map(TiledMap tiledMap) =>
			TiledMap = tiledMap;
		
		public virtual void Load()
		{
			MapScene = SceneMgr.CreateScene(TiledMap.Name);
			
			var tilesets = ConvertTilesets(TiledMap.Tilesets);

			LoadTileLayers(tilesets);
			LoadObjectLayers();
			LoadImageLayers();

			Loaded = true;
		}

		public virtual void Unload()
		{
			SceneMgr.DestroyScene(MapScene);
			MapScene = null;

			Loaded = false;
		}


		
		protected virtual List<Tileset> ConvertTilesets(TiledMapTileset[] tilesets)
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

		protected Tileset GetTilesetFromIndex(int index, List<Tileset> tilesets)
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



		protected virtual void LoadTileLayers(List<Tileset> tilesets)
		{
			foreach(var tileLayer in TiledMap.TileLayers)
			{
				var layer = MapScene.CreateLayer(tileLayer.Name);
				layer.Priority = GetLayerPriority(tileLayer);
				
				var tilemap = new BasicTilemapComponent(tileLayer.Width, tileLayer.Height, tileLayer.TileWidth, tileLayer.TileHeight);
				for(var y = 0; y < tilemap.Height; y += 1)	
				{
					for(var x = 0; x < tilemap.Width; x += 1)
					{
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
				
				var tilemapEntity = new Entity(layer, tilemap.Tag);
				tilemapEntity.AddComponent(tilemap);
			}
		}
		


		protected virtual void LoadObjectLayers()
		{
			foreach(var objectLayer in TiledMap.ObjectLayers)
			{
				var layer = MapScene.CreateLayer(objectLayer.Name);
				layer.Priority = GetLayerPriority(objectLayer);

				foreach(var obj in objectLayer.Objects)
				{
					Console.WriteLine("Making " + obj.Name);
					MapMgr.MakeEntity(obj, layer);
				}
			}
		}



		protected virtual void LoadImageLayers()
		{
			foreach(var imageLayer in TiledMap.ImageLayers)
			{
				var layer = MapScene.CreateLayer(imageLayer.Name);
				var entity = new Entity(layer, "tiledImage");
				var frame = new Frame(
					imageLayer.Texture, 
					imageLayer.Texture.Bounds, 
					Vector2.Zero, 
					imageLayer.Texture.Bounds.Width, 
					imageLayer.Texture.Bounds.Height
				);
				entity.AddComponent(new ImageLayerComponent(imageLayer.Offset, frame));
			}
		}



		/// <summary>
		/// Returns Tiled layer priority, which is stored in its properties.
		/// If no such property was found, returns 0.
		/// </summary>
		protected int GetLayerPriority(TiledMapLayer layer)
		{
			try
			{
				return int.Parse(layer.Properties["priority"]);
			}
			catch(Exception)
			{
				return 0;
			}
		}

		

	}
}
