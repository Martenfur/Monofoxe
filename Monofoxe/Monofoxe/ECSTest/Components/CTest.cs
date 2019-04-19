using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Resources.Sprites;
using Monofoxe.Engine;

namespace Monofoxe.ECSTest.Components
{
	public class CTest : Component
	{
		public Vector2 Position;
		
		public Sprite Spr = Default.TestKitten;

		public Color Color = Color.Blue;

		public Buttons Button = Buttons.A;
	}
}
