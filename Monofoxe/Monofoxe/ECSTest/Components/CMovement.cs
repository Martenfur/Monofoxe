using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Monofoxe.Engine.Converters;
using Monofoxe.Engine.Drawing;
using Resources.Sprites;

namespace Monofoxe.ECSTest.Components
{
	public class CMovement : Component
	{
		[JsonConverter(typeof(Vector2Converter))]
		public Vector2 Position;
		
		[JsonConverter(typeof(SpriteConverter))]
		public Sprite Spr = SpritesDefault.Bench;

		[JsonConverter(typeof(ColorConverter))]
		public Color PrettyBoi = Color.Blue;

		public override object Clone()
		{
			var component = new CMovement();
			component.Position = Position;
			component.Spr = Spr;
			component.PrettyBoi = PrettyBoi;

			return component;
		}
	}
}
