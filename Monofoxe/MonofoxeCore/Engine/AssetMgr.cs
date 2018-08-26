using Microsoft.Xna.Framework;
using System;
using Monofoxe.Engine.Audio;
using MonoGame.AssetInfo;
using System.Collections.Generic;
using System;
using System.IO;

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
		public static IReadOnlyList<AssetInfo> AssetInfo;

		internal static void Init() =>
			AssetInfo = GameMgr.Game.Content.LoadMGCB(ContentDir);


		public static List<string> GetAssetPaths(string path = "")
		{
			var list = new List<string>();
			path = path.Replace('\\', '/');	
			
			if (path != "")
			{
				if (!path.EndsWith("/"))
				{
					path += '/';
				}
				foreach(AssetInfo info in AssetInfo)
				{
					if (info.OutputPath.StartsWith(path))
					{
						list.Add(info.OutputPath);
					}
				}
			}
			else
			{
				foreach(AssetInfo info in AssetInfo)
				{
					list.Add(info.OutputPath);
				}
			}

			return list;
		}
		
	}
}
