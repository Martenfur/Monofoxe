using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.SceneSystem
{
	public static class SceneMgr
	{
		public static IReadOnlyCollection<Scene> Scenes => _scenes;

		public static Scene CurrentScene {get; internal set;}
		public static Layer CurrentLayer {get; internal set;}

		private static List<Scene> _scenes = new List<Scene>();

		
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
				if (_scenes[i].Name == name)
				{
					_scenes[i].Destroy();
					_scenes.RemoveAt(i);
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
				if (scene.Name == name)
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
				if (scene.Name == name)
				{
					return true;
				}
			}
			return false;
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
							SystemMgr.Draw(layer._depthSortedComponents);
							foreach(var entity in layer._depthSortedEntities)
							{
								if (entity.Visible && !entity.Destroyed)
								{
									entity.Draw();
								}
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
							SystemMgr.Draw(layer._depthSortedComponents);
							foreach(var entity in layer._depthSortedEntities)
							{
								if (entity.Visible && !entity.Destroyed)
								{
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
