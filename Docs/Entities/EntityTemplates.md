# Entity templates

If you are using inheritance, you can just add all your components in the constructor. But what if you're going pure ECS and using basic `Entity` class directly? Then you have no place to assemble your entity.

This is why entity templates exist. `EntityTemplate` is a little factory class which creates entities on demand.

Here's how you can create one:

```C#
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;

public class TestTemplate : IEntityTemplate
{
	public string Tag => "testEntity";

	public Entity Make(Layer layer)
	{
		var entity = new Entity(layer, Tag);
		
		var testComponent = new CTest();
		entity.AddComponent(testComponent);
		
		return entity;
	}
}
```

Notice that we're passing a tag into the entity. `Tag` is necessary for pure ECS entity to be distinguishable from others. With inheritance you can just tell them apart by their type. 

To invoke template, you must call static method

```C#
Entity.CreateFromTemplate(Layer, "testEntity");
```



## [<< Components](Components.md)	|	[Systems >>](Systems.md)

[<<< Contents](../Contents.md)

