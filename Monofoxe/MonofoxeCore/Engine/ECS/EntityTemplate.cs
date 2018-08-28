using System;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Entity template. Used to store info about entities.
	/// </summary>
	public class EntityTemplate
	{
		public readonly string Tag;

		public readonly Component[] Components;

		public EntityTemplate(string tag, Component[] components)
		{
			Tag = tag;
			Components = new Component[components.Length];
			Array.Copy(components, Components, components.Length);
		}

	}
}
