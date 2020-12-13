# Components

Let's refresh - component in EC is just an addon for an entity. It runs its own Update/Draw methods and may not depend on the entity's logic whatsoever.

Creating a component is very easy:

```C#
using Monofoxe.Engine.EC;

public class CTest : Component // You can name your components either CYourName or YourNameComponent.
{
	// Your data goes here!
}
```

Just inherit Component class and you're done. 

Components also have several fields:

- `Owner` - Reference to an entity which owns this component. There can be only one owner.
- `Initialized` - Tells if `Create` event has been called for this component.
- `Enabled` - If true, component will be processed by system's `Update()` and `FixedUpdate()` events.
- `Visible` - If true, component will be processed by system's `Draw()` event. **NOTE**: `Visible` is false by default. Set it to true in component's constructor or system's `Create` event if you want component to be drawn.

Like entities, components have events:

- `Initialize()` - Executes every time when component is added to the entity. If component is readded several times, the event will also execute several times.
- `Update()` - Executes every frame and is designed to hold game logic.
- `FixedUpdate()` - Executes at fixed intervals governed by `GameMgr.FixedUpdateRate`. 
- `Draw()` - Executes every frame for each camera (so if you got several cameras, Draw will execute several times per frame) and is designed to hold drawing logic. **IMPORTANT**: Do not put any heavy logic into Draw. It may start skipping frames due to how Monogame works. 
- `Destroy()` - Executes when the component is destroyed.



## How do I add components to entities?

```C#
var entity = new Entity(Layer);
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

 

## [<< Introduction to EC](IntroductionToEC.md)	|	[Layers >>](../SceneSystem/Layers.md)

[<<< Contents](../Contents.md)

