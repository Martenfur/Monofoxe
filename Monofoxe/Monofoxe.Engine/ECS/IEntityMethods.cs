using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Methods for working with entities.
	/// 
	/// NOTE: Methods should return all entities regardless of if they are enabled. 
	/// </summary>
	interface IEntityMethods
	{
		/// <summary>
		/// Returns list of entities of certain type.
		/// </summary>
		List<T> GetEntityList<T>() where T : Entity;
		
		/// <summary>
		/// Counts amount of entities of certain type.
		/// </summary>
		int CountEntities<T>() where T : Entity;

		/// <summary>
		/// Checks if any instances of an entity exist.
		/// </summary>
		bool EntityExists<T>() where T : Entity;

		/// <summary>
		/// Finds first entity of given type.
		/// </summary>
		T FindEntity<T>() where T : Entity;
		
		

		/// Due to ECS, there may be lots of objects with same type, 
		/// but different component sets. They differ only by their tag.
		/// This is, why we need tag overloads.

		/// <summary>
		/// Returns list of entities with given tag.
		/// </summary>
		List<Entity> GetEntityList(string tag);
		
		/// <summary>
		/// Counts amount of entities with given tag.
		/// </summary>
		int CountEntities(string tag);
		
		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		bool EntityExists(string tag);
		
		/// <summary>
		/// Finds first entit with given tag.
		/// </summary>
		Entity FindEntity(string tag);


		
		/// <summary>
		/// Returns list of entities, which have component - enabled or disabled -  of given type.
		/// </summary>
		List<Entity> GetEntityListByComponent<T>() where T : Component;
		
		/// <summary>
		/// Counts amount of entities, which have component - enabled or disabled - of given type.
		/// </summary>
		int CountEntitiesByComponent<T>() where T : Component;
		
		/// <summary>
		/// Finds first entity, which has component - enabled or disabled -  of given type.
		/// </summary>
		Entity FindEntityByComponent<T>() where T : Component;



		/// <summary>
		/// Returns list of components - enabled and disabled - of given type.
		/// </summary>
		List<Component> GetComponentList<T>() where T : Component;
		
	}
}
