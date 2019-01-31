using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Converters;
using Newtonsoft.Json;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class PhysicsComponent : Component
	{
		[JsonConverter(typeof(Vector2Converter))]
		public Vector2 Size;
		[JsonConverter(typeof(Vector2Converter))]
		public Vector2 Speed;
		
		public Color Color = Color.Black;


		public override object Clone()
		{
			var physicsObjectComponent = new PhysicsComponent();
			physicsObjectComponent.Size = Size;
			physicsObjectComponent.Speed = Speed;

			return physicsObjectComponent;
		}
	}
}
