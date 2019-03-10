using Monofoxe.Engine.ECS;

namespace Monofoxe.ECSTest.Components
{
	public class CCollision : Component
	{
		public float MaskR = 32;

		//public new bool Visible = true;

		public CCollision()
		{
			Visible = true;
		}

		public override object Clone()
		{
			var component = new CCollision();
			component.MaskR = MaskR;
			
			return component;
		}
	}
}
