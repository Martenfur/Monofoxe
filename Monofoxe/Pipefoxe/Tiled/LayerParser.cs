using System;
using System.Collections.Generic;
using Monofoxe.Tiled.MapStructure;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pipefoxe.Tiled
{
	static class LayerParser
	{
		static List<TiledMapTileLayer> _tileLayers;
		// TODO: Add image and object layers.

		public static void Parse(XmlNode mapXml, TiledMap map)
		{
			_tileLayers = new List<TiledMapTileLayer>();

			ParseGroup(mapXml);

			map.TileLayers = _tileLayers.ToArray();
		}

		static void ParseGroup(XmlNode groupXml)
		{
			var groups = groupXml.SelectNodes("group");
			foreach(XmlNode group in groups)
			{
				ParseGroup(group);
			}

			var tileLayers = groupXml.SelectNodes("layer");
			foreach(XmlNode layer in tileLayers)
			{
				_tileLayers.Add(ParseTileLayer(layer));
			}
		}


		static TiledMapTileLayer ParseTileLayer(XmlNode layerXml)
		{
			var layer = new TiledMapTileLayer();	

			layer.ID = int.Parse(layerXml.Attributes["id"].Value);
			layer.Width = int.Parse(layerXml.Attributes["width"].Value);
			layer.Height = int.Parse(layerXml.Attributes["height"].Value);
			layer.Name = layerXml.Attributes["name"].Value;
			layer.Opacity = TiledMapImporter.GetXmlFloatSafe(layerXml, "opacity");
			layer.Visible = TiledMapImporter.GetXmlBoolSafe(layerXml, "visible");
			layer.Offset = new Vector2(
				TiledMapImporter.GetXmlFloatSafe(layerXml, "offsetx"),
				TiledMapImporter.GetXmlFloatSafe(layerXml, "offsety")
			);

			if (layerXml["data"].Attributes["encoding"].Value != "csv")
			{
				throw new NotSupportedException("Error while parsing layer " + layer.Name + ". Only CSV encoding is supported.");
			}

			// Parsing csv tile values.
			var tilemapValuesStr = layerXml["data"].InnerText.Split(',');
			var tilemapValues = new uint[tilemapValuesStr.Length];

			for(var i = 0; i < tilemapValues.Length; i += 1)
			{
				tilemapValues[i] = uint.Parse(tilemapValuesStr[i]);
			}
			// Parsing csv tile values.

			var tiles = new TiledMapTile[layer.Width][];

			for(var x = 0; x < layer.Width; x += 1)
			{
				tiles[x] = new TiledMapTile[layer.Height];
			}

			for(var y = 0; y < layer.Height; y += 1)
			{
				for(var x = 0; x < layer.Width; x += 1)
				{
					tiles[x][y] = new TiledMapTile();
					var tilemapValue = tilemapValues[y * layer.Width + x];

					tiles[x][y].FlipHor = ((tilemapValue & (uint)FlipFlags.FlipHor) != 0);
					tiles[x][y].FlipVer = ((tilemapValue & (uint)FlipFlags.FlipVer) != 0);
					tiles[x][y].FlipDiag = ((tilemapValue & (uint)FlipFlags.FlipDiag) != 0);
					tiles[x][y].GID = (int)(tilemapValue & (~(uint)FlipFlags.All));
				}
			}

			layer.Tiles = tiles;

			layer.Properties = TiledMapImporter.GetProperties(layerXml);

			return layer;
		}
	}
}
