using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	public interface IEntityMethods
	{
		/// <summary>
		/// Returns list of objects of certain type.
		/// </summary>
		List<T> GetList<T>() where T : Entity;
		
		/// <summary>
		/// Counts amount of objects of certain type.
		/// </summary>
		int Count<T>() where T : Entity;

		/// <summary>
		/// Checks if any instances of an entity exist.
		/// </summary>
		bool EntityExists<T>() where T : Entity;

		/// <summary>
		/// Finds first entity of given type.
		/// </summary>
		T FindEntity<T>() where T : Entity;
		
		

		/// Due to ECS fun, there may be lots of objects with same type, 
		/// but different component sets. They differ only by their tag.
		/// This is why we need tag overloads.

		/// <summary>
		/// Returns list of entities with given tag.
		/// </summary>
		List<Entity> GetList(string tag);
		
		/// <summary>
		/// Counts amount of entities with given tag.
		/// </summary>
		int Count(string tag);
		
		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		bool EntityExists(string tag);
		
		/// <summary>
		/// Finds first entit with given tag.
		/// </summary>
		Entity FindEntity(string tag);
	}
}
