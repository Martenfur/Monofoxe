using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Monofoxe.Engine.Converters;
using Monofoxe.Engine.Drawing;
using Resources.Sprites;

namespace Monofoxe.ECSTest.Components
{
	public class CBirb : Component
	{
		public override string Tag => "birb";

		[JsonConverter(typeof(Vector2Converter))]
		public Vector2 Position;
		
		[JsonConverter(typeof(SpriteConverter))]
		public Sprite Spr = SpritesDefault.Bro;
		
		public override object Clone()
		{
			var component = new CBirb();
			component.Position = Position;
			component.Spr = Spr;
			
			return component;
		}
	}
}
