# v 2.0.0.0-dev:

### FEATURES:

- Documentation!
- Added `ResourceHub` and `ResourceBox` instead of old loading systems.

### CHANGES:

- Camera implements `IDisposable` interface now.
- Alarms, AutoAlarms and Animations use `EventHandler` instead of `Action` now.
- Spritegroup cstemplates doesn't require quotes for variable values now.

### FIXES:

- Layer depth sorting now works properly.
- `CameraMgr.Cameras` is a List instead of `IReadOnlyColection` now.
- `KeepAspestRatio` canvas mode now scales canvas correctly.
- Fixed memory leak in `Camera`.
- Fixed `BasicTilemapSystem` not drawing the very last row and column of tiles.
- Nopipeline now works with paths which contain spaces.



<hr/>
# v 1.0.1.1

### FIXES:

- Fixed `MapBuilder` crashing when some tilesets are ignored.

<hr/>
# v1.0.1.0

### FEATURES:

- Added Animation class.
- Added Easing class.
- Added DirectionToVector2 methods to GameMath.
- Added a set of demos. See Monofoxe.Playground project.
- Added Windows DirectX templates.

### CHANGES:

- Moved Cameras from Monofoxe.Engine.Utils to Monofoxe.Engine namespace.
- Moved NumberExtensions and Vector2Extensions to Monofoxe.Engine namespace.
- Camera.Rotation is now float instead of int.
- Added GraphicsMgr.CurrentWorld.
- Renamed GraphicsMgr.CurrentTransformMatrix to GraphicsMgr.CurrentView.

### FIXES:

- `AlphaBlend.fx` now works for DirectX.
- Content.mgcb now correctly builds for cross-platform projects.
- `AddComponent()` now throws an exception, if trying to add a component owned by other entity.
- `AffectedBySpeedMultiplier` has been removed from alarms. It can be replaced by `TimeKeeper`.
- Fixed NoPipeline not saving cases of the resource names.
- Fixed NoPipeline's watcher not working with newly added files.
- Fixed crashing, when text is being drawn last in the frame.
- Renamed `Round/Ceiling/Floor` to `RoundV/CeilingV/FloorV` in `Vector2` extensions, which were causing name clashes.
- Fixed tile objects `GID` being offset by 1 when loaded from templates.
- Optimized `MapBuilder`.
- Optimized graphics pipeline.
- Fixed graphics not working properly on DirectX.
- Fixed `ToggleFullScreen` not working properly with Canvas.

<hr/>
# v1.0.0.0



