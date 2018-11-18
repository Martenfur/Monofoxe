using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Components;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.ECSTest.Systems
{
	public class SCollision : BaseSystem
	{
		public override string Tag => "collision";

		public override void Create(Component component) {}

		public override void Destroy(Component component) {}

		public override void Update(List<Component> components) {}

		
		public override void FixedUpdate(List<Component> components) 
		{
			var movement = ComponentMgr.GetComponentList<CMovement>(components);
			
			var id = 0;
			var otherId = 0;
			/*
			foreach(CCollision collider in components)
			{
				otherId = 0;
				foreach(CCollision otherCollider in components)
				{
					if (id != otherId && GameMath.Distance(movement[id].Position, movement[otherId].Position) < collider.MaskR + otherCollider.MaskR - 1)
					{
						var rSum = collider.MaskR + otherCollider.MaskR;
						var dist = GameMath.Distance(movement[id].Position, movement[otherId].Position);

						var v = movement[id].Position - movement[otherId].Position;
						v.Normalize();

						var resVect = v * (rSum - dist) / 2f;

						movement[id].Position += resVect;
						movement[otherId].Position -= resVect;
					}

					otherId += 1;
				}
				id += 1;
			}
			*/
			id = 0;
			foreach(CCollision collider in components)
			{
				collider.Owner.Depth = -(int)movement[id].Position.Y;
				id += 1;
			}
		}

		
		public override void Draw(List<Component> components)
		{
			// Game crashes, if entities are deleted. TODO: Investigate.
			var movement = ComponentMgr.GetComponentList<CMovement>(components);

			//DrawMgr.BlendState = BlendState.AlphaBlend;
			DrawMgr.CurrentColor = new Color(255, 0, 255, 255);//Red * 0.5f;
			
			//DrawMgr.Effect = Resources.Effects.Effect;
			var id = 0;
			foreach(CCollision collider in components)
			{
				//DrawMgr.DrawCircle(movement[id].Position, collider.MaskR, true);
				DrawMgr.DrawSprite(Resources.Sprites.SpritesDefault.Barrel, movement[id].Position);
				id += 1;
			}
			DrawMgr.CurrentEffect = null;
			Console.WriteLine("Barrels: " + id);
		}
	}
}
