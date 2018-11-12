using System.Collections.Generic;

namespace Monofoxe.Engine
{
	public static class AssetMgr
	{
		/// <summary>
		/// Root directory of the game content.
		/// </summary>
		public static string ContentDir = "Content";
		
		/// <summary>
		/// Root directory for the graphics.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string GraphicsDir = "Graphics";
		
		/// <summary>
		/// Root directory for the audio.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string AudioDir = "Audio";
		
		/// <summary>
		/// Root directory for the shaders.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string EffectsDir = "Effects";

		/// <summary>
		/// Root directory for the fonts.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string FontsDir = "Fonts";

		/// <summary>
		/// Root directory for the entity templates.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string EntityTemplatesDir = "Entities";
		

		/// <summary>
		/// List of all game ssets.
		/// </summary>
		private static string[] _assetPaths;



		internal static void Init() =>
			_assetPaths = GameMgr.Game.Content.Load<string[]>(ContentDir); // Maybe make new content instance?
		
		
		
		/// <summary>
		/// Returns list of asset paths that match input path.
		/// </summary>
		public static string[] GetAssetPaths(string path = "")
		{
			path = path.Replace('\\', '/');	
			
			if (path != "")
			{
				var list = new List<string>();
				
				if (!path.EndsWith("/"))
				{
					path += '/';
				}
				foreach(string info in _assetPaths)
				{
					if (info.StartsWith(path))
					{
						list.Add(info);
					}
				}
				
				return list.ToArray();
			}
			else
			{
				var assetPathsCopy = new string[_assetPaths.Length];
				_assetPaths.CopyTo(assetPathsCopy, 0);
				return assetPathsCopy;
			}
		}
		
	}
}
