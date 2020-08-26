using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;
using Monofoxe.Tiled.MapStructure;

namespace Monofoxe.Pipeline.Tiled
{
	[ContentImporter(".tmx", DefaultProcessor = "PassThroughProcessor",
	DisplayName = "Tiled Map Importer - Monofoxe")]
	public class TiledMapImporter : ContentImporter<TiledMap>
	{
		/// <summary>
		/// Directory of the .tmx file.
		/// </summary>
		public static string TmxRootDir;
		public static string TmxFilename;

		/// <summary>
		/// Current root directory.
		/// </summary>
		public static string CurrentRootDir;


		public override TiledMap Import(string filename, ContentImporterContext context)
		{
			Logger.Init("map.log");
			TmxRootDir = Path.GetDirectoryName(filename) + '/';
			TmxFilename = filename;
			CurrentRootDir = TmxRootDir;
			Logger.LogLine("ROOT:" + TmxRootDir);
			try
			{
				var xml = new XmlDocument();
				xml.Load(filename);
				
				return MapParser.Parse(xml);
			}
			catch(System.Exception e)
			{
				throw new System.Exception("Failed to import the map! " + e.Message + " " + e.StackTrace);
			}
		}
	}
}
