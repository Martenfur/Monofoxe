using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Utils.Tilemaps
{
	/// <summary>
	/// Component for Tiled image layers.
	/// </summary>
	public class ImageLayerComponent : Component
	{
		public override string Tag => "imageLayer";

		public Frame Frame;

		public Vector2 Offset;

		public bool Visible = true;

		public ImageLayerComponent(Vector2 offset, Frame frame)
		{
			Frame = frame;
			Offset = offset;
		}

		public override object Clone() =>
			throw new NotImplementedException();

	}
}
