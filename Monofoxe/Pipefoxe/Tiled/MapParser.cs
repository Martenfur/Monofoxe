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
			// TODO: Add properties here.
			
			if (mapXml.Attributes["infinite"].Value == "1")
			{
				throw new Exception("Infinite tilemaps are not supported yet!");
			}

			map.Width = int.Parse(mapXml.Attributes["width"].Value);
			map.Height = int.Parse(mapXml.Attributes["height"].Value);
			map.TileWidth = int.Parse(mapXml.Attributes["tilewidth"].Value);
			map.TileHeight = int.Parse(mapXml.Attributes["tileheight"].Value);


			XmlNodeList tilesetsXml = mapXml.SelectNodes("tileset");
			map.Tilesets = TilesetParser.Parse(tilesetsXml);
			LayerParser.Parse(mapXml, map);


			return map;
		}
	}
}
