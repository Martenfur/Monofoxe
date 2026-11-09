using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MonoGame.Framework.Content.Pipeline.Builder;

namespace Monofoxe.Pipeline.ResourceInfo
{
	/// <summary>
	/// List of all game assets. ResourceInfoMgr uses it at runtime to find
	/// resources by wildcard.
	/// 
	/// The list is generated from the Content Builder rules and is built into
	/// the "Content" asset. Before the Content Builder existed, Nopipeline
	/// generated a .mgcb file and the importer parsed it. Now the list is
	/// generated straight from the content collection, so there is no config
	/// to keep in sync.
	/// </summary>
	public static class ResourceList
	{
		/// <summary>
		/// Name of the generated file inside the assets folder.
		/// </summary>
		public const string FileName = "ResourceList.reslist";

		/// <summary>
		/// Asset name the list is built into. ResourceInfoMgr loads it by this name.
		/// </summary>
		public const string AssetName = "Content";



		/// <summary>
		/// Collects asset names from the content collection rules.
		/// Built assets get their extension stripped, copied assets keep it,
		/// because that's how they are loaded at runtime.
		/// </summary>
		public static string[] Collect(IContentCollection contentCollection, ContentBuilderParams parameters)
		{
			var assets = new SortedSet<string>(StringComparer.Ordinal);
			var sourceDir = parameters.RootedSourceDirectory;

			foreach (var file in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
			{
				var relativePath = Path.GetRelativePath(sourceDir, file).Replace('\\', '/');

				if (relativePath == FileName)
				{
					continue;
				}

				if (!contentCollection.GetContentInfo(relativePath, out var contentInfos))
				{
					continue;
				}

				foreach (var contentInfo in contentInfos)
				{
					var assetPath = contentInfo.ShouldBuild
						? Path.ChangeExtension(relativePath, null)
						: relativePath;

					assets.Add(contentInfo.GetOutputPath(assetPath).Replace('\\', '/'));
				}
			}

			return assets.ToArray();
		}



		/// <summary>
		/// Writes the list into the assets folder. The file is rewritten only
		/// when the list has changed, so the builder cache doesn't get
		/// invalidated by every build.
		/// </summary>
		public static void Save(string path, IEnumerable<string> assets)
		{
			var json = JsonSerializer.Serialize(
				assets.ToArray(),
				new JsonSerializerOptions { WriteIndented = true }
			);

			if (File.Exists(path) && File.ReadAllText(path) == json)
			{
				return;
			}

			var directory = Path.GetDirectoryName(path);
			if (!string.IsNullOrEmpty(directory))
			{
				Directory.CreateDirectory(directory);
			}
			File.WriteAllText(path, json);
		}



		public static string[] Load(string path) =>
			JsonSerializer.Deserialize<string[]>(File.ReadAllText(path));
	}
}
