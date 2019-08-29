# Layers

Entities could very well execute in a single huge pile, but this becomes a bit problematic. Let's say, you got some entities on the background, on the foreground and some in-between. How would you know which are which? Sure, you can remember it yourself, or put some flags, but this is just not convenient.

So instead, we use layers. `Layer` is a class residing in `Monofoxe.Engine.SceneSystem`. Its only purpose is to be a container for entities. In fact, you cannot create an entity without specifying a layer. So, if we want background and foreground, we just create a layer for each.

To create a new layer, use this code:

```c#
YourScene.CreateLayer("layerName");
```

You can also see that we create it through `Scene` class. Don't worry about it for now, all you need to know - scenes are containers for layers, just like layers are containers for entities.

To destroy the layer, use this:

```C#
YourScene.DestroyLayer("layerName");
```

Also note that this will destoy any entity which belongs to this layer.

When the game is launched, it automatically creates a scene named "default" with a layer named "default". 

Layer also has several fields:

- `Scene` - Owner scene of a layer.
- `Name` - The name of the layer. Should be unique within the scene.
- `Enabled` - If false, entities and components will not execute their `Update()` and `FixedUpdate()` methods.
- `Visible` - If false, entities and components will not execute their `Draw()` methods.
- `IsGUI` - If true, draws everything directly to the backbuffer instead of cameras.
- `DepthSorting` - If true, entities will be sorted by their `Depth` when being drawn. False by default. **NOTE:** Depth sorting is pretty slow, so avoid it when you can. 
- `Priority` - Layers with higher priority will be executed first within their scene.
- `PostprocessorEffects` - List of effects which will be applied to the layer when it's done rendering and camera has postprocessor effects enabled for the layers.

Additionally, there is a bunch of selection methods which search for certain entities within the layer, you can look them up in sources.

You also can move entities to different layers. To do that, assign their `Layer` field to a different layer.

## [<< Systems](../Entities/Systems.md) | [Scenes >>](Scenes.md)   

[<<< Contents](../Contents.md)

