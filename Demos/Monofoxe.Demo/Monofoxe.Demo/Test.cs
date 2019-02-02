using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Cameras;
using Resources.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Demo.GameLogic.Entities;
using System;
using Monofoxe.Tiled;

namespace Monofoxe.Demo
{
	public class Test : Entity
	{
		public static Camera Camera = new Camera(800, 600);

		Map _test;

		public Test() : base(SceneMgr.GetScene("default")["default"])
		{
			GameMgr.MaxGameSpeed = 60;
			
			Camera.BackgroundColor = new Color(64, 32, 32);

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
			_test = new Map(Resources.Maps.Test);
			_test.Load();
		}
		
		public override void Update()
		{
			if (Input.CheckButtonPress(Buttons.MouseRight))
			{
				var entity = EntityMgr.CreateEntityFromTemplate(Layer, "SolidBoi");
				entity.GetComponent<PositionComponent>().Position = Camera.GetRelativeMousePosition();
			}

			if (Input.CheckButtonPress(Buttons.Space))
			{
				var entity = EntityMgr.CreateEntityFromTemplate(Layer, "PhysicsBoi");
				entity.GetComponent<PositionComponent>().Position = Camera.GetRelativeMousePosition();
				Console.WriteLine("kok");
			}
		}

		
		public override void Draw()
		{
			//DrawMgr.DrawSprite(SpritesDefault.Monofoxe, 400, 300);
		}

	}
}