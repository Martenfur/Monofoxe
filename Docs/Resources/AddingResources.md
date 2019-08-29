# Adding resources

Now we know how resource management in Monofoxe works, but how to access resources in game?

By default, all the resources are loaded from classes in the `Resources` folder. These classes are purely optional and you can roll out your own loading system.

The easiest one is `Sprites.Default` class. It's generated automatically as you add new sprites in the Explorer.

With other resource types things are a bit less convenient. Let's look at the `Fonts` class.

```c#
namespace Resources
{
	public static class Fonts
	{
		private static ContentManager _content;
		
		static string Ascii = " !" + '"' + @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		
		public static IFont Arial;

		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.FontsDir;
			
			Arial = new Font(_content.Load<SpriteFont>("Arial"));
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}

```

As you can see, it has `Load()` and `Unload()` methods. They are called in `Game1` class to load and unload the resources.

To load another font, you should add a new entry in the class by hand. You can also notice the 'AssetMgr' class. All it does is provide a bunch of shortcuts for commonly used names. Additionally, it has `GetAssetPaths()` method, which returns names of all the assets.



## [<< NoPipeline](NoPipeline.md) | [Creating custom resource types >>](CreatingCustomResourceTypes.md) 

[<<< Contents](../Contents.md)

