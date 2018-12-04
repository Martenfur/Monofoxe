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
		public override string Tag => "physicsObject";

		private List<SolidObjectComponent> _solidObjects;

		public override void Update(List<Component> components)
		{
			_solidObjects = ComponentMgr.GetAllComponents(components[0].Tag, SceneMgr.CurrentLayer).Select(x => (SolidObjectComponent)x).ToList();
			//ComponentMgr.GetComponentList<SolidObjectComponent>();

			foreach(PhysicsObjectComponent physicsObject in components)
			{
				var positionComponent = physicsObject.Owner.GetComponent<PositionComponent>();
				positionComponent.Position = Input.MousePos;//physicsObject.Speed;




			}
		}

		public override void Draw(Component component)
		{
			var physicsObject = (PhysicsObjectComponent)component;
			var position = physicsObject.Owner.GetComponent<PositionComponent>();

			DrawMgr.DrawRectangle(
				position.Position - physicsObject.Size / 2,
				position.Position + physicsObject.Size / 2,
				true
			);

			DrawMgr.DrawCircle(
				position.PreviousPosition,
				8,
				true
			);

		}

		bool CheckCollision(PhysicsObjectComponent physicsObject, Vector2 offset)
		{
			foreach(var solidObject in _solidObjects)
			{
				
			}
		}

	}
}
