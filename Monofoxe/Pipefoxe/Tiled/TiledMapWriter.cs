using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Monofoxe.Tiled.MapStructure;


namespace Pipefoxe.Tiled
{
	[ContentTypeWriter]
	public class TiledMapWriter : ContentTypeWriter<TiledMap>
	{
		protected override void Write(ContentWriter output, TiledMap map)
		{
			output.WriteObject(map.BackgroundColor);
			output.Write(map.Width);
			output.Write(map.Height);
			output.Write(map.TileWidth);
			output.Write(map.TileHeight);

			output.Write((byte)map.RenderOrder);
			output.Write((byte)map.Orientation);

			output.Write((byte)map.StaggerAxis);
			output.Write((byte)map.StaggerIndex);

			output.Write(map.HexSideLength);

			WriteTilesets(output, map.Tilesets);
			WriteTileLayers(output, map.TileLayers);
			
			output.WriteObject(map.Properties);
		}

		

		void WriteTilesets(ContentWriter output, TiledMapTileset[] tilesets)
		{
			output.Write(tilesets.Length);
			foreach(var tileset in tilesets)
			{
				output.Write(tileset.Name);
				output.WriteObject(tileset.TexturePaths);

				if (
					tileset.Properties.ContainsKey(TilesetParser.IgnoreTilesetTextureFlag) 
					&& tileset.Properties[TilesetParser.IgnoreTilesetTextureFlag].ToLower() == "true"
				)
				{
					output.Write(false);
				}
				else
				{
					output.Write(true);
					output.Write(tileset.TexturePaths.Length);
					for(var i = 0; i < tileset.TexturePaths.Length; i += 1)
					{
						var externalReference = TiledMapProcessor.TextureReferences[tileset.TexturePaths[i]];
						output.WriteExternalReference(externalReference);
					}
				}

				output.Write(tileset.FirstGID);
				output.Write(tileset.TileWidth);
				output.Write(tileset.TileHeight);
				output.Write(tileset.Spacing);
				output.Write(tileset.Margin);
				output.Write(tileset.TileCount);
				output.Write(tileset.Columns);

				output.Write(tileset.Offset);

				foreach(var tile in tileset.Tiles)
				{
					WriteTilesetTile(output, tile);
				}
				
				output.WriteObject(tileset.BackgroundColor);
				output.WriteObject(tileset.Properties);
			}
		}



		void WriteTilesetTile(ContentWriter output, TiledMapTilesetTile tile)
		{
			output.Write(tile.GID);
			output.Write(tile.TextureID);
			output.WriteObject(tile.TexturePosition);
			output.WriteObject(tile.Properties);
		}

		void WriteTile(ContentWriter output, TiledMapTile tile)
		{
			output.Write(tile.GID);
			output.Write(tile.FlipHor);
			output.Write(tile.FlipVer);
			output.Write(tile.FlipDiag);
		}



		void WriteLayer(ContentWriter output, TiledMapLayer layer)
		{
			output.Write(layer.Name);
			output.Write(layer.ID);
			output.Write(layer.Visible);
			output.Write(layer.Opacity);
			output.Write(layer.Offset);
			
			output.WriteObject(layer.Properties);
		}

		void WriteTileLayers(ContentWriter output, TiledMapTileLayer[] layers)
		{
			output.Write(layers.Length);
			foreach(var layer in layers)
			{
				WriteLayer(output, layer);
				output.Write(layer.Width);
				output.Write(layer.Height);
				
				for(var y = 0; y < layer.Height; y += 1)
				{
					for(var x = 0; x < layer.Width; x += 1)
					{
						WriteTile(output, layer.Tiles[x][y]);
					}
				}
			}
		}



		

		public override string GetRuntimeType(TargetPlatform targetPlatform) =>
			"Monofoxe.Tiled.MapStructure.TiledMap, Monofoxe.Tiled";



		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Tiled.ContentReaders.TiledMapReader, Monofoxe.Tiled";
	}
}

