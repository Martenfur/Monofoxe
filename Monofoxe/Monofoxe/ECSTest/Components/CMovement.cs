using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Resources.Sprites;

namespace Monofoxe.ECSTest.Components
{
	public class CMovement : Component
	{
		public Vector2 Position;
		
		public Sprite Spr = Default.Bench;

		public Color PrettyBoi = Color.Blue;

		public new bool Visible = true;
	}
}
