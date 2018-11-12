using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Tiled.MapStructure.Objects;

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
			map.Name = input.AssetName;
			Console.WriteLine(map.Name);
			map.BackgroundColor = input.ReadObject<Color?>();
			map.Width = input.ReadInt32();
			map.Height = input.ReadInt32();
			map.TileWidth = input.ReadInt32();
			map.TileHeight = input.ReadInt32();

			map.RenderOrder = (RenderOrder)input.ReadByte();
			map.Orientation = (Orientation)input.ReadByte();

			map.StaggerAxis = (StaggerAxis)input.ReadByte();
			map.StaggerIndex = (StaggerIndex)input.ReadByte();

			map.HexSideLength = input.ReadInt32();

			ReadTilesets(input, map);
			ReadTileLayers(input, map);
			ReadObjectLayers(input, map);
			ReadImageLayers(input, map);

			map.Properties = input.ReadObject<Dictionary<string, string>>();
			return map;
		}

		#region Tilesets.

		void ReadTilesets(ContentReader input, TiledMap map)
		{
			var tilesetsCount = input.ReadInt32();
			Console.WriteLine("Tilesets: " + tilesetsCount);
			var tilesets = new TiledMapTileset[tilesetsCount];

			for(var i = 0; i < tilesetsCount; i += 1)
			{
				tilesets[i] = new TiledMapTileset();

				tilesets[i].Name = input.ReadString();
				tilesets[i].TexturePaths = input.ReadObject<string[]>();

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

		#endregion Tilesets.


		void ReadLayer(ContentReader input, TiledMapLayer layer)
		{
			layer.Name = input.ReadString();
			layer.ID = input.ReadInt32();
			layer.Visible = input.ReadBoolean();
			layer.Opacity = input.ReadSingle();
			layer.Offset = input.ReadVector2();

			layer.Properties = input.ReadObject<Dictionary<string, string>>();
		}


		#region Tiles.

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

		TiledMapTile ReadTile(ContentReader input)
		{
			var tile = new TiledMapTile();
			tile.GID = input.ReadInt32();
			tile.FlipHor = input.ReadBoolean();
			tile.FlipVer = input.ReadBoolean();
			tile.FlipDiag = input.ReadBoolean();

			return tile;
		}

		#endregion Tiles.

		
		#region Objects.

		void ReadObjectLayers(ContentReader input, TiledMap map)
		{
			var layersCount = input.ReadInt32();
			var layers = new TiledMapObjectLayer[layersCount];
			
			for(var i = 0; i < layersCount; i += 1)
			{
				var layer = new TiledMapObjectLayer();
				ReadLayer(input, layer);

				layer.DrawingOrder = (TiledMapObjectDrawingOrder)input.ReadByte();
				layer.Color = input.ReadColor();

				var objectsCount = input.ReadInt32();
				var objects = new TiledObject[objectsCount];

				for(var k = 0; k < objectsCount; k += 1)
				{
					objects[k] = ReadObject(input);
				}

				layer.Objects = objects;

				layers[i] = layer;
			}

			map.ObjectLayers = layers;
		}


		TiledObject ReadObject(ContentReader input)
		{
			var obj = ReadBaseObject(input);
			var objType = (TiledObjectType)input.ReadByte();

			if (objType == TiledObjectType.Tile)
			{
				return ReadTileObject(input, obj);
			}

			if (objType == TiledObjectType.Point)
			{
				return ReadPointObject(obj);
			}

			if (objType == TiledObjectType.Polygon)
			{
				return ReadPolygonObject(input, obj);
			}

			if (objType == TiledObjectType.Ellipse)
			{
				return ReadEllipseObject(obj);
			}

			if (objType == TiledObjectType.Text)
			{
				return ReadTextObject(input, obj);
			}

			if (objType == TiledObjectType.Rectangle)
			{
				return ReadRectangleObject(obj);
			}
			
			return obj;
		}


		TiledObject ReadBaseObject(ContentReader input)
		{
			var obj = new TiledObject();

			obj.Name = input.ReadString();
			obj.Type = input.ReadString();
			obj.ID = input.ReadInt32();
			obj.Position = input.ReadVector2();
			obj.Size = input.ReadVector2();
			obj.Rotation = input.ReadSingle();
			obj.Visible = input.ReadBoolean();
			obj.Properties = input.ReadObject<Dictionary<string, string>>();

			return obj;
		}

		TiledObject ReadTileObject(ContentReader input, TiledObject baseObj)
		{
			var obj = new TiledTileObject(baseObj);

			obj.GID = input.ReadInt32();
			obj.FlipHor = input.ReadBoolean();
			obj.FlipVer = input.ReadBoolean();

			return obj;
		}
		
		TiledObject ReadPointObject(TiledObject baseObj) =>
			new TiledPointObject(baseObj);
		
		TiledObject ReadPolygonObject(ContentReader input, TiledObject baseObj)
		{
			var obj = new TiledPolygonObject(baseObj);

			obj.Closed = input.ReadBoolean();
			obj.Points = input.ReadObject<Vector2[]>();

			return obj;
		}

		TiledObject ReadEllipseObject(TiledObject baseObj) =>
			new TiledEllipseObject(baseObj);
		
		TiledObject ReadTextObject(ContentReader input, TiledObject baseObj)
		{
			var obj = new TiledTextObject(baseObj);

			obj.Text = input.ReadString();
			obj.Color = input.ReadColor();
			obj.WordWrap = input.ReadBoolean();
			obj.HorAlign = (TiledTextAlign)input.ReadByte();
			obj.VerAlign = (TiledTextAlign)input.ReadByte();
			obj.Font = input.ReadString();
			obj.FontSize = input.ReadInt32();
			obj.Underlined = input.ReadBoolean();
			obj.StrikedOut = input.ReadBoolean();

			return obj;
		}


		TiledObject ReadRectangleObject(TiledObject baseObj) =>
			new TiledRectangleObject(baseObj);

		#endregion Objects.
		
		
		#region Images.
		
		void ReadImageLayers(ContentReader input, TiledMap map)
		{
			var layersCount = input.ReadInt32();
			var layers = new TiledMapImageLayer[layersCount];
			
			for(var i = 0; i < layersCount; i += 1)
			{
				var layer = new TiledMapImageLayer();
				ReadLayer(input, layer);

				layer.TexturePath = input.ReadString();
				layer.Texture = input.ReadExternalReference<Texture2D>();	
				layer.TransparentColor = input.ReadColor();

				layers[i] = layer;
			}

			map.ImageLayers = layers;
		}
		
		#endregion Images.

	}
}
