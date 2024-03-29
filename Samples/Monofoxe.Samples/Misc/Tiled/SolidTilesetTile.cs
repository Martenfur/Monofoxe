﻿using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils.Tilemaps;

namespace Monofoxe.Samples.Misc.Tiled
{

	public class SolidTilesetTile : ITilesetTile
	{
		public Frame Frame {get; private set;}
		public bool Solid;
			
		public SolidTilesetTile(Frame frame, bool solid)
		{
			Frame = frame;
			Solid = solid;
		}
			
	}
}
