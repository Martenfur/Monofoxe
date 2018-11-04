using System;
using System.Collections.Generic;
using Monofoxe.Tiled.MapStructure;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Pipefoxe.Tiled
{
	static class MapParser
	{
		public static TiledMap Parse(XmlDocument xml)
		{
			var map = new TiledMap();
			var mapXml = xml["map"];
			var mapAttributes = xml["map"].Attributes;

			if (mapAttributes["infinite"].Value == "1")
			{
				throw new Exception("Infinite maps are not supported yet!");
			}

			map.Width = int.Parse(mapAttributes["width"].Value);
			map.Height = int.Parse(mapAttributes["height"].Value);
			map.TileWidth = int.Parse(mapAttributes["tilewidth"].Value);
			map.TileHeight = int.Parse(mapAttributes["tileheight"].Value);
			
			Enum.TryParse(mapAttributes["renderorder"].Value.Replace("-", ""), true, out map.RenderOrder);
			Enum.TryParse(mapAttributes["orientation"].Value, true, out map.Orientation);

			if (mapAttributes["staggeraxis"] != null)
			{
				Enum.TryParse(mapAttributes["staggeraxis"].Value, true, out map.StaggerAxis);
			}
			if (mapAttributes["staggerindex"] != null)
			{
				Enum.TryParse(mapAttributes["staggerindex"].Value, true, out map.StaggerIndex);
			}

			map.HexSideLength = TiledMapImporter.GetXmlIntSafe(mapXml, "hexsidelength");


			XmlNodeList tilesetsXml = mapXml.SelectNodes("tileset");
			map.Tilesets = TilesetParser.Parse(tilesetsXml);
			LayerParser.Parse(mapXml, map);


			return map;
		}
	}
}
