using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

// Inspired by:
// https://github.com/Ragath/MonoGame.AssetInfo


namespace Pipefoxe.ResourceInfo
{
	/// <summary>
	/// Asset info importer. Reads .mcgb file and extracts all asset paths.
	/// </summary>
	[ContentImporter(".npl", DisplayName = "Resource Info Importer - Monofoxe", DefaultProcessor = "PassThroughProcessor")]
	public class ResourceInfoImporter : ContentImporter<string[]>
	{
		const string _beginTag = "#begin ";
		const string _copyTag = "/copy:";

		public override string[] Import(string filename, ContentImporterContext context)
		{
			// Since 3.8, pipeline cannot import its own mgcb files as resources,
			// so we have to reference npl config instead and swap it for mgcb.
			filename = Path.ChangeExtension(filename, ".mgcb");

			var lines = File.ReadAllLines(filename);

			var resources = new List<string>();

			for(var i = 0; i < lines.Length; i += 1)
			{
				if (lines[i].StartsWith(_beginTag))
				{
					var resourcePath = lines[i].Remove(0, _beginTag.Length);

					// If resource is being copied, we'll need to leave its extension.
					if (i + 1 < lines.Length && !lines[i + 1].StartsWith(_copyTag))
					{
						resourcePath = Path.Combine(Path.GetDirectoryName(resourcePath), Path.GetFileNameWithoutExtension(resourcePath));
					}
					resources.Add(resourcePath.Replace('\\', '/'));
				}
			}

			return resources.ToArray();
		}		
	}
}
