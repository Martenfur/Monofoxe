using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Resources;
using System;
using System.Collections.Generic;

namespace $rootnamespace$
{
	public class $safeitemname$ : ResourceBox<string> // Replace with your resource type.
	{
		private static ContentManager _content;

		public override string Name => "Effects";

		public override void Load()
		{
			if (Loaded)
			{
				return;
			}
			Loaded = true;
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.EffectsDir;

			// Add your resources here. 
			// Example:
			// AddResource("Test", _content.Load<YourResourceType>("test"));
			// You can access the resources via ResourceHub.GetResource<>();
			
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