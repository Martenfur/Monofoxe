using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
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

		public override void Destroy()
		{
			throw new System.NotImplementedException();
		}

		public override void FixedUpdate()
		{
			throw new System.NotImplementedException();
		}

		public override void Update()
		{
			if (Move)
			{
				// Retrieving the position component from entity.
				var position = Owner.GetComponent<PositionComponent>();

				position.PreviousPosition = position.Position;
				position.Position += TimeKeeper.Global.Time(Speed) * Direction.ToVector2();
			}
		}

		public override void Draw()
		{
			var position = Owner.GetComponent<PositionComponent>();

			GraphicsMgr.CurrentColor = Color.White;

			Sprite.Draw(position.Position);
		}

	}
}
