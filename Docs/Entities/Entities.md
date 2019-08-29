# Entities

Ok, now we know how to press buttons and everything, but what about actual game objects? We could put all the game logic into `Game1.Update()`, but that won't be very convenient. 

This is why entities exist! Class `Entity` in `Monofoxe.Engine.ECS` is the basic game object which can contain game logic. It has several events:

- `Update()` - Executes every frame and is designed to hold game logic.
- `FixedUpdate()` - Executes at fixed intervals governed by `GameMgr.FixedUpdateRate`. By default `Update()` is configured to execute at fixed intervals too, but you can change that.
- `Draw()` - Executes every frame for each camera (so if you got several cameras, Draw will execute several times per frame) and is designed to hold drawing logic. **IMPORTANT**: Do not put any heavy logic into Draw. It may start skipping frames due to how Monogame works. 
- `Destroy()` - Executes when the entity is destroyed.

**NOTE:** Only one event from one entity can execute at any moment. There is no multithreading magic involved.

Entity also has several methods and fields:

- `DestroyEntity()` - Removes an entity from update loop and marks it as destroyed.
- `Enabled` - If true, entity and its components will execute their `Update()` and `FixedUpdate()` events.
- `Visible` - If true, entity and its components will execute their `Draw()` event.
- `Scene` - Scene, on which current entity exists.
- `Layer` - Layer, on which current entity exists.
- `Depth` - If layer's depth sorting is enabled, entities will be sorted by their depth when being drawn. Entities with bigger `Depth` are drawn first.

There are others, but they are less important for now.

Let's look at an example of creating an entity:

```c#
using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using System;

public class FoxeEntity : Entity
{
	public Vector2 Position;

	public FoxeEntity(Layer layer, Vector2 position) : base(layer)
	{
		Position = position;
	}

	public override void Update()
	{
		// Moving entity around with the keyboard.
		Position += new Vector2(
			Input.CheckButton(Buttons.Right).ToInt() - Input.CheckButton(Buttons.Left).ToInt(),
			Input.CheckButton(Buttons.Down).ToInt() - Input.CheckButton(Buttons.Up).ToInt()
		);
	}

	public override void Draw()
	{
		GraphicsMgr.CurrentColor = Color.White;
		CircleShape.Draw(Position, 8, false); // Drawing filled white circle at the position.
	}

	public override void Destroy()
	{
		Console.WriteLine("Oh no. :c");
	}
}
```

To create this entity somewhere, use

```C#
new Foxe(Layer, new Vector2(200, 100));
```



## [<< Input](../Input.md)	|	[Introduction to ECS >>](IntroductionToECS.md)

[<<< Contents](../Contents.md)