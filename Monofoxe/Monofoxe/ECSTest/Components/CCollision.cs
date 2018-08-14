using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;

namespace Monofoxe.ECSTest.Components
{
	public class CCollision : Component
	{
		public float MaskR = 32;

		public CCollision()
		{
			Tag = "Collision";
		}
	}
}
