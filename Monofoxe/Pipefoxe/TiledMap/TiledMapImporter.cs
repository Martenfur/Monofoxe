using System;
using System.Collections.Generic;
using Monofoxe.Tiled.MapStructure;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.Text;
using System.IO;


namespace Pipefoxe.TiledMap
{
	[ContentImporter(".tmx", DefaultProcessor = "PassThroughProcessor", 
	DisplayName = "Tiled Map Importer - Monofoxe")]
	public class TiledMapImporter : ContentImporter<XmlDocument>
	{
		StringBuilder logs = new StringBuilder();

		public override XmlDocument Import(string filename, ContentImporterContext context)
		{
			var rootDir = Path.GetDirectoryName(filename) + '/';

			context.Logger.LoggerRootDirectory = rootDir;

			var xml = new XmlDocument();
			xml.Load(filename);

			XmlNodeList tilesetsXml = xml["map"].SelectNodes("tileset");
			
			__Log(tilesetsXml.Count + "");
			__Log(rootDir);

			foreach(XmlNode tilesetXml in tilesetsXml)
			{
				__Log("Hi!");
			}

			__SaveLog(rootDir);
			return xml;
		}

		TiledMapTileset ParseTileset()
		{
			return null;
		}

		void __Log(string text)
		{
			logs.Append(text + Environment.NewLine);
		}

		void __SaveLog(string path)
		{
			File.WriteAllText(path + "log.log", logs.ToString());
		}

	}
}
