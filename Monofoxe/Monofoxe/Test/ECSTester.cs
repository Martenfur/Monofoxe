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


namespace Monofoxe.Test
{
	public class ECSTester : Entity
	{
		
		public ECSTester(Layer layer) : base(layer)
		{
			
		}

		public override void Update()
		{
			if (Input.CheckButton(Buttons.MouseRight))
			{
				var ball = EntityMgr.CreateEntity(Layer.Get("balls"), "ball");
				ball.GetComponent<CMovement>().Position = Input.ScreenMousePos;
				ball.GetComponent<CCollision>().MaskR = 20;//r.Next(10, 16);
				Console.WriteLine(EntityMgr.Count("ball"));
			}
			if (Input.CheckButton(Buttons.MouseMiddle))
			{
				foreach(var entity in EntityMgr.GetList("ball"))
				{
					EntityMgr.DestroyEntity(entity);
				}
			}
			
			if (Input.CheckButtonPress(Buttons.G))
			{
				Layer.DepthSorting = !Layer.DepthSorting;
			}
		}
	}
}
