using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Reads atlass file.
	/// </summary>
	public class AtlassReader : ContentTypeReader<Dictionary<string, Frame[]>>
	{
		protected override Dictionary<string, Frame[]> Read(ContentReader input, Dictionary<string, Frame[]> existingInstance)
		{
			var texture = input.ReadObject<Texture2D>();

			var dictionary = new Dictionary<string, Frame[]>();

			var spritesCount = input.ReadInt32();

			for(var i = 0; i < spritesCount; i += 1)
			{
				var spriteName = input.ReadString();
				var framesCount = input.ReadInt32();

				Frame[] frames = new Frame[framesCount];
				
				Debug.WriteLine(framesCount);
				for(var k = 0; k < framesCount; k += 1)
				{
					var frameW = input.ReadInt32();
					var frameH = input.ReadInt32();
				
					var frameOrigin = new Vector2(input.ReadInt32(), input.ReadInt32());

					var frameTexturePos = new Rectangle(
						input.ReadInt32(), 
						input.ReadInt32(), 
						input.ReadInt32(), 
						input.ReadInt32()
					);
				
					frames[k] = new Frame(texture, frameTexturePos, frameOrigin, frameW, frameH);
				}
				dictionary.Add(spriteName, frames);

			}

			return dictionary;
		}
	}
}
