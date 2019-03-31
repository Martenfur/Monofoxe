using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.Cameras;
using Monofoxe.Engine.Utils.CustomCollections;

namespace Monofoxe.Engine.SceneSystem
{
	/// <summary>
	/// Manager of all scenes.
	/// </summary>
	public static class SceneMgr
	{
		/// <summary>
		/// List of all scenes.
		/// </summary>
		public static IReadOnlyCollection<Scene> Scenes => _scenes.ToList();
		internal static SafeSortedList<Scene> _scenes = new SafeSortedList<Scene>(x => x.Priority);

		/// <summary>
		/// Current active scene.
		/// </summary>
		public static Scene CurrentScene {get; internal set;}

		/// <summary>
		/// Current active layer.
		/// </summary>
		public static Layer CurrentLayer {get; internal set;}
		
		
		/// <summary>
		/// Creates new scene with given name.
		/// </summary>
		public static Scene CreateScene(string name)
		{
			var scene = new Scene(name);
			_scenes.Add(scene);
			return scene;
		}

		/// <summary>
		/// Destroys given scene.
		/// </summary>
		public static void DestroyScene(Scene scene)
		{
			scene.Destroy();
			_scenes.Remove(scene);
		}


		/// <summary>
		/// Destroys scene with given name.
		/// </summary>
		public static void DestroyScene(string name)
		{
			for(var i = _scenes.Count - 1; i >= 0; i += 1)
			{
				if (string.Equals(_scenes[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					_scenes[i].Destroy();
					_scenes.Remove(_scenes[i]);
				}
			}
		}


		/// <summary>
		/// Returns layer with given name.
		/// </summary>
		public static Scene GetScene(string name)
		{
			foreach(var scene in _scenes)
			{
				if (string.Equals(scene.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return scene;
				}
			}
			return null;
		}


		/// <summary>
		/// Returns true, if there is a layer with given name. 
		/// </summary>
		public static bool SceneExists(string name)
		{
			foreach(var scene in _scenes)
			{
				if (string.Equals(scene.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
		



		/// <summary>
		/// Counts time until next fixed update.
		/// </summary>
		private static double _fixedUpdateTimer;


		internal static void PreUpdateRoutine()
		{
			// Clearing main list from destroyed objects.
			foreach(var scene in Scenes)
			{
				foreach(var layer in scene.Layers)
				{
					layer.UpdateEntityList();
				}
			}
			
			SystemMgr.UpdateActiveSystems();
		
		}


		internal static void PostUpdateRoutine()
		{
			// Updating depth list for drawing stuff.
			foreach(var scene in _scenes)
			{
				foreach(var layer in scene.Layers)
				{
					layer.SortByDepth();
				}
			}
		}


		internal static void CallFixedUpdateEvents(GameTime gameTime)
		{
			_fixedUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;

			if (_fixedUpdateTimer >= GameMgr.FixedUpdateRate)
			{
				var overflow = (int)(_fixedUpdateTimer / GameMgr.FixedUpdateRate); // In case of lags.
				_fixedUpdateTimer -= GameMgr.FixedUpdateRate * overflow;

				TimeKeeper._elapsedTime = GameMgr.FixedUpdateRate;

				foreach(var scene in _scenes)
				{
					if (scene.Enabled)
					{
						CurrentScene = scene;
						
						foreach(var layer in scene.Layers)
						{
							if (layer.Enabled)
							{
								CurrentLayer = layer;
								SystemMgr.FixedUpdate(layer._components);
								foreach(var entity in layer.Entities)
								{
									if (entity.Enabled && !entity.Destroyed)
									{
										entity.FixedUpdate();
									}
								}
							}
						}
					}
				}
			}
		}

		internal static void CallUpdateEvents(GameTime gameTime)
		{
			TimeKeeper._elapsedTime = GameMgr.ElapsedTime;
			
			foreach(var scene in _scenes)
			{		
				if (scene.Enabled)
				{
					CurrentScene = scene;
					
					foreach(var layer in scene.Layers)
					{
						if (layer.Enabled)
						{
							CurrentLayer = layer;
							SystemMgr.Update(layer._components);

							foreach(var entity in layer.Entities)
							{
								if (entity.Enabled && !entity.Destroyed)
								{
									entity.Update(); 
								}
							}
						}
					}
				}
			}
			
		}
		

		/// <summary>
		/// Executes Draw events.
		/// </summary>
		internal static void CallDrawEvents()
		{
			foreach(var scene in _scenes)
			{
				if (scene.Visible)
				{
					CurrentScene = scene;
					foreach(var layer in scene.Layers)
					{
						if (
							layer.Visible && 
							!layer.IsGUI && 
							!DrawMgr.CurrentCamera.Filter(scene.Name, layer.Name)
						)
						{
							CurrentLayer = layer;

							bool hasPostprocessing = (
								DrawMgr.CurrentCamera.PostprocessingMode == PostprocessingMode.CameraAndLayers 
								&& layer.PostprocessorEffects.Count > 0
							);

							if (hasPostprocessing)
							{
								DrawMgr.SetSurfaceTarget(DrawMgr.CurrentCamera._postprocessorLayerBuffer, DrawMgr.CurrentTransformMatrix);
								DrawMgr.Device.Clear(Color.TransparentBlack);
							}

							foreach(var entity in layer._depthSortedEntities)
							{
								if (entity.Visible && !entity.Destroyed)
								{
									foreach(var componentPair in entity._components)
									{
										if (componentPair.Value.Visible)
										{
											SystemMgr.Draw(componentPair.Value);
										}
									}
									entity.Draw();
								}
							}

							if (hasPostprocessing)
							{
								DrawMgr.ResetSurfaceTarget();

								var oldRasterizer = DrawMgr.Rasterizer;
								DrawMgr.Rasterizer = DrawMgr._cameraRasterizerState;
								DrawMgr.SetTransformMatrix(Matrix.CreateTranslation(Vector3.Zero));
								layer.ApplyPostprocessing();
								DrawMgr.ResetTransformMatrix();
								DrawMgr.Rasterizer = oldRasterizer;
							}

						}

					}
				}
			}
		}
		
		/// <summary>
		/// Executes Draw GUI events.
		/// </summary>
		internal static void CallDrawGUIEvents()
		{
			foreach(var scene in _scenes)
			{
				if (scene.Visible)
				{
					CurrentScene = scene;
					foreach(var layer in scene.Layers)
					{
						if (layer.Visible && layer.IsGUI)
						{
							CurrentLayer = layer;
							foreach(var entity in layer._depthSortedEntities)
							{
								if (entity.Visible && !entity.Destroyed)
								{
									foreach(var componentPair in entity._components)
									{
										if (componentPair.Value.Visible)
										{
											SystemMgr.Draw(componentPair.Value);
										}
									}
									entity.Draw();
								}
							}
						}
					}
				}
			}
		}


	}
}
