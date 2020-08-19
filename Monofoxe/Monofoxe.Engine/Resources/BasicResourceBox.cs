using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Resources;
using System.IO;

namespace Monofoxe.Resources
{
	/// <summary>
	/// Loads all content from a specified directory.
	/// NOTE: All content files in the directory should be
	/// of the same type!!!
	/// </summary>
  public class BasicResourceBox<T> : ResourceBox<T>
	{
		private static ContentManager _content;

		private readonly string _resourcePath;

		public BasicResourceBox(string name, string resourcePath) : base(name)
		{
			_resourcePath = resourcePath;
		}

		public override void Load()
		{
			if (Loaded)
			{
				return;
			}
			Loaded = true;

			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = ResourceInfoMgr.ContentDir;

			var paths = ResourceInfoMgr.GetResourcePaths(_resourcePath);

			foreach(var path in paths)
			{
				AddResource(Path.GetFileNameWithoutExtension(path), _content.Load<T>(path));
			}
		}

		public override void Unload()
		{
			if (!Loaded)
			{
				return;
			}
			_content.Unload();
		}

	}
}
