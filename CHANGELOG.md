# [Changelog](http://keepachangelog.com/en/1.0.0/):

## [Unreleased]

### Added:

- Added `Rotate` method for `Vector2`.
- Added half-pixel offset support for `VertexBatch`.
- Added Pre and Post events to layers, scenes and scene manager.
- Added `ZDepth` to all shape classes.

### Changed:

- `Input.ScrollWheelValue` now returns signed scroll speed value instead of only its sign.
- Reworked `Alarm` class and merged it with `AutoAlarm` and `Timer`.
- Changed `IDrawable` interface to `Drawable` class.

### Removed:

- Removed `AutoAlarm` and `Timer` classes.
- Removed `MousePosition` from `Input` class.

### Fixed:

- Fixed instantiated rectangle shape not being drawn properly.
- Fixed entity methods crashing the game after creating new `Entity`.
- Fixed crashes when `Scene`'s layer methods were called.
- Fixed crashes when `SceneMgr`'s scene methods were called.
- Fixed `Component.Initialized` never being set.
- Fixed gamepad press/release not working correctly.
- Fixed gamepad index not being used in input methods.

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

