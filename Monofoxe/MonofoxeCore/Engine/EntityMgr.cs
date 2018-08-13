using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using System.Diagnostics;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine
{
	public static class EntityMgr
	{

		/// <summary>
		/// List of all game objects.
		/// </summary>
		private static List<Entity> _entities = new List<Entity>();

		/// <summary>
		/// List of newly created game objects. Since it won't be that cool to modify main list 
		/// in mid-step, they'll be added in next one.
		/// </summary>
		private static List<Entity> _newEntities = new List<Entity>();

		/// <summary>
		/// Entity list sorted by depth.
		/// </summary>
		private static List<Entity> _depthSortedEntities = new List<Entity>();


		/// <summary>
		/// Counts time until next fixed update.
		/// </summary>
		private static double _fixedUpdateTimer;


		internal static void Update(GameTime gameTime)
		{		
			// Clearing main list from destroyed objects.
			var updatedList = new List<Entity>();
			foreach(Entity obj in _entities)
			{
				if (!obj.Destroyed)
				{
					updatedList.Add(obj);
				}
			}
			_entities = updatedList;
			// Clearing main list from destroyed objects.


			// Adding new objects to the list.
			_entities.AddRange(_newEntities);		
			_newEntities.Clear();
			// Adding new objects to the list.

			ECSMgr.Create();

			// Fixed updates.
			_fixedUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;

			if (_fixedUpdateTimer >= GameMgr.FixedUpdateRate)
			{
				var overflow = (int)(_fixedUpdateTimer / GameMgr.FixedUpdateRate); // In case of lags.
				_fixedUpdateTimer -= GameMgr.FixedUpdateRate * overflow;

				ECSMgr.FixedUpdate();
				foreach(Entity obj in _entities)
				{
					if (obj.Active)
					{
						obj.FixedUpdateBegin();
					}
				}

				ECSMgr.FixedUpdateBegin();
				foreach(Entity obj in _entities)
				{
					if (obj.Active)
					{
						obj.FixedUpdate();
					}
				}

				ECSMgr.FixedUpdateEnd();
				foreach(Entity obj in _entities)
				{
					if (obj.Active)
					{
						obj.FixedUpdateEnd(); 
					}
				}
			}
			// Fixed updates.


			// Normal updates.
			ECSMgr.UpdateBegin();
			foreach(Entity obj in _entities)
			{
				if (obj.Active)
				{
					obj.UpdateBegin();
				}
			}

			ECSMgr.Update();
			foreach(Entity obj in _entities)
			{
				if (obj.Active)
				{ 
					obj.Update(); 
				}
			}

			ECSMgr.UpdateEnd();
			foreach(Entity obj in _entities)
			{
				if (obj.Active)
				{ 
					obj.UpdateEnd();
				}
			}
			// Normal updates.


			// Updating depth list for drawing stuff.
			_depthSortedEntities = _entities.OrderByDescending(o => o.Depth).ToList();
			ECSMgr.SortComponentsByDepth();
		}



		internal static void Draw()
		{
			ECSMgr.DrawBegin();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.DrawBegin();
				}
			}

			ECSMgr.Draw();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.Draw();
				}
			}
			
			ECSMgr.DrawEnd();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.DrawEnd();
				}
			}
		}


		internal static void DrawGUI()
		{
			ECSMgr.DrawGUI();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.DrawGUI();
				}
			}
		}




		/// <summary>
		/// Adds object to object list.
		/// </summary>
		internal static void AddEntity(Entity obj) => 
			_newEntities.Add(obj);
		

		#region User functions. 

		/// <summary>
		/// Returns list of objects of certain type.
		/// </summary>
		public static List<T> GetList<T>() where T : Entity => 
			_entities.OfType<T>().ToList();


		/// <summary>
		/// Counts amount of objects of certain type.
		/// </summary>
		public static int Count<T>() where T : Entity => 
			_entities.OfType<T>().Count();

		
		/// <summary>
		/// Destroys game object.
		/// </summary>
		public static void Destroy(Entity obj)
		{
			if (!obj.Destroyed)
			{
				obj.Destroyed = true;
				if (obj.Active)
				{
					obj.Destroy();
				}
				obj.RemoveAllComponents();
			}
		}


		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		public static bool ObjExists<T>() where T : Entity
		{
			foreach(Entity obj in _entities)
			{
				if (obj is T)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Finds n-th object of given type.
		/// </summary>
		/// <typeparam name="T">Type to search.</typeparam>
		/// <param name="count">Number of the object in object list.</param>
		/// <returns>Returns object if it was found, or null, if it wasn't.</returns>
		public static T ObjFind<T>(int count) where T : Entity
		{
			var counter = 0;

			foreach(Entity obj in _entities)
			{
				if (obj is T)
				{
					if (counter >= count)
					{
						return (T)obj;
					}
					counter += 1;
				}
			}
			return null;
		}

		#endregion User functions.


		#region ECS functions.

		/// <summary>
		/// Returns list of objects of certain type.
		/// </summary>
		public static List<Entity> GetList(string tag)
		{
			var list = new List<Entity>();

			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					list.Add(obj);
				}
			}
			return list;
		}
		

		/// <summary>
		/// Counts amount of objects of certain type.
		/// </summary>
		public static int Count(string tag)
		{
			var counter = 0;

			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					counter += 1;
				}
			}
			return counter;
		}
		

		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		public static bool ObjExists(string tag)
		{
			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					return true;
				}
			}
			return false;
		}
		

		/// <summary>
		/// Finds n-th object of given type.
		/// </summary>
		/// <typeparam name="T">Type to search.</typeparam>
		/// <param name="count">Number of the object in object list.</param>
		/// <returns>Returns object if it was found, or null, if it wasn't.</returns>
		public static Entity ObjFind(string tag, int count)
		{
			var counter = 0;

			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					if (counter >= count)
					{
						return obj;
					}
					counter += 1;
				}
			}
			return null;
		}

		#endregion ECS functions.

	}
}
