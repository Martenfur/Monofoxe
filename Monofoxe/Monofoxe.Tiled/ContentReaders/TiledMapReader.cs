using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Monofoxe.Tiled.MapStructure;
using System;

namespace Monofoxe.Tiled.ContentReaders
{
	/// <summary>
	/// Reads sprite group file.
	/// </summary>
	public class TiledMapReader : ContentTypeReader<TiledMap>
	{
		protected override TiledMap Read(ContentReader input, TiledMap existingInstance)
		{
			//var map = input.ReadObject<TiledMap>();
			Console.WriteLine(input.ReadObject<Color?>());
			Console.WriteLine(input.ReadInt32());
			Console.WriteLine(input.ReadInt32());

			return null;
		}
	}
}
