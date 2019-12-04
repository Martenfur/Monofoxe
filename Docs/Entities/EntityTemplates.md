# Entity templates

If you are using inheritance, you can just add all your components in an entity's constructor. But what if you're going pure EC and using basic `Entity` class directly? Then you have no place to assemble your entity.

This is why entity templates exist. `EntityTemplate` is a little factory class which creates entities on demand.

Here's how you can create one:

```C#
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;

public class TestTemplate : IEntityTemplate
{
	public string Tag => "testEntity";

	public Entity Make(Layer layer)
	{
		var entity = new Entity(layer);
		
		var testComponent = new CTest();
		entity.AddComponent(testComponent);
		
		return entity;
	}
}
```

To invoke template, you must call static method

```C#
Entity.CreateFromTemplate(Layer, "testEntity");
```



## [<< Components](Components.md)	|	[Layers >>](../SceneSystem/Layers.md)

[<<< Contents](../Contents.md)

