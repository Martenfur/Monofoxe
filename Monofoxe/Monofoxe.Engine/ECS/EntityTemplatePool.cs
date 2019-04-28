using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Entity factory pool stores in instance of each entity factory.
	/// </summary>
	static class EntityTemplatePool
	{
		/// <summary>
		/// Pool of all factories in all assemblies. Sorted by their tags.
		/// </summary>
		public static Dictionary<string, IEntityTemplate> TemplatePool;

		/// <summary>
		/// Initialized template pool by creating an instance of each IEntityTemplateclass.
		/// </summary>
		public static void InitTemplatePool()
		{
			TemplatePool = new Dictionary<string, IEntityTemplate>();
			
			// Creating an instance of each.
			foreach(var type in GameMgr.Types)
			{
				if (typeof(IEntityTemplate).IsAssignableFrom(type.Value) && !type.Value.IsInterface)
				{
					var newTemplate = (IEntityTemplate)Activator.CreateInstance(type.Value);
					TemplatePool.Add(newTemplate.Tag.ToLower(), newTemplate);
				}
			}
		}
	}
}
