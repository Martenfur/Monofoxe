using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Utils.CustomCollections;

namespace Monofoxe.Engine.SceneSystem
{
	public delegate void SceneMgrEventDelegate();

	/// <summary>
	/// Manager of all scenes. Updates entities and components.
	/// </summary>
	public static class SceneMgr
	{
		/// <summary>
		/// List of all scenes.
		/// </summary>
		public static List<Scene> Scenes => _scenes.ToList();
		internal static SafeSortedList<Scene> _scenes = new SafeSortedList<Scene>(x => x.Priority);

		/// <summary>
		/// Current active scene.
		/// </summary>
		public static Scene CurrentScene {get; private set;}

		
		/// <summary>
		/// Counts time until next fixed update.
		/// </summary>
		private static double _fixedUpdateTimer;

		/// <summary>
		/// If true, all crashes within the scenes will be caught and OnCrash event will be called.
		/// </summary>
		public static bool CrashHandlingEnabled = false;
		
		/// <summary>
		/// Gets called whenever the code within a scene crashes.
		/// IMPORTANT: CrashHandlingEnabled has to be set to true!!!
		/// </summary>
		public static event Action<Scene, Exception> OnCrash;


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
		/// Returns scene with given name.
		/// </summary>
		public static Scene GetScene(string name)
		{
			for (var i = 0; i < _scenes.Count; i += 1)
			{
				if (string.Equals(_scenes[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return _scenes[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Finds scene with given name. Returns true, if scene was found.
		/// </summary>
		public static bool TryGetScene(string name, out Scene scene)
		{
			for (var i = 0; i < _scenes.Count; i += 1)
			{
				if (string.Equals(_scenes[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					scene = _scenes[i];
					return true;
				}
			}
			scene = null;
			return false;
		}

		/// <summary>
		/// Returns true, if there is a scene with given name. 
		/// </summary>
		public static bool HasScene(string name)
		{
			for(var i = 0; i < _scenes.Count; i += 1)
			{
				if (string.Equals(_scenes[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
		

		

		/// <summary>
		/// Routine needed before updating entities.
		/// </summary>
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
		}


		
		/// <summary>
		/// Routine needed after updating entities.
		/// </summary>
		internal static void PostUpdateRoutine()
		{
			// Updating depth list for drawing stuff.
			foreach(var scene in Scenes)
			{
				foreach(var layer in scene.Layers)
				{
					layer.SortByDepth();
				}
			}
		}


		#region Events.

		/// <summary>
		/// Triggers every frame before all scenes perform Update.
		/// </summary>
		public static event SceneMgrEventDelegate OnPreUpdate;
		/// <summary>
		/// Triggers every frame after all scenes perform Update.
		/// </summary>
		public static event SceneMgrEventDelegate OnPostUpdate;
		/// <summary>
		/// Triggers every frame before all scenes perform FixedUpdate.
		/// </summary>
		public static event SceneMgrEventDelegate OnPreFixedUpdate;
		/// <summary>
		/// Triggers every frame after all scenes perform FixedUpdate.
		/// </summary>
		public static event SceneMgrEventDelegate OnPostFixedUpdate;
		/// <summary>
		/// Triggers every frame before all non-GUI layers perform Draw.
		/// </summary>
		public static event SceneMgrEventDelegate OnPreDraw;
		/// <summary>
		/// Triggers every frame after all non-GUI layers perform Draw.
		/// </summary>
		public static event SceneMgrEventDelegate OnPostDraw;
		/// <summary>
		/// Triggers every frame before all GUI layers perform Draw.
		/// </summary>
		public static event SceneMgrEventDelegate OnPreDrawGUI;
		/// <summary>
		/// Triggers every frame after all GUI layers perform Draw.
		/// </summary>
		public static event SceneMgrEventDelegate OnPostDrawGUI;


		/// <summary>
		/// Executes Fixed Update events.
		/// </summary>
		internal static void CallFixedUpdateEvents(GameTime gameTime)
		{
			_fixedUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;

			if (_fixedUpdateTimer >= GameMgr.FixedUpdateRate)
			{
				var overflow = (int)(_fixedUpdateTimer / GameMgr.FixedUpdateRate); // In case of lags.
				_fixedUpdateTimer -= GameMgr.FixedUpdateRate * overflow;

				TimeKeeper._elapsedTime = GameMgr.FixedUpdateRate;

				OnPreFixedUpdate?.Invoke();
				foreach(var scene in Scenes)
				{		
					if (scene.Enabled)
					{
						CurrentScene = scene;

						if (CrashHandlingEnabled)
						{
							try
							{
								scene.FixedUpdate();
							}
							catch (Exception e)
							{
								OnCrash?.Invoke(scene, e);
							}
						}
						else
						{
							scene.FixedUpdate();
						}
					}
				}
				OnPostFixedUpdate?.Invoke();
			}
		}


		/// <summary>
		/// Executes Update events.
		/// </summary>
		internal static void CallUpdateEvents(GameTime gameTime)
		{
			TimeKeeper._elapsedTime = GameMgr.ElapsedTime;

			OnPreUpdate?.Invoke();
			foreach (var scene in Scenes)
			{		
				if (scene.Enabled)
				{
					CurrentScene = scene;

					if (CrashHandlingEnabled)
					{
						try
						{
							scene.Update();
						}
						catch(Exception e)
						{
							OnCrash?.Invoke(scene, e);
						}
					}
					else
					{
						scene.Update();
					}
				}
			}
			OnPostUpdate?.Invoke();
		}
		
		
		/// <summary>
		/// Executes Draw events.
		/// </summary>
		internal static void CallDrawEvents()
		{
			OnPreDraw?.Invoke();
			foreach (var scene in Scenes)
			{
				if (scene.Visible)
				{
					CurrentScene = scene;

					if (CrashHandlingEnabled)
					{
						try
						{
							scene.Draw();
						}
						catch (Exception e)
						{
							OnCrash?.Invoke(scene, e);
						}
					}
					else
					{
						scene.Draw();
					}
				}
			}
			OnPostDraw?.Invoke();
		}
		

		/// <summary>
		/// Executes Draw GUI events.
		/// </summary>
		internal static void CallDrawGUIEvents()
		{
			OnPreDrawGUI?.Invoke();
			foreach (var scene in Scenes)
			{
				if (scene.Visible)
				{
					CurrentScene = scene;

					if (CrashHandlingEnabled)
					{
						try
						{
							scene.DrawGUI();
						}
						catch (Exception e)
						{
							OnCrash?.Invoke(scene, e);
						}
					}
					else
					{
						scene.DrawGUI();
					}
				}
			}
			OnPostDrawGUI?.Invoke();
		}

		#endregion Events.


	}
}
