using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Resources.Sprites;
using Resources;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Systems;
using Monofoxe.ECSTest.Components;
using Monofoxe.Engine.SceneSystem;


namespace Monofoxe.Test
{
	public class ECSTester : Entity
	{
		
		public ECSTester(Layer layer) : base(layer)
		{
			
		}

		public override void Update()
		{
			Console.WriteLine(Layer.Count("ball"));
		
			if (Input.CheckButton(Buttons.MouseLeft))
			{
				var ball = EntityMgr.CreateEntityFromTemplate(Layer, "ball");
				ball.GetComponent<CMovement>().Position = Input.ScreenMousePos;
				ball.GetComponent<CCollision>().MaskR = 20;//r.Next(10, 16);
			}
			
			if (Input.CheckButton(Buttons.MouseRight))
			{
				foreach(var entity in Layer.GetList("ball"))
				{
					EntityMgr.DestroyEntity(entity);
				}
				var ball = EntityMgr.CreateEntityFromTemplate(Layer, "ball");
				ball.GetComponent<CMovement>().Position = Input.ScreenMousePos;
				ball.GetComponent<CCollision>().MaskR = 20;//r.Next(10, 16);
			}
			
			if (Input.CheckButtonPress(Buttons.G))
			{
				Layer.DepthSorting = !Layer.DepthSorting;
			}
			if (Input.CheckButtonPress(Buttons.H))
			{
				Layer.IsGUI = !Layer.IsGUI;
			}
		}
	}
}
