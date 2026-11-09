# Content Builder

Monofoxe uses the [MonoGame Content Builder](https://docs.monogame.net/articles/getting_started/content_pipeline/content_builder/index.html) to build resources. Since MonoGame `3.8.6` it replaces the old `.mgcb` files and the Pipeline tool, and Monofoxe no longer needs [Nopipeline](https://github.com/Martenfur/Nopipeline) to generate its config.

The content project (`YourProjectName.Content`) is a small console application:

```
YourProjectName.Content/
├── Content/                # Your source assets
│   ├── Graphics/
│   ├── Audio/
│   └── ...
├── Builder/
│   └── Builder.cs          # Rules that decide how assets are built
├── BuildContent.targets    # Runs the builder when a platform project builds
└── YourProjectName.Content.csproj
```

When you build `YourProjectName.DX` or `YourProjectName.GL`, `BuildContent.targets` builds the content project and runs it. The built `.xnb` files are placed into the `Content` folder inside the game's output directory, so `Content.Load()` works exactly as before. You can also run the builder by hand:

```bash
dotnet run --project YourProjectName.Content -- build -s YourProjectName.Content/Content -o YourProjectName.DX/bin/Debug/net8.0-windows -i YourProjectName.DX/obj/Content -p Windows
```

## Adding resources

Drop the file into the right folder inside `Content` and build the game. There is nothing to register and no config to update: rules are evaluated on every build.

Each rule matches a group of files:

```c#
// Build all sprite groups.
content.Include<WildcardRule>("Graphics/*.spritegroup", new SpriteGroupImporter(), new SpriteGroupProcessor());

// Build all tileset textures as regular textures.
content.Include<WildcardRule>("Maps/*.png", new TextureImporter(), new TextureProcessor());

// Copy audio without processing it.
content.IncludeCopy<WildcardRule>("Audio/Sounds/*.wav");
```

`*` also matches `/`, so `Maps/*.png` covers every subfolder. The last matching rule wins, and `Exclude` removes a file from the build. See `Builder.cs` in your content project for the complete set of rules.

If you need a new resource type, add a rule for its extension. That's the equivalent of adding a file group to the old Nopipeline config.

## Resource list

`ResourceInfoMgr` needs to know what assets exist to load them by wildcard (see `DirectoryResourceBox<T>`). The builder generates `Content/ResourceList.reslist` from the same rules and builds it into the `Content` asset. The file is generated and ignored by git, so don't edit it by hand.

## Sprite groups and other multi-file assets

Some assets are built from other files that are not built on their own. A sprite group, for example, packs every `.png` from its `rootDir` into an atlas, and a Tiled map references tilesets and object templates.

Previously Nopipeline had a `watch` list for this: it told the Pipeline tool to rebuild the asset when one of the watched files changed. With the Content Builder the pipeline tracks this by itself:

- `SpriteGroupImporter` reports every sprite and sprite config it reads as a dependency. Editing a sprite rebuilds only the groups that use it.
- Each sprite group also carries a fingerprint of its sprite list, so adding or removing a sprite rebuilds the atlas even though new sprites are not part of the content collection yet.
- `TiledMapImporter` reports the tilesets and object templates the map references, so editing them rebuilds the map.

You don't have to configure any of this. If you write your own importer for an asset that depends on other files, call `context.AddDependency(path)` for every file you read, so the builder knows when the asset is out of date.

## [Adding resources >>](AddingResources.md)

[<<< Contents](../Contents.md)
