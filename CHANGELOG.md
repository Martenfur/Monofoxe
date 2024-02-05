# [Changelog](http://keepachangelog.com/en/1.0.0/):

## [Unreleased]

### Added

- Added `RenderMask` as a replacement to camera filters to `Scene`, `Layer` and `Entity`. 
- Added `Clear()` method to `ResourceBox`.
- Added `Offset()` method to linear dampers.
- Added `OnFrameStart`, `OnFrameFinish` and `OnAfterDraw` events to `GraphicsMgr`.
- Added `Pool` collection.
- Added `AccumulationBuffer` collection.
- Added `UnorderedList` collection.
- Added `GetArea()`, `GetSignedArea()`, `IsClockwise()` methods to `GameMath`.
- Added new collision system.

### Changed
- **BREAKING CHANGE:** `ResourceInfoMgr` now accepts wildcards instead of directory names. For example, `ResourceInfoMgr.GetResourcePaths("Graphics/Trees");` should now be replaced with `ResourceInfoMgr.GetResourcePaths("Graphics/Trees/*");`
- **BREAKING CHANGE:** Renamed `GetSafeNormalize()` to `SafeNormalize()`.

### Fixed

- Fixed `AddComponent<>()` not taking generic type into account.
- `DirectoryResourceBox` now ignores non-xnb files properly.

### Removed
- **BREAKING CHANGE:** Removed camera layer filters.

## [v2.2.0] - *17.09.2022*

### Added

- Added `Name` field to `Sprite`.
- Added `Range<>` class.
- Added HEX conversion for `Color`.
- Added `HsvColor` Lerp and basic operators.
- Added simplified `GetResource<>()` methods.
- Added `OnResize` event to `Camera`.
- Added `Global` instance to `RandomExt`.
- Added unit circle methods to `RandomExt`.
- Added dampers based on second order motion.
- Added job system.
- Added `started` argument to `Alarm` constructor. 
- Added `Start(time)` and `Pause(), Resume()` metods to `Alarm`.
- Added `Counter`.
- Added ` GetOrCreateLayer` to `Scene`.
- Added `OnFinish` event to coroutines.
- Added `Projection` method to `Vector2` and `Vector3`.
- Added `PerlinNoise`.
- Added `SlowRotator`.
- Added simple logger.

### Changed

- `Destroy()` event triggers even if an entity is disabled.
- Migrated to NET6.
- Updated MonoGame to `3.8.1`.
- Updated Nopipeline to `2.2.0`.
- Replaced `Newtonsoft.Json` dependency with `System.Text.Json`.
- Removed VS2019 support.

### Fixed

- Fixed `Sprite.Clone()` not copying frames properly.



## [v2.1.0] - *12.08.2021*

### Changed

- Removed VS2017 template support and added VS2022 templates.
- Solution created from template no longer requires to manually build Content project.
- Changed `Component` methods from abstract to virtual.
- Templates now support .NET5 instead of netcore 3.

### Added

- Added support for the `backgroundcolor` property for Tiled maps.
- Added `OnCrash` event to `SceneMgr` which allows to catch and recover from the exceptions within a scene.
- Added `IsFixedUpdateFrame` to `SceneMgr`.
- Added coroutines.

### Fixed:

- Fixed `Scene` not being able to delete its layers properly.
- Made `SafeList` thread safe.
- Newly created solutions set their default project to GL instead of library, which cannot be run.

## [v2.0.0] - *27.08.2020*

### Added:

- Added `Rotate` method for `Vector2`.
- Added half-pixel offset support for `VertexBatch`.
- Added Pre and Post events to layers, scenes and scene manager.
- Added `ZDepth` to all shape classes.
- Added platform-specific projects for WindowsDX and DesktopGL.
- Added `StuffResolver` class.
- Added `HsvColor` class.
- Added automatic content loaders.
- Added sprite json mathematical expressions.
- Added `CurrentPlatform` and `CurrentGraphicsBackend` to `GameMgr`.

### Changed:

