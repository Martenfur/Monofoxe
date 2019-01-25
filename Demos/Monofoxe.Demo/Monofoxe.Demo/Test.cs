using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Cameras;
using Resources.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Demo.GameLogic.Entities;
using System;

namespace Monofoxe.Demo
{
	class Test : Entity
	{
		Camera cam = new Camera(800, 600);

		public Test() : base(SceneMgr.GetScene("default")["default"])
		{
			GameMgr.MaxGameSpeed = 60;
			
			cam.BackgroundColor = new Color(64, 32, 32);

			GameMgr.WindowManager.CanvasSize = new Vector2(800, 600);
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			GameMgr.WindowManager.CanvasMode = CanvasMode.Fill;
			
			DrawMgr.Sampler = SamplerState.PointClamp;
			/*
			var entity = new Entity(Layer, "physicsBoi");
			entity.AddComponent(new PositionComponent(Vector2.Zero));
			var phy = new PhysicsObjectComponent();
			phy.Size = Vector2.One * 32;
			entity.AddComponent(phy);
			*/
		}
		
		public override void Update()
		{
			if (Input.CheckButtonPress(Buttons.MouseRight))
			{
				var entity = EntityMgr.CreateEntityFromTemplate(Layer, "SolidBoi");
				entity.GetComponent<PositionComponent>().Position = Input.MousePos;
			}

			if (Input.CheckButtonPress(Buttons.Space))
			{
				var entity = EntityMgr.CreateEntityFromTemplate(Layer, "PhysicsBoi");
				entity.GetComponent<PositionComponent>().Position = Input.MousePos;
				Console.WriteLine("kok");
			}
		}

		
		public override void Draw()
		{
			DrawMgr.DrawSprite(SpritesDefault.Monofoxe, 400, 300);
		}

	}
}