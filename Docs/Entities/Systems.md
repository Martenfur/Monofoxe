# Systems

Time to get serious. Systems are component processors. They take components, operate on them and release into the wild. 

**NOTE:** To be processed by the system, component should have an owner.

Like entities, systems have several events:

- `Create()` - Executes when component is created.
- `Update()` - Executes every frame and is designed to hold game logic. **IMPORTANT**: `Update()` accepts ALL the entities from the scene at the same time. 
- `FixedUpdate()` - Executes at fixed intervals governed by `GameMgr.FixedUpdateRate`. **IMPORTANT**: `Update()` accepts ALL the entities from the scene at the same time.
- `Draw()` - Executes every frame for each camera (so if you got several cameras, Draw will execute several times per frame) and is designed to hold drawing logic. **IMPORTANT**: Do not put any heavy logic into Draw. It may start skipping frames due to how Monogame works. 
- `Destroy()` - Executes when the component is destroyed.

As you can see, some methods accept whole lists of entities at a time. It's also important to note that this happens per-scene, so if you have several scenes active, components won't mix up.

Let's create a system.

```C#
using Monofoxe.Engine.ECS;
using System;
using System.Collections.Generic;

public class STest : BaseSystem // You can name your systems either SYourName or YourNameSystem.
{
	// Now all the components of type CTest are binded to thsi system.
	// NOTE: Each component type can have only one system binded to it.
	public override Type ComponentType => typeof(CTest);

	public override void Create(Component component)
	{
		// Systems pass components is base type, so we need to cast them back.
		var test = (CTest)component;
		test.Visible = true;
	}

	public override void FixedUpdate(List<Component> components)
	{
		foreach(CTest test in components)
		{
			// Do stuff.
		}
	}

	public override void Draw(Component component)
	{
    var test = (CTest)component;
		// Draw stuff.
	}
}
```



## [<< Entity templates](EntityTemplates.md) | 

[<<< Contents](../Contents.md)

