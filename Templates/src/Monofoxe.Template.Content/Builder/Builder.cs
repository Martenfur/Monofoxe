using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using MonoGame.Framework.Content.Pipeline.Builder;
using Monofoxe.Pipeline.ResourceInfo;
using Monofoxe.Pipeline.SpriteGroup;
using Monofoxe.Pipeline.Tiled;

/// <summary>
/// Entry point for the Content Builder project. It builds the contents of the
/// Content folder according to the rules defined in the Builder class.
///
/// Rules are evaluated on every build, so new assets are picked up without
/// editing any config - just drop them into the right folder.
/// </summary>
var contentBuilderParams = new ContentBuilderParams()
{
	Mode = ContentBuilderMode.Builder,
	WorkingDirectory = $"{AppContext.BaseDirectory}../../",
	SourceDirectory = "Content",
	Platform = TargetPlatform.DesktopGL
};

var builder = new Builder();

if (args is not null && args.Length > 0)
{
	builder.Run(args);
}
else
{
	builder.Run(contentBuilderParams);
}

return builder.FailedToBuild > 0 ? -1 : 0;



public class Builder : ContentBuilder
{
	public override IContentCollection GetContentCollection()
	{
		var content = new ContentCollection();
		var sourceDirectory = Parameters.RootedSourceDirectory;

		// Audio is copied as is and loaded at runtime directly.
		content.IncludeCopy<WildcardRule>("Audio/Music/*.ogg");
		content.IncludeCopy<WildcardRule>("Audio/Sounds/*.wav");

		// Sprite groups pack all sprites from their root directory into texture
		// atlases. Each group gets its own rule carrying a fingerprint of its
		// sprite list, so adding or removing a sprite rebuilds that atlas. Edits
		// of existing sprites are reported by the importer as dependencies.
		// This replaces Nopipeline's "watch" lists.
		foreach (var spriteGroupPath in Directory.EnumerateFiles(sourceDirectory, "*.spritegroup", SearchOption.AllDirectories))
		{
			var relativePath = Path.GetRelativePath(sourceDirectory, spriteGroupPath).Replace('\\', '/');

			content.Include(
				relativePath,
				SpriteGroupImporter.Create(spriteGroupPath),
				new SpriteGroupProcessor()
			);
		}

		content.Include<WildcardRule>(
			"Effects/*.fx",
			new EffectImporter(),
			new EffectProcessor()
		);

		// Tiled maps reference tilesets and object templates, which the importer
		// also reports as dependencies.
		content.Include<WildcardRule>(
			"Maps/*.tmx",
			new TiledMapImporter(),
			new PassThroughProcessor()
		);

		// Tileset textures are loaded by the game as regular textures.
		content.Include<WildcardRule>(
			"Maps/*.png",
			new TextureImporter(),
			new TextureProcessor()
		);

		content.Include<WildcardRule>(
			"Fonts/*.spritefont",
			new FontDescriptionImporter(),
			new FontDescriptionProcessor()
		);

		// ResourceInfoMgr uses this list at runtime to find resources by wildcard.
		var resourceListPath = Path.Combine(sourceDirectory, ResourceList.FileName);
		ResourceList.Save(resourceListPath, ResourceList.Collect(content, Parameters));
		content.Include(
			ResourceList.FileName,
			ResourceList.AssetName,
			new ResourceInfoImporter(),
			new PassThroughProcessor()
		);

		return content;
	}
}
