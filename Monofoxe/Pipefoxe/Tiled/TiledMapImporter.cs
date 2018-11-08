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
	[ContentImporter(".tmx", DefaultProcessor = "TiledMapProcessor",//"PassThroughProcessor", 
	DisplayName = "Tiled Map Importer - Monofoxe")]
	public class TiledMapImporter : ContentImporter<TiledMap>
	{
		static StringBuilder _logs = new StringBuilder();

		public static string RootDir;

		public override TiledMap Import(string filename, ContentImporterContext context)
		{
			RootDir = Path.GetDirectoryName(filename) + '/';
			
			var xml = new XmlDocument();
			xml.Load(filename);

			//try
			//{
				var map = MapParser.Parse(xml);

				__SaveLog(RootDir);
				return map;
			//}
			//catch(Exception e)
		//	{
		//		__SaveLog(RootDir);
		//		throw new Exception(e.StackTrace);
			//}
		}

		
		
		public static void __Log(string text)
		{
			_logs.Append(text + Environment.NewLine);
		}



		public static void __SaveLog(string path)
		{
			File.WriteAllText(path + "log.log", _logs.ToString());
		}
		
	}
}
