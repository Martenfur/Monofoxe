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
		public static string RootDir;

		public override TiledMap Import(string filename, ContentImporterContext context)
		{
			RootDir = Path.GetDirectoryName(filename) + '/';
			
			var xml = new XmlDocument();
			xml.Load(filename);

			return MapParser.Parse(xml);
		}
	}
}
