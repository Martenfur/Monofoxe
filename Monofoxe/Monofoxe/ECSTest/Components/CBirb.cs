using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Resources.Sprites;

namespace Monofoxe.ECSTest.Components
{
	public class CBirb : Component
	{
		public Vector2 Position;
		
		public Sprite Spr = Default.Bro;
	}
}
