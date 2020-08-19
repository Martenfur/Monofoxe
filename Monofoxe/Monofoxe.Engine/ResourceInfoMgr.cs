using System;
using System.Collections.Generic;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Provides convenient resource paths and lists all content assets.
	/// </summary>
	public static class ResourceInfoMgr
	{
		/// <summary>
		/// Root directory of the game content.
		/// </summary>
		public static string ContentDir = "Content";
		
		/// <summary>
		/// Root directory for graphics.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string GraphicsDir = "Graphics";
		
		/// <summary>
		/// Root directory for the audio.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string AudioDir = "Audio";
		
		/// <summary>
		/// Root directory for shaders.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string EffectsDir = "Effects";

		/// <summary>
		/// Root directory for fonts.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string FontsDir = "Fonts";

		/// <summary>
		/// Root directory for maps.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string MapsDir = "Maps";


		/// <summary>
		/// List of all game ssets.
		/// </summary>
		private static string[] _assetPaths;



		internal static void Init()
		{
			try
			{
				_assetPaths = GameMgr.Game.Content.Load<string[]>("Content");
			}
			catch(Exception) { }
		}
		
		
		
		/// <summary>
		/// Returns list of resource paths matching input path.
		/// Empty string will return all asset paths.
		/// </summary>
		public static string[] GetResourcePaths(string path = "")
		{
			if (_assetPaths == null)
			{ 
				return null;
			}
			path = path.Replace('\\', '/');	

			if (path != "")
			{
				var list = new List<string>();
				
				if (!path.EndsWith("/"))
				{
					path += '/';
				}
				foreach(var info in _assetPaths)
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
