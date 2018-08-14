using System;
using System.Collections.Generic;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;

namespace Monofoxe
{
	public class TestComponent : Component
	{
		
		public Vector2 Position;

		public TestComponent(Vector2 pos)
		{
			Tag = "test";
			Position = pos;
		}

	}
}
