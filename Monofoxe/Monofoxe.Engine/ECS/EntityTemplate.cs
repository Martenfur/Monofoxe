using System;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Entity template. Used to store info about entities.
	/// </summary>
	public class EntityTemplate
	{
		/// <summary>
		/// Template tag. Will be assigned to entity during its creation.
		/// </summary>
		public readonly string Tag;

		/// <summary>
		/// Template's list of components.
		/// </summary>
		public readonly Component[] Components;

		public EntityTemplate(string tag, Component[] components)
		{
			Tag = tag;
			Components = new Component[components.Length];
			Array.Copy(components, Components, components.Length);
		}

	}
}
