# v 2.0.0.0-dev:

### IN THIS BUILD:
- Added `VertexBatch` class.
- Replaced GraphicsMgr's internal SpriteBatch with `VertexBatch`
- Moved graphics states from `GraphicsMgr` to `VertexBatch`.
- Moved matrix stack from `GraphicsMgr` to `VertexBatch`.
- Added per-vertex z depth for sprites, surfaces and frames.
- Fixed `Origin` property not being used in `Surface`.

### FEATURES:

- Documentation!
- Added `ResourceHub` and `ResourceBox` instead of old loading systems.
- Added `Angle` class for better angle management.
- Added .NET Standard library template.
- Nopipeline's NPL config supports adding references with environment variables.
- Monofoxe is now able to launch on Android.
- Added an ability to change entity update order.
- Added an all-in-one multiplatform project template.
- Added various item templates.
- Added `VertexBatch` class.
- Added per-vertex z depth for sprites, surfaces and frames.

### CHANGES:

- Camera implements `IDisposable` interface now.
- `Alarm`, `AutoAlarm` and `Animation` use `EventHandler` instead of `Action` now.
- Spritegroup cstemplates doesn't require quotes for variable values now.
- Changed Draw methods in `Frame`, `Sprite` and `Surface` to use their properties by default instead of default struct values.
- Moved `animation` argument in `Sprite.Draw` method after `position`.
- Specifying origin in `Sprite.Draw()` isn't mandatory anymore. 
- `Frame`, `Sprite` and `Surface`'s `Rotation` field is `Angle` instead of `float` now.
- `GameMath` doesn't contain angle-related methods anymore. They are moved to `Angle` instead.
- All Monofoxe libraries are .NET Standard now.
- Nopipeline is now embedded into Monofoxe.
- All projects reference Monofoxe libraries from common place instead of raw per-project libraries.
- Replaced static methods in `TimeKeeper` with static `Global` instance.
- Removed drawing methods which work with raw x;y.
- Project templates for VS2019 now have tags.
- Bumped .NET Framework version to 4.7.2 for templates.
- Entity methods which count components/entities have been removed.
- Systems have been removed entirely.
- Components now have their own events.
- Calling `base.%EventName%()` is now required in entities for EC to work.
- Replaced GraphicsMgr's internal `SpriteBatch` with `VertexBatch`
- Moved graphics states from `GraphicsMgr` to `VertexBatch`.
- Moved matrix stack from `GraphicsMgr` to `VertexBatch`.

### FIXES:

- Layer depth sorting now works properly.
- `CameraMgr.Cameras` is a List instead of `IReadOnlyColection` now.
- `KeepAspestRatio` canvas mode now scales canvas correctly.
- Fixed memory leak in `Camera`.
- Fixed `BasicTilemapSystem` not drawing the very last row and column of tiles.
- Nopipeline now works with paths which contain spaces.
- Angle difference formula now works properly.
- Uninstaller now appears in Add\Remove Programs section.
- Fixed various project warnings.
- Fixed `Origin` property not being used in `Surface`.


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



