using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Monofoxe.Engine.Converters;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class PositionComponent : Component
	{
		[JsonConverter(typeof(Vector2Converter))]
		public Vector2 Position;
		public Vector2 PreviousPosition;
		
		public PositionComponent(Vector2 position)
		{
			Position = position;
		}

		public override object Clone()
		{
			var positionComponent = new PositionComponent(Position);
			positionComponent.Position = Position;
			positionComponent.PreviousPosition = PreviousPosition;

			return positionComponent;
		}
	}
}
