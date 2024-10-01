using Microsoft.Xna.Framework;
using Monofoxe.Engine.EC;


namespace Monofoxe.Engine.Utils
{
	public class PositionComponent : Component
	{
		[Inspectable]
		public Vector2 Value;
		[Inspectable]
		public float Depth;
		[Inspectable(false)]
		public Vector2 OldValue;
		[Inspectable(false)]
		public float OldDepth;

		public PositionComponent(Vector2 position, float depth = 0)
		{
			Value = position;
			Depth = depth;
		}

		public override void Update()
		{
			base.Update();
			OldValue = Value;
			OldDepth = Depth;
		}
	}
}
