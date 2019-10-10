using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;
using Resources.Sprites;

namespace Resources
{
	public class Fonts : ResourceBox<IFont>
	{
		private ContentManager _content;

		static readonly string Ascii = " !" + '"' + @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

		public override bool Loaded { get; protected set; }

		public override string Name => "Fonts";

		public Fonts()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.FontsDir;
		}

		public override void Load()
		{
			AddResource("Arial", new Font(_content.Load<SpriteFont>("Arial")));
			AddResource("FancyFont", new TextureFont(Default.Font, 1, 1, Ascii, false));
		}

		public override void Unload()
		{
			_content.Unload();
		}
	}
}
