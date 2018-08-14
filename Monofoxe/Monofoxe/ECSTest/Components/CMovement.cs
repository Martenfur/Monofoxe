using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;

namespace Monofoxe.ECSTest.Components
{
	public class CMovement : Component
	{
		public Vector2 Position;

		public CMovement()
		{
			Tag = "Movement";
		}
	}
}
