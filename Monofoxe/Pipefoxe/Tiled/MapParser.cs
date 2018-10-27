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
			
			XmlNodeList tilesetsXml = xml["map"].SelectNodes("tileset");
			
			map.Tilesets = TilesetParser.Parse(tilesetsXml);
			
			LayerParser.Parse(xml["map"], map);


			return map;
		}
	}
}
