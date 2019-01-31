using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Monofoxe.Engine.Converters;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class SolidComponent : Component
	{
		[JsonConverter(typeof(Vector2Converter))]
		public Vector2 Size;
		
		
		public override object Clone()
		{
			var positionComponent = new SolidComponent();
			positionComponent.Size = Size;
			
			return positionComponent;
		}
	}
}
