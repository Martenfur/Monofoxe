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
		
		public override string Tag => "movement";

		[JsonConverter(typeof(SpriteConverter))]
		public Sprite Spr = SpritesDefault.Bench;

		[JsonConverter(typeof(ColorConverter))]
		public Color PrettyBoi = Color.Blue;
	}
}
