using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.ECSTest.Components;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Utils;

namespace Monofoxe.ECSTest.Systems
{
	public class SCollision : BaseSystem
	{
		public override Type ComponentType => typeof(CCollision);

		public override int Priority => 0;

		public override void Create(Component component) {}

		public override void Destroy(Component component) {}

		public override void Update(List<Component> components) 
		{
		}

		
		public override void FixedUpdate(List<Component> components) 
		{
			
			
			foreach(CCollision collider in components)
			{
				
				var movement = collider.Owner.GetComponent<CMovement>();

				foreach(CCollision otherCollider in components)
				{
					var otherMovement = otherCollider.Owner.GetComponent<CMovement>();

					if (collider != otherCollider && GameMath.Distance(movement.Position, otherMovement.Position) < collider.MaskR + otherCollider.MaskR - 1)
					{
						var rSum = collider.MaskR + otherCollider.MaskR;
						var dist = GameMath.Distance(movement.Position, otherMovement.Position);

						if (dist == 0)
						{
							dist = 1;
						}
						
						var v = movement.Position - otherMovement.Position;
						v.Normalize();

						var resVect = v * (rSum - dist) / 2f;

						movement.Position += resVect;
						otherMovement.Position -= resVect;
					}
					
				}
			}
			
			foreach(CCollision collider in components)
			{
				var movement = collider.Owner.GetComponent<CMovement>();

				collider.Owner.Depth = -(int)movement.Position.Y;
			}
			//Console.WriteLine("I'm here!");
		}

		
		public override void Draw(Component component)
		{
			var collider = (CCollision)component;
			var movement = collider.Owner.GetComponent<CMovement>();

			GraphicsMgr.CurrentColor = new Color(255, 255, 255, 255);//Red * 0.5f;
			
			//DrawMgr.DrawCircle(movement[id].Position, collider.MaskR, true);
			movement.Spr.Draw(movement.Position, movement.Spr.Origin);
			
			GraphicsMgr.CurrentColor = Color.Red;
			RectangleShape.DrawBySize(movement.Position, Vector2.One * 32, true);
			CircleShape.Draw(movement.Position, 16, true);
		}
	}
}
