using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;
using MonoGame.Extended.Tiled;
using System;

namespace Resources
{
	public static class Maps
	{
		private static ContentManager _content;
		
		public static TiledMap Test;
		
		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + "Maps";
			
			Test = _content.Load<TiledMap>("test");
			Console.WriteLine(Test.Layers[0].Name);
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
