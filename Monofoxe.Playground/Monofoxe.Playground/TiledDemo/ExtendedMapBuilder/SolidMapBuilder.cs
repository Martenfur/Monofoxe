using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Tilemaps;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Tiled.MapStructure.Objects;

namespace Monofoxe.Playground.TiledDemo.ExtendedMapBuilder
{
	/// <summary>
	/// Custom map builder. Adds tilemap collision functionality to regular tilemap.
	/// </summary>
	public class SolidMapBuilder : MapBuilder
	{
		private const string _typeProperty = "type";
		private const string _rectangleName = "rectangle";
		private const string _platformName = "platform";


		public SolidMapBuilder(TiledMap tiledMap) : base(tiledMap) { }

		public override void Build()
		{
			base.Build();
		}

		protected override List<Tileset> BuildTilesets(TiledMapTileset[] tilesets)
		{
			// Letting basic tileset builder do its stuff.
			var convertedBasicTilesets = base.BuildTilesets(tilesets);

			var convertedColliderTilesets = new List<Tileset>();

			// Now we got tilesets with basic tiles, which we need to convert into collider tile.

			for (var i = 0; i < convertedBasicTilesets.Count; i += 1)
			{
				var basicTileset = convertedBasicTilesets[i];

				// Essentially cloning a tileset with new set of converted tiles.
				// Goal here is to make tilemap work with collision system. 
				// All collider data is stored in tileset tiles, which are later assigned to tilemap tiles.
				// To see collision data, we just need ti take tilemap tile and look at its assigned tileset tile. 

				var colliderTileset = new Tileset(
					ConvertTiles(tilesets[i], convertedBasicTilesets[i]), // All the magic happens here.
					basicTileset.Offset,
					basicTileset.StartingIndex
				);
				convertedColliderTilesets.Add(colliderTileset);
			}

			return convertedColliderTilesets;
		}

		/*
		protected override List<Layer> BuildTileLayers(List<Tileset> tilesets)
		{
			// Letting basic layer builder do its stuff.
			var layers = base.BuildTileLayers(tilesets);

			// Now we need to add position and collider components to entity to make it count as a solid.
			foreach (var layer in layers)
			{
				// Getting list of all tilemaps on this layer.
				var tilemaps = layer.GetEntityListByComponent<BasicTilemapComponent>();

				foreach (var tilemap in tilemaps)
				{
					var tilemapComponent = tilemap.GetComponent<BasicTilemapComponent>();

					tilemapComponent.Padding = 3; // Padding is increased, so biffer tiles like trees won't disappear while still on screen.
					
				}
			}

			return layers;
		}*/



		/// <summary>
		/// Converts basic tilesets into collider tilesets using data from Tiled structures.
		/// </summary>
		ITilesetTile[] ConvertTiles(TiledMapTileset tiledTileset, Tileset tileset)
		{
			var tilesetTiles = new List<ITilesetTile>();

			for (var i = 0; i < tileset.Tiles.Length; i += 1)
			{
				var tiledTile = tiledTileset.Tiles[i];

				// Getting collision mode of a tile.
				var solid = false;
				try
				{
					solid = bool.Parse(tiledTile.Properties["Solid"]);
					Console.WriteLine("MODE: " + solid);
				}
				catch (Exception) {}
				// Getting collision mode of a tile.

				SolidTilesetTile tilesetTile;

				tilesetTile = new SolidTilesetTile(tileset.Tiles[i].Frame, solid);
				
				tilesetTiles.Add(tilesetTile);
			}

			return tilesetTiles.ToArray();
		}
		
	}
}
