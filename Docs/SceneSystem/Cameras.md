# Cameras

*Trained foxes are working on this article.*

Camera is your window into the game. You can position, rotate and zoom it. There should be at least one camera, or else Draw events will not trigger. Though, it is still possible to draw without any cameras by setting `IsGUI` flag to true for layers.

Creating the camera is dead simple:

```c#
new Camera(800, 600, 0); // Creates new camera with the view size of (800;600) and a priority of 0.
```

**NOTE**: Each camera creates new surface for itself.

Cameras should be disposed.

```c#
camera.Dispose();
```

I recommend to create your cameras once and then reuse them though all of your game.

Cameras have quite a lot of fields:

- `Position` - Camera position in the game world.

- `Size` - Camera's view size. Matches surface size. If you want to change the size, use `Resize()` method.

- `Offset` - Origin point of the camera in the world. Camera will rotate around that point. think of it the same way as the sprite origin.

- `Rotation` - Camera's rotation in the world. Measured in degrees.

- `Zoom` - Camera's zoom in the world. 

- `PortPosition` - Position of the camera surface on the backbuffer. Doesn't influence world position. 

- `PortOffset` - Surface offset on the backbuffer.

- `PortScale` - Surface scale on the backbuffer.

- `PortRotation` - Surface rotation on the backbuffer. Measured in degrees. **NOTE:** Rotating the surface will break world mouse positioning.

  This could be quite confusing - why do we need two sets of translation variables? Regular translation variables affect transform matrix of the game world which affects everything that is drawn within the camera. All of it is drawn on a camera surface which is then drawn on the backbuffer. This is where `Port` variables apply - while drawing the surface.

-  `Surface` - Camera surface. Everything camera sees is being drawn on it.

- `Priority` - Camera priority. If you have several cameras, ones with higher priority will be rendered first.

- `BackgroundColor` - This color will be used when clearing the surface at the beginning of the step.

- `ClearBackground` - If true, at the beginning of each step camera surface will be cleared with `BackgroundColor`.

- `TransformMatrix` - Transform matrix which is used to apply camera transformations. Updated each step. Use  `UpdateTransformMatrix()` to update it manually.

To get the mouse position relative to the camera, use `GetRelativeMousePosition()`.



## Filtering

You can tell the camera to not draw certain layers. To enable filtering, change `FilterMode` from the default value.

- `FilterMode.None` - Filtering is disabled. Set by default.
- `FilterMode.Inclusive` - Triggers rendering, if filter **does** contain layer.
- `FilterMode.Exclusive` - Triggers rendering, if filter **does not** contain layer.

To add layer into the filter, use `AddFilterEntry()`, and `RemoveFilterEntry()` to remove it.

You can use filtering to create certain effects coupled with shaders.



## Postprocessing

You can apply shaders to the camera surface after it's being rendered. To enable postprocessing, set `PostprocessingMode` from the default value.

- `PostprocessingMode.None` - Postprocessing is disabled.
- `PostprocessingMode.Camera` - Enables postprocessing for camera.
- `PostprocessingMode.CameraAndLayers` - Each layer can have its own postprocessing shaders. To use them, set postprocessing to this mode.

**NOTE:** Enabling postprocessing will create additional surface.

To add postprocessing shaders, use `PostprocessorEffects` list.



## [<< Scenes](Scenes.md) | [Update order >>](UpdateOrder.md)   

[<<< Contents](../Contents.md)

