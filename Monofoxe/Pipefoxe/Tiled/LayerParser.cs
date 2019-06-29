using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Tiled.MapStructure.Objects;
using System.IO;

namespace Pipefoxe.Tiled
{
	static class LayerParser
	{
		static List<TiledMapTileLayer> _tileLayers;
		static List<TiledMapObjectLayer> _objectLayers;
		static List<TiledMapImageLayer> _imageLayers;
		

		
		public static void Parse(XmlNode mapXml, TiledMap map)
		{
			_tileLayers = new List<TiledMapTileLayer>();
			_objectLayers = new List<TiledMapObjectLayer>();
			_imageLayers = new List<TiledMapImageLayer>();

			ParseGroup(mapXml);

			map.TileLayers = _tileLayers.ToArray();
			map.ObjectLayers = _objectLayers.ToArray();
			map.ImageLayers = _imageLayers.ToArray();
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

			var imageLayers = groupXml.SelectNodes("imagelayer");
			foreach(XmlNode layer in imageLayers)
			{
				var parsedLayer = ParseImageLayer(layer);
				if (parsedLayer != null)
				{
					_imageLayers.Add(parsedLayer);
				}
			}
		}


		static void ParseBaseLayer(XmlNode layerXml, TiledMapLayer layer)
		{
			layer.ID = int.Parse(layerXml.Attributes["id"].Value);
			layer.Name = layerXml.Attributes["name"].Value;
			layer.Opacity = XmlHelper.GetXmlFloatSafe(layerXml, "opacity");
			layer.Visible = XmlHelper.GetXmlBoolSafe(layerXml, "visible");
			layer.Offset = new Vector2(
				XmlHelper.GetXmlFloatSafe(layerXml, "offsetx"),
				XmlHelper.GetXmlFloatSafe(layerXml, "offsety")
			);
			layer.Properties = XmlHelper.GetProperties(layerXml);
		}


		static TiledMapTileLayer ParseTileLayer(XmlNode layerXml)
		{
			var layer = new TiledMapTileLayer();	

			ParseBaseLayer(layerXml, layer);

			layer.Width = int.Parse(layerXml.Attributes["width"].Value);
			layer.Height = int.Parse(layerXml.Attributes["height"].Value);


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

			return layer;
		}



		public static TiledMapObjectLayer ParseObjectLayer(XmlNode layerXml, bool parseBase = true)
		{
			var layer = new TiledMapObjectLayer();
			
			if (parseBase) // This is done for tiles, which have objects. Their objectgroup doesn't need any base layer stuff.
			{
				ParseBaseLayer(layerXml, layer);
			}

			if (layerXml.Attributes["color"] != null)
			{
				layer.Color = XmlHelper.StringToColor(layerXml.Attributes["color"].Value);
			}

			if (XmlHelper.GetXmlStringSafe(layerXml, "draworder") == "index")
			{
				layer.DrawingOrder = TiledMapObjectDrawingOrder.Manual;
			}
			else
			{
				layer.DrawingOrder = TiledMapObjectDrawingOrder.TopDown;
			}

			// Parsing objects.
			var objectsXml = layerXml.SelectNodes("object");
			var objects = new List<TiledObject>();

			foreach(XmlNode obj in objectsXml)
			{
				objects.Add(ParseObject(obj));
			}

			layer.Objects = objects.ToArray();
			// Parsing objects.

			return layer;
		}

		static TiledObject ParseObject(XmlNode node)
		{
		
			var oldRoot = TiledMapImporter.CurrentRootDir;

			TiledObject obj = null;

			// If there is a template, we need to merge it with object.
			if (node.Attributes["template"] != null)
			{
				var templatePath = TiledMapImporter.CurrentRootDir + node.Attributes["template"].Value;
				node = MergeWithTemplate(node, templatePath);
				TiledMapImporter.CurrentRootDir = Path.GetDirectoryName(templatePath);
			}

			// Determining object type.
			var baseObj = ParseBaseObject(node);
			
			// Yes, this is horrible. But there's no better way, as far, as I know.

			if (node.Attributes["gid"] != null)
			{
				obj = ParseTileObject(node, baseObj);
			}

			if (node["point"] != null)
			{
				obj = ParsePointObject(baseObj);
			}

			if (node["polygon"] != null || node["polyline"] != null)
			{
				obj = ParsePolygonObject(node, baseObj);
			}

			if (node["ellipse"] != null)
			{
				obj = ParseEllipseObject(baseObj);
			}

			if (node["text"] != null)
			{
				obj = ParseTextObject(node, baseObj);
			}

			if (obj == null)
			{
				obj = ParseRectangleObject(baseObj);
			}

			TiledMapImporter.CurrentRootDir = oldRoot;
			return obj;
			// Determining object type.
		}



		/// <summary>
		/// Parses basic object properties, common to all object types.
		/// </summary>
		static TiledObject ParseBaseObject(XmlNode node)
		{
			var obj = new TiledObject();

			obj.Name = XmlHelper.GetXmlStringSafe(node, "name");
			obj.Type = XmlHelper.GetXmlStringSafe(node, "type");
			obj.ID = int.Parse(node.Attributes["id"].Value);
			obj.Position = new Vector2(
				XmlHelper.GetXmlFloatSafe(node, "x"),
				XmlHelper.GetXmlFloatSafe(node, "y")
			);
			obj.Size = new Vector2(
				XmlHelper.GetXmlFloatSafe(node, "width"),
				XmlHelper.GetXmlFloatSafe(node, "height")
			);
			obj.Rotation = XmlHelper.GetXmlFloatSafe(node, "rotation");
			obj.Visible = XmlHelper.GetXmlBoolSafe(node, "visible");
			
			obj.Properties = XmlHelper.GetProperties(node);

			return obj;
		}


