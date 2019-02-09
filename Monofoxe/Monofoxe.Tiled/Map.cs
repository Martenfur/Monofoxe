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
	/// <summary>
	/// Basic map class. Creates a map from Tiled data structures.
	/// Can be extended.
	/// </summary>
	public class Map
	{
		public readonly TiledMap TiledMap;
		public Scene MapScene {get; protected set;}
		public bool Loaded {get; protected set;} = false;


		public Map(TiledMap tiledMap) =>
			TiledMap = tiledMap;
		
		/// <summary>
		/// Loads map scene.
		/// </summary>
		public virtual void Load()
		{
			if (!Loaded)
			{
				MapScene = SceneMgr.CreateScene(TiledMap.Name);
			
				var tilesets = BuildTilesets(TiledMap.Tilesets);

				BuildTileLayers(tilesets);
				BuildObjectLayers();
				BuildImageLayers();

				Loaded = true;
			}
		}

		
		/// <summary>
		/// Unloads map scene.
		/// </summary>
		public virtual void Unload()
		{
			SceneMgr.DestroyScene(MapScene);
			MapScene = null;

			Loaded = false;
		}



		#region Map building.

		/// <summary>
		/// Builds Tiled tilesets to actual tilesets.
		/// </summary>
		protected virtual List<Tileset> BuildTilesets(TiledMapTileset[] tilesets)
		{
			var convertedTilesets = new List<Tileset>();

			foreach(var tileset in tilesets)
			{
				// Creating sprite from raw texture.
				List<ITilesetTile> tilesetTilesList = new List<ITilesetTile>();
				
				if (tileset.Textures != null)
				{
					var isMultitexture = (tileset.Textures.Length > 1);
					
					for(var y = 0; y < tileset.Height; y += 1)
					{
						for(var x = 0; x < tileset.Width; x += 1)
						{
							var tile = tileset.Tiles[y * tileset.Width + x];

							var tileTexture = tileset.Textures[tile.TextureID];
							int tileW, tileH;

							/*
							 * TileWidth and TileHeight values are incorrect for multitexture tilesets. 
							 * They contain max width and height of all textures.
							*/
							if (isMultitexture)
							{
								tileW = tileTexture.Width;
								tileH = tileTexture.Height;
							}
							else
							{
								tileW = tileset.TileWidth;
								tileH = tileset.TileHeight;
							}

							var frame = new Frame(tileTexture, tile.TexturePosition, Vector2.Zero, tileW, tileH);
						
							var tilesetTile = new BasicTilesetTile(frame);
							tilesetTilesList.Add(tilesetTile);
						}
					}
					
					// Tileset origins in Tiled are in the left bottom corner. Derp.
				}
				// Creating sprite from raw texture.

				var finalTileset = new Tileset(tilesetTilesList.ToArray(), tileset.Offset, tileset.FirstGID);
				convertedTilesets.Add(finalTileset);
			}

			return convertedTilesets;
		}

		protected Tileset GetTilesetFromTileIndex(int index, List<Tileset> tilesets)
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



		protected virtual void BuildTileLayers(List<Tileset> tilesets)
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
								GetTilesetFromTileIndex(tileIndex, tilesets),
								tileLayer.Tiles[x][y].FlipHor,
								tileLayer.Tiles[x][y].FlipVer,
								tileLayer.Tiles[x][y].FlipDiag
							)
						);
					}
				}
				
				var tilemapEntity = new Entity(layer, "BasicTilemap");
				tilemapEntity.AddComponent(tilemap);
			}
		}
		


		protected virtual void BuildObjectLayers()
		{
			foreach(var objectLayer in TiledMap.ObjectLayers)
			{
				var layer = MapScene.CreateLayer(objectLayer.Name);
				layer.Priority = GetLayerPriority(objectLayer);

				foreach(var obj in objectLayer.Objects)
				{
					MapMgr.MakeEntity(obj, layer);
				}
			}
		}



		protected virtual void BuildImageLayers()
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

		#endregion Map building.

	}
}
