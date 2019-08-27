# Components

Let's refresh -- component in ECS is just data structure which gets processed by the system. Since our ECS is a bit bendy, components shouldn't necessarily be processed by the systems. Entities can do this as well, since they have access to their own components.

Creating a component is very easy:

```C#
using Monofoxe.Engine.ECS;

public class CTest : Component // You can name your components either CYourName or YourNameComponent.
{
	// Your data goes here!
}
```

Just inherit Component class and you're done. 

Components also have several fields:

- `Owner` - Reference to an entity which owns this component. There can be only one owner.
- `System` - Reference to a system which processes this component. Set automatically.
- `Initialized` - Tells if `Create` event has been called for this component.
- `Enabled` - If true, component will be processed by system's `Update()` and `FixedUpdate()` events.
- `Visible` - If true, component will be processed by system's `Draw()` event. **NOTE**: `Visible` is false by default. Set it to true in component's constructor or system's `Create` event if you want component to be drawn.

## How do I add components to entities?

```C#
var entity = new Entity(Layer, "testEntity");
var testComponent = new CTest();
entity.AddComponent(testComponent);
```

You can add components in entity's constructor or create an entity template, which will be discussed later.

To access components from an entity, use this:

```C#
entity.GetComponent<CTest>();
```

You can also access other components from a component through `Owner` field.

```C#
component.Owner.GetComponent<CTest>();
```

 

## [<< Introduction to ECS](IntroductionToECS.md)	|	[Entity templates >>](EntityTemplates.md)

[<<< Contents](../Contents.md)

