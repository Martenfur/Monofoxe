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
		
		public ECSTester(Layer layer) : base(SceneMgr.GetScene("default")["balls"])
		{
		}

		public override void Update()
		{
			SceneMgr.GetScene("default")["balls"].Priority = 999999;
	
			if (Input.CheckButton(Buttons.MouseLeft))
			{
				if (Input.CheckButton(Buttons.Space))
				{
					//for(var i = 0; i < 10; i += 1)
					{
						var ball = EntityMgr.CreateEntityFromTemplate(Layer, "birb");
						ball.GetComponent<CBirb>().Position = Input.ScreenMousePos;
					}
				}
				else
				{
					//for(var i = 0; i < 10; i += 1)
					{
						var ball = EntityMgr.CreateEntityFromTemplate(Layer, "ball");
						ball.GetComponent<CMovement>().Position = Input.ScreenMousePos;
						ball.GetComponent<CCollision>().MaskR = 20;//r.Next(10, 16);
					}
				}
				Console.WriteLine(Layer.CountEntities("ball") + Layer.CountEntities("birb"));
		
			}
			
			if (Input.CheckButton(Buttons.MouseRight))
			{
				foreach(var entity in Layer.GetEntityList("ball"))
				{
					EntityMgr.DestroyEntity(entity);
				}
				var ball = EntityMgr.CreateEntityFromTemplate(Layer, "ball");
				ball.GetComponent<CMovement>().Position = Input.ScreenMousePos;
				ball.GetComponent<CCollision>().MaskR = 20;//r.Next(10, 16);
			}
			
			if (Input.CheckButtonPress(Buttons.N))
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
