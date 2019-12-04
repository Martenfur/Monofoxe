# Update order

Now with all the scene stuff out of the way, it's important to know what executes when.

Here's some pseudocode of what happens under the hood:

```c#
if (FixedUpdateTimerIsTriggered)
{
	foreach(var scene in scenes)
	{
		foreach(var layer in scene.Layers)
		{
			foreach(var entity in layer.Entities)
				FixedUpdateForEntity();
		}
	}
}
```

First event to execute is `FixedUpdate()`. It only executes if its internal timer is up. The code iterates through all scenes, layers, entities and executes their FixedUpdate events. Component's events are evecuted inside the entity's `Update()`.

After `FixedUpdate()`, regular `Update()` is executed:

```c#
foreach(var scene in scenes)
{
	foreach(var layer in scene.Layers)
	{
		foreach(var entity in layer.Entities)
			UpdateForEntity();
	}
}
```

It's pretty much the same thing as `FixedUpdate`, but without the timer.

After `Update()`, regular `Draw()` is executed:

```c#
foreach(var camera in cameras)
{
	SetCameraMatrixAndSurface();
	foreach(var scene in scenes)
	{
		foreach(var layer in scene.Layers)
		{
			if (layer.IsGUI)
				continue;
		
			foreach(var entity in layer.DepthSortedEntities)
			{
				DrawForEntity();
			}
		}
	}
}
```

The main difference now is that `Draw()` executes for each camera separately. So if you have two cameras, each entity will be drawn twice. Also here's where `IsGUI` flag comes into play. If the layer is considered as GUI, it can't be drawn on camera and is not executed. 

After all that, we execute `Draw()` again, but for layers marked as GUI:

```c#
foreach(var scene in scenes)
{
	foreach(var layer in scene.Layers)
	{
		if (!layer.IsGUI)
			continue;
		
		foreach(var entity in layer.DepthSortedEntities)
		{
			DrawForEntity();
		}
	}
}
```

This time we don't have a camera, so layers are drawn directly on backbuffer and not affected by any camera.



## [<< Cameras](Cameras.md) | [??? >>](.md)   

[<<< Contents](../Contents.md)

