using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

// Inspired by:
// https://github.com/Ragath/MonoGame.AssetInfo


namespace Pipefoxe.AssetInfo
{
	/// <summary>
	/// Asset info importer. Reads .mcgb file and extracts all asset paths.
	/// </summary>
	[ContentImporter(".mgcb", DisplayName = "Asset Info Importer - Monofoxe", DefaultProcessor = "PassThroughProcessor")]
	public class AssetInfoImporter : ContentImporter<string[]>
	{
		const string _beginTag = "#begin ";
		const string _copyTag = "/copy:";

		public override string[] Import(string filename, ContentImporterContext context)
		{
			var lines = File.ReadAllLines(filename);

			var assets = new List<string>();

			for(var i = 0; i < lines.Length; i += 1)
			{
				if (lines[i].StartsWith(_beginTag))
				{
					var assetPath = lines[i].Remove(0, _beginTag.Length);

					// If asset is being copied, we'll need to leave its extension.
					if (i + 1 < lines.Length && !lines[i + 1].StartsWith(_copyTag))
					{
						assetPath = Path.GetDirectoryName(assetPath) + '/' + Path.GetFileNameWithoutExtension(assetPath);
					}
					assets.Add(assetPath);
				}
			}

			return assets.ToArray();
		}		
	}
}
