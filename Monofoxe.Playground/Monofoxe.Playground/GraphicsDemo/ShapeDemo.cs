using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.SceneSystem;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class ShapeDemo : Entity
	{
		public ShapeDemo(Layer layer) : base(layer)
		{
			
		}

		public override void Draw()
		{	
			CircleShape.Draw(new Vector2(100, 100), 100, true);
		}

	}
}
