using Microsoft.Xna.Framework.Content.Pipeline;

namespace Monofoxe.Pipeline.ResourceInfo
{
	/// <summary>
	/// Imports the generated resource list. The list contains paths of all
	/// built and copied assets and is used by ResourceInfoMgr at runtime.
	/// 
	/// The Content Builder generates the file from the content collection,
	/// see ResourceList.Collect().
	/// </summary>
	[ContentImporter(".reslist", DisplayName = "Resource Info Importer - Monofoxe", DefaultProcessor = "PassThroughProcessor")]
	public class ResourceInfoImporter : ContentImporter<string[]>
	{
		public override string[] Import(string filename, ContentImporterContext context) =>
			ResourceList.Load(filename);
	}
}
