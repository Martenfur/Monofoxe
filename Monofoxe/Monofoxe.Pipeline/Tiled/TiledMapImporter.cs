using System.Collections.Generic;
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

		/// <summary>
		/// Files the map references (external tilesets and object templates).
		/// They are reported to the builder as dependencies, so editing a tileset
		/// or a template rebuilds the map. Nopipeline used to do the same with
		/// its "watch" lists.
		/// </summary>
		private static readonly List<string> _dependencies = new List<string>();

		public static void AddDependency(string path)
		{
			var fullPath = Path.GetFullPath(path);
			if (!_dependencies.Contains(fullPath))
			{
				_dependencies.Add(fullPath);
			}
		}


		public override TiledMap Import(string filename, ContentImporterContext context)
		{
			Logger.Init("map.log");
			TmxRootDir = Path.GetDirectoryName(filename) + '/';
			TmxFilename = filename;
			CurrentRootDir = TmxRootDir;
			_dependencies.Clear();
			Logger.LogLine("ROOT:" + TmxRootDir);
			try
			{
				var xml = new XmlDocument();
				xml.Load(filename);

				var map = MapParser.Parse(xml);

				foreach (var dependency in _dependencies)
				{
					context.AddDependency(dependency);
				}

				return map;
			}
			catch(System.Exception e)
			{
				throw new System.Exception("Failed to import the map! " + e.Message + " " + e.StackTrace);
			}
		}
	}
}
