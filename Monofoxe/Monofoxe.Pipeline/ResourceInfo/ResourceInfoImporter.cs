using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

// Inspired by:
// https://github.com/Ragath/MonoGame.AssetInfo


namespace Monofoxe.Pipeline.ResourceInfo
{
	/// <summary>
	/// Asset info importer. Reads .mcgb file and extracts all asset paths.
	/// </summary>
	[ContentImporter(".npl", DisplayName = "Resource Info Importer - Monofoxe", DefaultProcessor = "PassThroughProcessor")]
	public class ResourceInfoImporter : ContentImporter<string[]>
	{
		private const string _buildTag = "/build:";
		private const string _copyTag = "/copy:";

		public override string[] Import(string filename, ContentImporterContext context)
		{
			// Since 3.8, pipeline cannot import its own mgcb files as resources,
			// so we have to reference npl config instead and swap it for mgcb.
			filename = Path.ChangeExtension(filename, ".mgcb");

			var lines = File.ReadAllLines(filename);

			var resources = new List<string>();

			for(var i = 0; i < lines.Length; i += 1)
			{
				if (lines[i].StartsWith(_buildTag))
				{
					var resourcePath = Dereference(lines[i].Remove(0, _buildTag.Length));

					resourcePath = Path.Combine(Path.GetDirectoryName(resourcePath), Path.GetFileNameWithoutExtension(resourcePath));

					resources.Add(resourcePath.Replace('\\', '/'));
					continue;
				}				
				if (lines[i].StartsWith(_copyTag))
				{
					var resourcePath = Dereference(lines[i].Remove(0, _copyTag.Length));

					// If resource is being copied, we'll need to leave its extension.
					resources.Add(resourcePath.Replace('\\', '/'));
				}
			}

			return resources.ToArray();
		}

		private string Dereference(string path)
		{ 
			if (path.Contains(";"))
			{ 
				return path.Split(';')[1];
			}
			return path;
		}
	}
}
