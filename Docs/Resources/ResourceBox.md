# Resource Box

As been said in the previous article, `ResourceBox` is a resource container. By default, Monofoxe project creates several of them:

- `"DefaultSprites"`
- `"Fonts"`
- `"Effects"`
- `"Maps"`

Note that resource boxes aren't mandatory - you can create whatever resource system you want. But foxes do not recommend.

Let's look at an empty ResourceBox:

```c#
namespace Resources
{
	// Resource box should inherit ResourceBox type.
	// Altrnatively you can implement IResourceBox interface if you 
	// need even more flexibility.
	public class DefaultBox : ResourceBox<YourResource>
	{
		public DefaultBox()
		{
			// Put ContentManager init code here.
		}

		public override void Load()
		{
			// A check to not load the same stuff twice.
			if (Loaded)
			{
				return;
			}
			Loaded = true;

			// Actual resources. Add more resources here.
			AddResource("Resource1", new YourResource());
			AddResource("Resource2", new YourResource());
			// Actual resources.
		}

		public override void Unload()
		{
			if (!Loaded)
			{
				return;
			}
			Loaded = false;
		}
	}
}


```



Now, just put `new DefaultBox();` into your `Game1` class, and... that's it. Resource box will be automatically added to the `ResourceHub`.



## [<< Resource Hub](ResourceHub.md) | [Creating custom resource types >>](CreatingCustomResourceTypes.md) 

[<<< Contents](../Contents.md)

