﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;

namespace Monofoxe.Samples.Resources
{
	public class Fonts : ResourceBox<IFont>
	{
		private ContentManager _content;

		public Fonts() : base("Fonts")
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = ResourceInfoMgr.ContentDir + "/Fonts";
		}

		public override void Load()
		{
			if (Loaded)
			{
				return;
			}
			Loaded = true;

			AddResource("Arial", new Font(_content.Load<SpriteFont>("Arial")));

			var fontSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Font");

			AddResource("FancyFont", new TextureFont(fontSprite, 1, 1, TextureFont.Ascii, false));
		}

		public override void Unload()
		{
			if (!Loaded)
			{
				return;
			}
			Loaded = false;
			_content.Unload();
		}
	}
}
