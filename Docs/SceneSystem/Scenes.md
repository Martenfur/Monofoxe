# Scenes

As been said before, scenes are containers for layers. Scenes serve as means to further organize your entities. For example, you can have a scene for gameplay entities, a scene for pause menu and a scene for global entities which should store information between levels.

Creating a new scene is done like this:

```C#
var scene = SceneMgr.CreateScene("sceneName");
scene.CreateLayer("layerName"); // Scene is created with no layers, so we need to create one to be able to add entities.
```

Also note that layer cannot exist by itself - you always need at least one scene.

Destroying the scene:

```C#
SceneMgr.DestryScene("sceneName");
```

Note that destroying scene will also destroy all its layers, which in turn will destroy all entities in them. Unlike entities, you cannot move a layer to a different scene.

Layers within the scene can be accessed like this:

```C#
scene["layerName"];
```

Scene has fields similar to the layer - except `IsGUI`. Additionaly, you can access entities directly through the scene with the same methods as in the layer - the only difference is that now you'll select entities from all the layers.  



## [<< Layers](Layers.md) | [Update order >>](UpdateOrder.md)    

[<<< Contents](../Contents.md)

