using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.SceneSystem
{
	public static class SceneMgr
	{
		public static IReadOnlyCollection<Scene> Scenes => _scenes;

		private static List<Scene> _scenes = new List<Scene>();


		public static Scene CreateScene(string name)
		{
			var scene = new Scene(name);
			_scenes.Add(scene);
			return scene;
		}

		public static void DestroyScene(Scene scene)
		{
			scene.Destroy();
			_scenes.Remove(scene);
		}

		public static void DestroyScene(string name)
		{
			throw(new NotImplementedException());
		}

		/// <summary>
		/// Executes Draw events.
		/// </summary>
		internal static void CallDrawEvents()
		{
			foreach(var scene in _scenes)
			{
				foreach(var layer in scene.Layers)
				{
					if (!layer.IsGUI)
					{
						SystemMgr.Draw(layer._depthSortedComponents);
						foreach(var entity in layer._depthSortedEntities)
						{
							if (entity.Active && !entity.Destroyed)
							{
								entity.Draw();
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
				foreach(var layer in scene.Layers)
				{
					if (layer.IsGUI)
					{
						SystemMgr.Draw(layer._depthSortedComponents);
						foreach(var entity in layer._depthSortedEntities)
						{
							if (entity.Active && !entity.Destroyed)
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