		static TiledRectangleObject ParseRectangleObject(TiledObject baseObj) =>
			new TiledRectangleObject(baseObj);
		
		static TiledEllipseObject ParseEllipseObject(TiledObject baseObj) =>
			new TiledEllipseObject(baseObj);

		static TiledPointObject ParsePointObject(TiledObject baseObj) =>
			new TiledPointObject(baseObj);
		
		static TiledTileObject ParseTileObject(XmlNode node, TiledObject baseObj)
		{
			var obj = new TiledTileObject(baseObj);

			uint firstGID = 0;


			if (node["tileset"] != null && node["tileset"].Attributes["source"] != null)
			{
				// GetFullPath gets rid of stuff like // and /../ 
				var src = Path.GetFullPath(TiledMapImporter.CurrentRootDir + '/' + node["tileset"].Attributes["source"].Value);
				TilesetParser.ExternalTilesetsFirstGID.TryGetValue(src, out firstGID);
			}
			
			// For some reason, gid field is counted from 1, but other tile ids are counted from 0.
			var gid = uint.Parse(node.Attributes["gid"].Value) + firstGID - 1;

			obj.FlipHor = ((gid & (uint)FlipFlags.FlipHor) != 0);
			obj.FlipVer = ((gid & (uint)FlipFlags.FlipVer) != 0);
			obj.GID = (int)(gid & (~(uint)FlipFlags.All));
			
			return obj;
		}
		
		static TiledTextObject ParseTextObject(XmlNode node, TiledObject baseObj)
		{
			var textXml = node["text"];
			var obj = new TiledTextObject(baseObj);
			obj.Text = textXml.InnerText;
			if (textXml.Attributes["color"] != null)
			{
				obj.Color = XmlHelper.StringToColor(textXml.Attributes["color"].Value);
			}
			obj.WordWrap = XmlHelper.GetXmlBoolSafe(textXml, "color", false);
			
			obj.HorAlign = (TiledTextAlign)XmlHelper.GetXmlEnumSafe(
				textXml, 
				"halign", 
				TiledTextAlign.Left
			);
			obj.VerAlign = (TiledTextAlign)XmlHelper.GetXmlEnumSafe(
				textXml, 
				"valign", 
				TiledTextAlign.Left
			);

			obj.Font = XmlHelper.GetXmlStringSafe(textXml, "fontfamily");
			obj.FontSize = XmlHelper.GetXmlIntSafe(textXml, "pixelsize");
			obj.Underlined = XmlHelper.GetXmlBoolSafe(textXml, "underline");
			obj.StrikedOut = XmlHelper.GetXmlBoolSafe(textXml, "strikeout");

			return obj;
		}

		static TiledPolygonObject ParsePolygonObject(XmlNode node, TiledObject baseObj)
		{
			var obj = new TiledPolygonObject(baseObj);
			XmlNode polyXml;

			if (node["polygon"] != null)
			{
				polyXml = node["polygon"];
				obj.Closed = true;
			}
			else
			{
				polyXml = node["polyline"];
				obj.Closed = false;
			}

			var pointStrings = polyXml.Attributes["points"].Value.Split(' ');

			var points = new Vector2[pointStrings.Length];

			for(var i = 0; i < points.Length; i += 1)
			{
				var vecStr = pointStrings[i].Split(',');
				points[i] = new Vector2(
					float.Parse(vecStr[0], CultureInfo.InvariantCulture),
					float.Parse(vecStr[1], CultureInfo.InvariantCulture)
				);
			}
			obj.Points = points;

			return obj;
		}

		/// <summary>
		/// Some objects are referencing templates, 
		/// and can override some parameters.
		/// 
		/// Method merges object and template, overriding existing template parameters.
		/// </summary>
		static XmlNode MergeWithTemplate(XmlNode node, string templatePath)
		{
			// Loading template.
			var doc = new XmlDocument();
			XmlNode template;
			try
			{
				doc.Load(templatePath);
				template = doc["template"]["object"];
				foreach(XmlNode neighbourNode in doc["template"])
				{
					if (template != neighbourNode)
					{
						template.AppendChild(neighbourNode);
					}
				}
			}
			catch(Exception e)
			{
				throw new Exception("Error loading object template! " + e.StackTrace + Environment.NewLine + e.Message);
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

			// Merging object properties with template's.
			if (template["properties"] != null)
			{
				// This is done because properties with default values aren't copied into object.
				foreach(XmlNode templateProperty in template["properties"].ChildNodes)
				{
					var nodeExists = false;

					foreach(XmlNode nodeProperty in node["properties"].ChildNodes)
					{
						if (nodeProperty.Attributes["name"].Value == templateProperty.Attributes["name"].Value)
						{
							nodeExists = true;
							break;
						}
					}

					if (!nodeExists)
					{
						var clonedProperty = owner.ImportNode(templateProperty, true);
						node["properties"].AppendChild(clonedProperty);
					}
				}
			}
			// Merging object properties with template's.

			return node;
		}



		static TiledMapImageLayer ParseImageLayer(XmlNode layerXml)
		{
			var imageXml = layerXml["image"];
			if (imageXml == null)
			{
				return null; // Without image, layer is empty, so we won't need it.
			}

			var layer = new TiledMapImageLayer();
			
			ParseBaseLayer(layerXml, layer);
			
			layer.TexturePath = XmlHelper.GetXmlStringSafe(imageXml, "source");
			
			if (layer.TexturePath == "") 
			{
				return null; // Same thing. No path == no image. 
			}

			if (imageXml.Attributes["trans"] != null)
			{
				layer.TransparentColor = XmlHelper.StringToColor(imageXml.Attributes["trans"].Value);
			}

			return layer;
		}

	}
}
