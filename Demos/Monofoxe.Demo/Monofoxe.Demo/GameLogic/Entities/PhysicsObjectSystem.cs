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
	public class PhysicsObjectSystem : BaseSystem
	{
		public override Type ComponentType => typeof(PhysicsObjectComponent);

		private List<Entity> _solidObjects;
		
		public float Gravity = 400;
		public float MaxFallSpeed = 500; // px/sec


		public override void Update(List<Component> components)
		{
			//_solidObjects = ComponentMgr.GetAllComponents(components[0].Tag, SceneMgr.CurrentLayer).Select(x => (SolidObjectComponent)x).ToList();
			//ComponentMgr.GetComponentList<SolidObjectComponent>();



			_solidObjects = SceneMgr.CurrentScene.GetEntityListByComponent<SolidObjectComponent>();


			foreach(PhysicsObjectComponent cPhysics in components)
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
				if (cPhysics.Speed.Y < MaxFallSpeed && CheckCollision(cPosition.Position + Vector2.UnitY, cPhysics.Size) == null)
				{
					cPhysics.Speed.Y += TimeKeeper.GlobalTime(Gravity);
					if (cPhysics.Speed.Y > MaxFallSpeed)
					{
						cPhysics.Speed.Y = MaxFallSpeed;
					}
				}
				// Gravity.

				
				cPosition.Position.X += TimeKeeper.GlobalTime(cPhysics.Speed.X);
				
				if (CheckCollision(cPosition.Position, cPhysics.Size) != null)
				{
					//cPhysics.Color = Color.Red;
					var sign = 1;
					if (cPhysics.Speed.X < 0)
					{
						sign = -1;
					}

					for(var x = 0; x <= TimeKeeper.GlobalTime(Math.Abs(cPhysics.Speed.X)) + 1; x += 1)
					{
						if (CheckCollision(cPosition.Position - Vector2.UnitX * x * sign, cPhysics.Size) == null)
						{
							cPhysics.Speed.X = 0;
							cPosition.Position.X -= x * sign;
							break;
						}
					}
				}

				cPosition.Position.Y += TimeKeeper.GlobalTime(cPhysics.Speed.Y);
				if (CheckCollision(cPosition.Position, cPhysics.Size) != null)
				{
					//cPhysics.Color = Color.Red;
					var sign = 1;
					if (cPhysics.Speed.Y < 0)
					{
						sign = -1;
					}

					for(var y = 0; y <= TimeKeeper.GlobalTime(Math.Abs(cPhysics.Speed.Y)) + 1; y += 1)
					{
						if (CheckCollision(cPosition.Position - Vector2.UnitY * y * sign, cPhysics.Size) == null)
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
			
			var physicsObject = (PhysicsObjectComponent)component;
			var position = physicsObject.Owner.GetComponent<PositionComponent>();

			DrawMgr.CurrentColor = physicsObject.Color;

			DrawMgr.DrawRectangle(
				position.Position.ToPoint().ToVector2() - physicsObject.Size / 2,
				position.Position.ToPoint().ToVector2() + physicsObject.Size / 2,
				true
			);

			DrawMgr.DrawCircle(
				position.PreviousPosition.ToPoint().ToVector2(),
				8,
				true
			);
			DrawMgr.CurrentColor = Color.Black;
		}


		Entity CheckCollision(Vector2 position, Vector2 size)
		{
			foreach(var solid in _solidObjects)
			{
				var solidPos = solid.GetComponent<PositionComponent>().Position;
				var solidSize = solid.GetComponent<SolidObjectComponent>().Size / 2f;

				if (GameMath.RectangleInRectangle(position - size / 2f, position + size / 2f, solidPos - solidSize, solidPos + solidSize))
				{
					return solid;
				}
			}
			return null;
		}

	}
}
