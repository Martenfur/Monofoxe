using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;


namespace Monofoxe.Demo.GameLogic.Entities
{
	public class PhysicsSystem : BaseSystem
	{
		public override Type ComponentType => typeof(PhysicsComponent);

		private List<Entity> _solidObjects;
		
		public override int Priority => 2;

		public float Gravity = 400;
		public float MaxFallSpeed = 500; // px/sec


		public override void Update(List<Component> components)
		{
			_solidObjects = SceneMgr.CurrentLayer.GetEntityListByComponent<SolidComponent>();

			foreach(PhysicsComponent cPhysics in components)
			{
				var entity = cPhysics.Owner;
				var cPosition = entity.GetComponent<PositionComponent>();
				
				cPhysics.Color = Color.Black;

				if (Input.CheckButton(Buttons.MouseLeft))
				{
					cPhysics.Speed = Input.MousePos - cPosition.Position;
					cPhysics.Speed.Normalize();
					cPhysics.Speed *= 150;
				}

				// Gravity.
				if (cPhysics.Speed.Y < MaxFallSpeed && CheckCollision(entity, cPosition.Position + Vector2.UnitY, cPhysics.Size) == null)
				{
					cPhysics.Speed.Y += TimeKeeper.GlobalTime(Gravity);
					if (cPhysics.Speed.Y > MaxFallSpeed)
					{
						cPhysics.Speed.Y = MaxFallSpeed;
					}
				}
				// Gravity.
				
				
				cPosition.Position.X += TimeKeeper.GlobalTime(cPhysics.Speed.X);
				
				var collider = CheckCollision(entity, cPosition.Position, cPhysics.Size);
				if (collider != null)
				{
					//cPhysics.Color = Color.Red;
					var sign = 1;
					if (cPhysics.Speed.X < 0)
					{
						sign = -1;
					}

					var colliderPos = collider.GetComponent<PositionComponent>();
					var colliderSolid = collider.GetComponent<SolidComponent>();

					var dPos = cPosition.Position.X - colliderPos.Position.X;
					var ppos = cPosition.Position - Vector2.UnitX * (dPos - Math.Sign(dPos) * (colliderSolid.Size.X + cPhysics.Size.X) / 2f);
					
					if (CheckCollision(entity, ppos, cPhysics.Size) != null)
					{
						for(var x = 0; x <= TimeKeeper.GlobalTime(Math.Abs(cPhysics.Speed.X)*2) + 1; x += 1)
						{
							if (CheckCollision(entity, cPosition.Position - Vector2.UnitX * x * sign, cPhysics.Size) == null)
							{
								cPhysics.Speed.X = 0;
								cPosition.Position.X -= x * sign;
								break;
							}
						}
					}
					else
					{
						cPhysics.Speed.X = 0;
						cPosition.Position.X = ppos.X;
					}
				}

				cPosition.Position.Y += TimeKeeper.GlobalTime(cPhysics.Speed.Y);
				if (CheckCollision(entity, cPosition.Position, cPhysics.Size) != null)
				{
					//cPhysics.Color = Color.Red;
					var sign = 1;
					if (cPhysics.Speed.Y < 0)
					{
						sign = -1;
					}

					for(var y = 0; y <= TimeKeeper.GlobalTime(Math.Abs(cPhysics.Speed.Y)*2) + 1; y += 1)
					{
						if (CheckCollision(entity, cPosition.Position - Vector2.UnitY * y * sign, cPhysics.Size) == null)
						{
							cPhysics.Speed.Y = 0;
							cPosition.Position.Y -= y * sign;
							break;
						}
					}
				}
			}
		}

		public override void Draw(Component component)
		{

		}


		Entity CheckCollision(Entity checker, Vector2 position, Vector2 size)
		{
			foreach(var solid in _solidObjects)
			{
				if (solid != checker)
				{
					var solidPos = solid.GetComponent<PositionComponent>().Position;
					var solidSize = solid.GetComponent<SolidComponent>().Size / 2f;

					if (GameMath.RectangleInRectangle(position - size / 2f, position + size / 2f, solidPos - solidSize, solidPos + solidSize))
					{
						return solid;
					}
				}
			}
			return null;
		}

	}
}
