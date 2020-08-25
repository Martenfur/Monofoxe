# Adding resources

Now we know how resource management in Monofoxe works, but how to access resources in the game?

By default, all the resources are loaded with classes in the `Resources` folder. These classes are purely optional and you can roll out your own loading system.

The easiest one is `Sprites.Default` class. It's generated automatically as you add new sprites in the Explorer.

With other resource types things are a bit less convenient. Let's look at the `Fonts` class.

```c#
namespace Resources
{
	public class Fonts : ResourceBox<IFont>
	{
		private ContentManager _content;

		public Fonts() : base("Fonts")
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = ResourceInfoMgr.ContentDir + "/Fonts";
		}

		public override void Load()
		{
			if (Loaded)
			{
				return;
			}
			Loaded = true;

			AddResource("Arial", new Font(_content.Load<SpriteFont>("Arial")));

			var fontSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Font");

			AddResource("FancyFont", new TextureFont(fontSprite, 1, 1, TextureFont.Ascii, false));
		}

		public override void Unload()
		{
			if (!Loaded)
			{
				return;
			}
			Loaded = false;
			_content.Unload();
		}
	}
}

```

As you can see, it has `Load()` and `Unload()` methods. They are called by the `ResourceHub` class to load and unload the resources.

To load another font, you should add a new entry in the class by hand. You can also notice the `AssetMgr` class being used. All it does is provide a bunch of shortcuts for commonly used names. Additionally, it has `GetAssetPaths()` method, which returns names of all the assets.



## [<< NoPipeline](NoPipeline.md) | [Resource Hub >>](ResourceHub.md) 

[<<< Contents](../Contents.md)

