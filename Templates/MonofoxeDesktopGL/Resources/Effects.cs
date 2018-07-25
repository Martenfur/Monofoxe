using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using Monofoxe.Engine.Drawing;

namespace Resources
{
	public static class Effects
	{
		private static ContentManager _content;
		
		public static Effect Effect;
		
		public static void Load(ContentManager content)
		{
			_content = new ContentManager(content.ServiceProvider);
			_content.RootDirectory = content.RootDirectory;
			
			Effect = _content.Load<Effect>("Effects/effect");
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}