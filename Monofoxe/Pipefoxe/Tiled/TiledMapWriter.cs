using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Tiled.MapStructure.Objects;

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
			WriteObjectLayers(output, map.ObjectLayers);
			WriteImageLayers(output, map.ImageLayers);

			output.WriteObject(map.Properties);
		}

		#region Tilesets.

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
			
			output.Write((byte)tile.ObjectsDrawingOrder);
			
			output.Write(tile.Objects.Length);
			foreach(var obj in tile.Objects)
			{
				WriteObject(output, obj);
			}

			output.WriteObject(tile.Properties);
		}

		
		#endregion Tilesets.


		void WriteLayer(ContentWriter output, TiledMapLayer layer)
		{
			output.Write(layer.Name);
			output.Write(layer.ID);
			output.Write(layer.Visible);
			output.Write(layer.Opacity);
			output.Write(layer.Offset);
			
			output.WriteObject(layer.Properties);
		}


		#region Tiles.

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



		void WriteTile(ContentWriter output, TiledMapTile tile)
		{
			output.Write(tile.GID);
			output.Write(tile.FlipHor);
			output.Write(tile.FlipVer);
			output.Write(tile.FlipDiag);
		}

		#endregion Tiles.



		#region Objects.

		void WriteObjectLayers(ContentWriter output, TiledMapObjectLayer[] layers)
		{
			output.Write(layers.Length);
			foreach(var layer in layers)
			{
				WriteLayer(output, layer);
				output.Write((byte)layer.DrawingOrder);
				output.Write(layer.Color);

				output.Write(layer.Objects.Length);
				foreach(var obj in layer.Objects)
				{
					WriteObject(output, obj);
				}
			}
		}

		void WriteObject(ContentWriter output, TiledObject obj)
		{
			WriteBaseObject(output, obj);
			if (obj is TiledTileObject)
			{
				WriteTileObject(output, (TiledTileObject)obj);
				return;
			}
			
			if (obj is TiledPointObject)
			{
				WritePointObject(output);
				return;
			}

			if (obj is TiledPolygonObject)
			{
				WritePolygonObject(output, (TiledPolygonObject)obj);
				return;
			}

			if (obj is TiledEllipseObject)
			{
				WriteEllipseObject(output);
				return;
			}

			if (obj is TiledTextObject)
			{
				WriteTextObject(output, (TiledTextObject)obj);
				return;
			}

			if (obj is TiledRectangleObject)
			{
				WriteRectangleObject(output);
			}
		}


		void WriteBaseObject(ContentWriter output, TiledObject obj)
		{
			output.Write(obj.Name);
			output.Write(obj.Type);
			output.Write(obj.ID);
			output.Write(obj.Position);
			output.Write(obj.Size);
			output.Write(obj.Rotation);
			output.Write(obj.Visible);
			output.WriteObject(obj.Properties);
		}
		
		void WriteTileObject(ContentWriter output, TiledTileObject obj)
		{
			output.Write((byte)TiledObjectType.Tile);
			output.Write(obj.GID);
			output.Write(obj.FlipHor);
			output.Write(obj.FlipVer);
		}

		void WritePointObject(ContentWriter output) =>
			output.Write((byte)TiledObjectType.Point);

		void WritePolygonObject(ContentWriter output, TiledPolygonObject obj)
		{
			output.Write((byte)TiledObjectType.Polygon);
			output.Write(obj.Closed);
			output.WriteObject(obj.Points);
		}

		void WriteEllipseObject(ContentWriter output) =>
			output.Write((byte)TiledObjectType.Ellipse);
		
		void WriteTextObject(ContentWriter output, TiledTextObject obj)
		{
			output.Write((byte)TiledObjectType.Text);
			output.Write(obj.Text);
			output.Write(obj.Color);
			output.Write(obj.WordWrap);
			output.Write((byte)obj.HorAlign);
			output.Write((byte)obj.VerAlign);
			output.Write(obj.Font);
			output.Write(obj.FontSize);
			output.Write(obj.Underlined);
			output.Write(obj.StrikedOut);
		}

		void WriteRectangleObject(ContentWriter output) =>
			output.Write((byte)TiledObjectType.Rectangle);

		#endregion Objects.



		#region Images.
		
		void WriteImageLayers(ContentWriter output, TiledMapImageLayer[] layers)
		{
			output.Write(layers.Length);
			foreach(var layer in layers)
			{
				WriteLayer(output, layer);

				output.Write(layer.TexturePath);
				var externalReference = TiledMapProcessor.TextureReferences[layer.TexturePath];
				output.WriteExternalReference(externalReference);
				output.Write(layer.TransparentColor);
			}
		}

		#endregion Images.
		
		
		public override string GetRuntimeType(TargetPlatform targetPlatform) =>
			"Monofoxe.Tiled.MapStructure.TiledMap, Monofoxe.Tiled";



		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Tiled.ContentReaders.TiledMapReader, Monofoxe.Tiled";
	}
}

