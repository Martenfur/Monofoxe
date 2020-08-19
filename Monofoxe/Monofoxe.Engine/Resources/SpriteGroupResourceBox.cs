using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;
using System.Collections.Generic;
using System.IO;

namespace Monofoxe.Resources
{
  public class SpriteGroupResourceBox : ResourceBox<Sprite>
	{
		
		private ContentManager _content = new ContentManager(GameMgr.Game.Services);

		
		private readonly string _resourcePath;

		public SpriteGroupResourceBox(string name, string spriteGroupPath) : base(name)
		{
			_resourcePath = spriteGroupPath;
		}

		public override void Load()
		{
			if (Loaded)
			{
				return;
			}
			Loaded = true;
			var graphicsPath = Path.Combine(ResourceInfoMgr.ContentDir, _resourcePath);
			var sprites = _content.Load<Dictionary<string, Sprite>>(graphicsPath);

			foreach (var spritePair in sprites)
			{
				AddResource(Path.GetFileNameWithoutExtension(spritePair.Key), spritePair.Value);
			}

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
