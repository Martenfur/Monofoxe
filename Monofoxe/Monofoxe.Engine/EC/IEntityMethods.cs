using System.Collections.Generic;

namespace Monofoxe.Engine.EC
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
		/// Checks if any instances of an entity exist.
		/// </summary>
		bool EntityExists<T>() where T : Entity;

		/// <summary>
		/// Finds first entity of given type.
		/// </summary>
		T FindEntity<T>() where T : Entity;
		
		
		/// <summary>
		/// Returns list of entities, which have component - enabled or disabled - of given type.
		/// </summary>
		List<Entity> GetEntityListByComponent<T>() where T : Component;
		
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
