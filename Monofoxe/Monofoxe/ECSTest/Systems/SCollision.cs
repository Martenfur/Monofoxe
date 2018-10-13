using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Components;
using Monofoxe.Utils;
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
			foreach(CCollision collider in components)
			{
				otherId = 0;
				foreach(CCollision otherCollider in components)
				{
					if (false)//id != otherId && GameMath.Distance(movement[id].Position, movement[otherId].Position) < collider.MaskR + otherCollider.MaskR - 1)
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
			
			id = 0;
			foreach(CCollision collider in components)
			{
				collider.Owner.Depth = -(int)movement[id].Position.Y;
				id += 1;
			}
		}

		
		public override void Draw(List<Component> components)
		{
			// Game crashes, if entities are deleted.
			var movement = ComponentMgr.GetComponentList<CMovement>(components);

			DrawMgr.BlendState = BlendState.AlphaBlend;//BlendState.NonPremultiplied;
			DrawMgr.CurrentColor = Color.Red * 0.5f;//new Color(255, 255, 255, 255);
			
			Resources.Effects.AlphaBlend.Parameters["World"].SetValue(Matrix.CreateTranslation(Vector3.Zero));
			
			Resources.Effects.AlphaBlend.Parameters["Projection"].SetValue(DrawMgr.CurrentProjection);
			Resources.Effects.AlphaBlend.Parameters["View"].SetValue(DrawMgr.CurrentTransformMatrix);
			//Resources.Effects.AlphaBlend.Parameters["AmbientColor"].SetValue(DrawMgr.CurrentColor);
			Resources.Effects.AlphaBlend.CurrentTechnique = Resources.Effects.AlphaBlend.Techniques["Textured"];
			DrawMgr.Effect = Resources.Effects.AlphaBlend;
			var id = 0;
			foreach(CCollision collider in components)
			{
				//DrawMgr.DrawCircle(movement[id].Position, collider.MaskR, true);
				DrawMgr.DrawSprite(Resources.Sprites.SpritesDefault.Flare, movement[id].Position);
				id += 1;
			}
			DrawMgr.Effect = null;
			DrawMgr.BlendState = BlendState.AlphaBlend;//BlendState.NonPremultiplied;
			
		}
	}
}
