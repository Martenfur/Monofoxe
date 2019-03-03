using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	/// <summary>
	/// Component for Tiled image layers.
	/// </summary>
	public class ImageLayerComponent : Component
	{
		public Frame Frame;

		public Vector2 Offset;

		public ImageLayerComponent(Vector2 offset, Frame frame)
		{
			Visible = true;
			Frame = frame;
			Offset = offset;
		}

		public override object Clone() =>
			throw new NotImplementedException();

	}
}
