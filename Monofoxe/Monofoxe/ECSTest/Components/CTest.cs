using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Monofoxe.Engine.Converters;
using Monofoxe.Engine.Drawing;
using Resources.Sprites;
using Monofoxe.Engine;

namespace Monofoxe.ECSTest.Components
{
	public class CTest : Component
	{
		[JsonConverter(typeof(Vector2Converter))]
		public Vector2 Position;
		
		public override string Tag => "test";

		[JsonConverter(typeof(SpriteConverter))]
		public Sprite Spr = SpritesDefault.TestKitten;

		[JsonConverter(typeof(ColorConverter))]
		public Color Color = Color.Blue;

		[JsonConverter(typeof(StringEnumConverter))]
		public Buttons Button = Buttons.A;

		public override object Clone()
		{
			var component = new CTest();
			component.Position = Position;
			component.Spr = Spr;
			component.Color = Color;
			component.Button = Button;

			return component;
		}
	}
}
