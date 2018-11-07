using System;
using System.Collections.Generic;
using Monofoxe.Tiled.MapStructure;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Tiled.MapStructure.Objects;

namespace Pipefoxe.Tiled
{
	static class LayerParser
	{
		static List<TiledMapTileLayer> _tileLayers;
		static List<TiledMapObjectLayer> _objectLayers;
		
		// TODO: Add image layers.

		public static void Parse(XmlNode mapXml, TiledMap map)
		{
			_tileLayers = new List<TiledMapTileLayer>();
			_objectLayers = new List<TiledMapObjectLayer>();

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

			var objectLayers = groupXml.SelectNodes("objectgroup");
			foreach(XmlNode layer in objectLayers)
			{
				_objectLayers.Add(ParseObjectLayer(layer));
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

			// Initing tile array.
			/*
			 * Pipeline cannot work with 2-dimensional arrays,
			 * so we're stuck arrays of arrays.
			 */
			var tiles = new TiledMapTile[layer.Width][];
			for(var x = 0; x < layer.Width; x += 1)
			{
				tiles[x] = new TiledMapTile[layer.Height];
			}
			// Initing tile array.

			// Filling tilemap with tiles.
			for(var y = 0; y < layer.Height; y += 1)
			{
				for(var x = 0; x < layer.Width; x += 1)
				{
					tiles[x][y] = new TiledMapTile();
					var tilemapValue = tilemapValues[y * layer.Width + x];

					// Tile flip flags are stored in the tile value itself as 3 highest bits.
					tiles[x][y].FlipHor = ((tilemapValue & (uint)FlipFlags.FlipHor) != 0);
					tiles[x][y].FlipVer = ((tilemapValue & (uint)FlipFlags.FlipVer) != 0);
					tiles[x][y].FlipDiag = ((tilemapValue & (uint)FlipFlags.FlipDiag) != 0);
					tiles[x][y].GID = (int)(tilemapValue & (~(uint)FlipFlags.All));
				}
			}
			// Filling tilemap with tiles.

			layer.Tiles = tiles;

			layer.Properties = TiledMapImporter.GetProperties(layerXml);

			return layer;
		}



		static TiledMapObjectLayer ParseObjectLayer(XmlNode layerXml)
		{
			var layer = new TiledMapObjectLayer();
			
			layer.ID = int.Parse(layerXml.Attributes["id"].Value);
			layer.Name = layerXml.Attributes["name"].Value;
			layer.Opacity = TiledMapImporter.GetXmlFloatSafe(layerXml, "opacity");
			layer.Visible = TiledMapImporter.GetXmlBoolSafe(layerXml, "visible");
			layer.Offset = new Vector2(
				TiledMapImporter.GetXmlFloatSafe(layerXml, "offsetx"),
				TiledMapImporter.GetXmlFloatSafe(layerXml, "offsety")
			);

			var objectsXml = layerXml.SelectNodes("object");

			foreach(XmlNode obj in objectsXml)
			{
				ParseObject(obj);
			}

			return layer;
		}

		static TiledObject ParseObject(XmlNode node)
		{
			var obj = new TiledObject();

			if (node.Attributes["template"] == null)
			{
				obj.Name = TiledMapImporter.GetXmlStringSafe(node, "name");
				obj.Position = new Vector2(
					TiledMapImporter.GetXmlFloatSafe(node, "x"),
					TiledMapImporter.GetXmlFloatSafe(node, "y")
				);
			}
			else
			{
				MergeWithTemplate(node);
			}

			return obj;
		}

		static XmlNode MergeWithTemplate(XmlNode node)
		{
			// Loading template.
			var doc = new XmlDocument();
			XmlNode template;
			try
			{
				doc.Load(TiledMapImporter.RootDir + node.Attributes["template"].Value);
				template = doc["template"]["object"];
			}
			catch(Exception e)
			{
				throw new Exception("Error loading object template! " + e.StackTrace);
			}
			// Loading template.


			/*
			 * So, now we need to read the template and take attributes, 
			 * which are not present in current object.
			 */
			
			var owner = node.OwnerDocument;
			
			foreach(XmlAttribute attribute in template.Attributes)
			{
				if (node.Attributes[attribute.Name] == null)
				{
					var newAttr = owner.CreateAttribute(attribute.Name);
					newAttr.Value = attribute.Value;
					node.Attributes.Append(newAttr);
				}
			}
			node.Attributes.RemoveNamedItem("template");
			foreach(XmlNode child in template.ChildNodes)
			{
				if (node[child.Name] == null)
				{
					var newChild = owner.CreateElement(child.Name);
					newChild.InnerText = child.InnerText;
					newChild.InnerXml = child.InnerXml;
					foreach(XmlAttribute attribute in template[child.Name].Attributes)
					{
						var newAttr = owner.CreateAttribute(attribute.Name);
						newAttr.Value = attribute.Value;
						newChild.Attributes.Append(newAttr);
					}
					node.AppendChild(newChild);
				}
			}

			TiledMapImporter.__Log(node.OuterXml);

			return node;
		}

		
	}
}
