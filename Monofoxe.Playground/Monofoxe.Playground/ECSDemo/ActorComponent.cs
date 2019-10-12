using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Playground.ECSDemo
{
	/// <summary>
	/// Actor component. Actors can move in some direction.
	/// NOTE: PositionComponent is required.
	/// </summary>
	public class ActorComponent : Component
	{
		public bool Move = false;

		public Angle Direction;
		public float Speed = 120;

		public Sprite Sprite;
		
		public ActorComponent(Sprite sprite)
		{
			Visible = true; // Components are not visible by default.
			
			Sprite = sprite;
		}
	}
}
