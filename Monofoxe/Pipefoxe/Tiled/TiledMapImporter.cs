using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;
using Monofoxe.Tiled.MapStructure;

namespace Pipefoxe.Tiled
{
	[ContentImporter(".tmx", DefaultProcessor = "TiledMapProcessor",
	DisplayName = "Tiled Map Importer - Monofoxe")]
	public class TiledMapImporter : ContentImporter<TiledMap>
	{
		/// <summary>
		/// Directory of the .tmx file.
		/// </summary>
		public static string TmxRootDir;

		/// <summary>
		/// Current root directory.
		/// </summary>
		public static string CurrentRootDir;


		public override TiledMap Import(string filename, ContentImporterContext context)
		{
			TmxRootDir = Path.GetDirectoryName(filename) + '/';
			CurrentRootDir = TmxRootDir;

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
