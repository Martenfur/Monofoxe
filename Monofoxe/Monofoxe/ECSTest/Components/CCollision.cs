using Monofoxe.Engine.ECS;

namespace Monofoxe.ECSTest.Components
{
	public class CCollision : Component
	{
		public float MaskR = 32;

		public override object Clone()
		{
			var component = new CCollision();
			component.MaskR = MaskR;
			
			return component;
		}
	}
}
