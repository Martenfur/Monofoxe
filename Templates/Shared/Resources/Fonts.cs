using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;

namespace Resources
{
	public class Fonts : ResourceBox<IFont>
	{
		private ContentManager _content;

		static readonly string Ascii = " !" + '"' + @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		
		public override string Name => "Fonts";

		public Fonts()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.FontsDir;
		}

		public override void Load()
		{
			if (Loaded)
			{
				return;
			}
			Loaded = true;

			// Add your resources here. 
			// Example:
			// AddResource("Test", _content.Load<SpriteFont>("test"));
			// You can access the resources via ResourceHub.GetResource<>();
			AddResource("Arial", new Font(_content.Load<SpriteFont>("Arial")));
			
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
