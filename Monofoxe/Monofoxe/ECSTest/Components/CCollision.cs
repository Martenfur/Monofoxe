using Monofoxe.Engine.ECS;

namespace Monofoxe.ECSTest.Components
{
	public class CCollision : Component
	{
		public float MaskR = 32;

		public CCollision()
		{
			Visible = true;
		}
	}
}
