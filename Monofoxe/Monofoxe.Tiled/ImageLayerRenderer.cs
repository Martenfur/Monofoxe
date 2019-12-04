using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	/// <summary>
	/// Component for Tiled image layers.
	/// </summary>
	public class ImageLayerRenderer : Entity
	{
		public Frame Frame;

		public Vector2 Offset;

		public ImageLayerRenderer(Layer layer, Vector2 offset, Frame frame) : base(layer)
		{
			Visible = true;
			Frame = frame;
			Offset = offset;
		}

		public override void Draw()
		{
			Frame.Draw(Offset, Vector2.Zero);
		}

	}
}
