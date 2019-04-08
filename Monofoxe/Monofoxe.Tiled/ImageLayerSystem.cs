using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	/// <summary>
	/// System for Tiled image layers. Just draws all images from components.
	/// </summary>
	public class ImageLayerSystem : BaseSystem
	{
		public override Type ComponentType => typeof(ImageLayerComponent);

		public override void Draw(Component component)
		{
			var image = (ImageLayerComponent)component;
			GraphicsMgr.DrawFrame(image.Frame, image.Offset, Vector2.Zero);
		}

	}
}