- **BREAKING CHANGE:** Monogame version has been updated to 3.8.
- Pipefoxe now supports netstandard2.0 and is fully crossplatform.
- Renamed Pipefoxe to Monofoxe.Pipeline.
- **BREAKING CHANGE:** `Input.ScrollWheelValue` now returns signed scroll speed value instead of only its sign.
- **BREAKING CHANGE:** Reworked `Alarm` class and merged it with `AutoAlarm` and `Timer`.
- Changed `IDrawable` interface to `Drawable` class.
- `Entity.AddComponent` now returns the component class which was passed into it.
- **BREAKING CHANGE:** `AssetInfo` has been renamed to `ResourceInfo`
- **BREAKING CHANGE:** `ResourceInfoImporter` now imports `.npl` Content file instead of `.mgcb`.
- Removed dependency on Windows-only `System.Drawing` for Monofoxe.Pipeline. 
- Changed the crossplatform project structure.
- **BREAKING CHANGE:** `AlphaBlend.fx` is now baked into the library and doesn't have to be manually put into the Content directory.
- Replaced offset_x/y with originX/Y in sprite jsons.
- `Component.Destroy()` is now called when the component is removed from the entity.

### Removed:

- **BREAKING CHANGE:** Removed `AutoAlarm` and `Timer` classes.
- **BREAKING CHANGE:** Removed `MousePosition` from `Input` class.
- **BREAKING CHANGE:** Removed entity templates.
- Removed all templates except Crossplatform, since there is no need in them anymore.

### Fixed:

- Fixed instantiated rectangle shape not being drawn properly.
- Fixed entity methods crashing the game after creating new `Entity`.
- Fixed crashes when `Scene`'s layer methods were called.
- Fixed crashes when `SceneMgr`'s scene methods were called.
- Fixed `Component.Initialized` never being set.
- Fixed gamepad press/release not working correctly.
- Fixed gamepad index not being used in input methods.
- Fixed parsing multiplne text properties from Tiled maps.

## [v2.0.0.0-dev+007] - *07.02.2020*

## Fixed:

- Fixed `VertexBatch.Effect` being reset every frame.
- Fixed crashing when adding/removing components during component event.
- Fixed `Text.Color` not being used.

<hr/>

## [v2.0.0.0-dev+006] - *07.01.2020*

## Added:

- Added `ZNearPlane` and `ZFarPlane` to the `Camera`.
- Added projection matrix to the camera.
- Added an option to set custom projection matrix to the Surface.

### Changed:

- Made `Camera` abstract class and added `Camera2D` class.
- `Camera`'s `Position` and `Origin` are `Vector3` instead of `Vector2` now.
- `Primitive2D` now uses array of vertices instead of a list.

## Fixed:

- Fixed circles not being drawn in some cases.
- Fixed project templates.

<hr/>

## [v2.0.0.0-dev+005] - 12.12.2019

## Added:

- Added `VertexBatch` class.
- Added per-vertex z depth for sprites, surfaces and frames.

## Changed:
- Replaced `GraphicsMgr`'s internal `SpriteBatch` with `VertexBatch`
- Moved graphics states from `GraphicsMgr` to `VertexBatch`.
- Moved matrix stack from `GraphicsMgr` to `VertexBatch`.

## Fixed:
- Fixed `Origin` property not being used in `Surface`.

<hr/>

## [v2.0.0.0-dev+004] - *01.12.2019*

## Added:

- Monofoxe is now able to launch on Android.
- Added an ability to change entity update order.
- Added an all-in-one multiplatform project template.
- Added various item templates.

## Changed:
- Components now have their own events.
- Calling `base.%EventName%()` is now required in entities for EC to work.

## Removed:

- Entity methods which count components/entities.
- Systems have been removed entirely.

<hr/>

## [v2.0.0.0-dev+003] - *25.10.2019*

### Changed:

- Bumped .NET Framework version to 4.7.2 for templates.

### Fixed:

- Fixed various project warnings.

<hr/>

## [v1.0.1.1] - *30.06.2019*

### Fixed:

- Fixed `MapBuilder` crashing when some tilesets are ignored.

<hr/>

## [v1.0.1.0] - *29.06.2019*

### Added:

- Added Animation class.
- Added Easing class.
- Added DirectionToVector2 methods to GameMath.
- Added a set of demos. See Monofoxe.Playground project.
- Added Windows DirectX templates.

### Changed:

- Moved Cameras from Monofoxe.Engine.Utils to Monofoxe.Engine namespace.
- Moved NumberExtensions and Vector2Extensions to Monofoxe.Engine namespace.
- Camera.Rotation is now float instead of int.
- Added GraphicsMgr.CurrentWorld.
- Renamed GraphicsMgr.CurrentTransformMatrix to GraphicsMgr.CurrentView.

### Fixed:

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

## [v1.0.0.0] - *26.06.2019*

### Added
- **It's alive!!!**

