using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Tilemaps;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure;
using System;
using System.Collections.Generic;

namespace Monofoxe.Samples.Misc.Tiled
{
	/// <summary>
	/// Custom map builder. Adds tilemap collision functionality to regular tilemap.
	/// You can use just MapBuilder as is, if you don't want that.
	/// </summary>
	public class SolidMapBuilder : MapBuilder
	{
		
		public SolidMapBuilder(TiledMap tiledMap) : base(tiledMap) {}

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
			for(var i = 0; i < convertedBasicTilesets.Count; i += 1)
			{
				var basicTileset = convertedBasicTilesets[i];

				// Essentially cloning a tileset with new set of converted tiles.
				// Goal here is to make tilemap work with collision system. 
				// All collider data is stored in tileset tiles, which are later assigned to tilemap tiles.
				// To see collision data, we just need to take tilemap tile and look at its assigned tileset tile. 

				var colliderTileset = new Tileset(
					ConvertTiles(tilesets[i], convertedBasicTilesets[i]), // All the magic happens here.
					basicTileset.Offset,
					basicTileset.StartingIndex
				);
				convertedColliderTilesets.Add(colliderTileset);
			}
			
			return convertedColliderTilesets;
		}


		/// <summary>
		/// Converts basic tilesets into collider tilesets using data from Tiled structures.
		/// </summary>
		ITilesetTile[] ConvertTiles(TiledMapTileset tiledTileset, Tileset tileset)
		{
			var tilesetTiles = new List<ITilesetTile>();

			for (var i = 0; i < tileset.Tiles.Length; i += 1)
			{
				var tiledTile = tiledTileset.Tiles[i];

				// Getting solid property from the tile.
				var solid = false;
				try
				{
					solid = bool.Parse(tiledTile.Properties["Solid"]);
				}
				catch (Exception) {}
				// Getting solid property from the tile.

				SolidTilesetTile tilesetTile;

				tilesetTile = new SolidTilesetTile(tileset.Tiles[i].Frame, solid);
				
				tilesetTiles.Add(tilesetTile);
			}

			return tilesetTiles.ToArray();
		}
		
	}
}
