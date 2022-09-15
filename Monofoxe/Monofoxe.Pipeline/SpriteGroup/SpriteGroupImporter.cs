using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Linq;
using System.Text.Json.Nodes;
using StbImageSharp;

/*
 * FUTURE NOTE:
 * To create Pipeline Extension project,
 * choose C# Class Library template,
 * then reference Monogame for Desktop GL
 * and get Monogame.Framework.Content.Pipeline
 * from NuGet.
 * 
 * To add library to pipeline project, reference
 * dll with project name.
 */
namespace Monofoxe.Pipeline.SpriteGroup
{

	/// <summary>
	/// Sprite group importer. Parses json config, and loads textures,
	/// which will be passed to AtlasProcessor.
	/// </summary>
	[ContentImporter(".spritegroup", DefaultProcessor = "SpriteGroupProcessor",
	DisplayName = "Sprite Group Importer - Monofoxe")]
	public class SpriteGroupImporter : ContentImporter<SpriteGroupData>
	{

		public override SpriteGroupData Import(string filename, ContentImporterContext context)
		{
			var groupData = new SpriteGroupData();

			string[] textureRegex;

			#region Parsing config.	

			try
			{
				var json = File.ReadAllText(filename);
				JsonNode configData = JsonNode.Parse(json);

				groupData.AtlasSize = int.Parse(configData["atlasSize"].ToString());
				groupData.TexturePadding = int.Parse(configData["texturePadding"].ToString());
				groupData.RootDir = Path.GetDirectoryName(filename) + '/' + configData["rootDir"].ToString();
				groupData.GroupName = Path.GetFileNameWithoutExtension(filename);

				var textureWildcards = (JsonArray)configData["singleTexturesWildcards"];

				textureRegex = new string[textureWildcards.Count];
				for (var i = 0; i < textureWildcards.Count; i += 1)
				{
					textureRegex[i] = WildCardToRegular(textureWildcards[i].ToString());
				}
			}
			catch (Exception)
			{
				throw new InvalidContentException("Incorrect JSON format!");
			}

			#endregion Parsing config.


			ImportTextures(groupData.RootDir, "", groupData, textureRegex);

			return groupData;

		}



		/// <summary>
		/// Recursively looks into root dir and loads textures. 
		/// </summary>
		/// <param name="dirPath">Full path to directory.</param>
		/// <param name="dirName">Full path minus root.</param>
		/// <param name="groupData">SpriteGroupData object.</param>
		/// <param name="textureRegex">Regex filter. Determines if texture is part of atlas or single.</param>
		private void ImportTextures(string dirPath, string dirName, SpriteGroupData groupData, string[] textureRegex)
		{
			var dirInfo = new DirectoryInfo(dirPath);

			foreach (var file in dirInfo.GetFiles("*.png"))
			{
				var spr = new RawSprite();
				spr.Name = dirName + Path.GetFileNameWithoutExtension(file.Name);

				spr.RawTexture = new Bmp(file.FullName);

				var configPath = Path.ChangeExtension(file.FullName, ".json");


				#region Reading config.
				/*
				 * Just reading sprite jsons.
				 * If you want to add more parameters, begin from here.
				 */
				if (File.Exists(configPath))
				{
					try
					{
						var conf = File.ReadAllText(configPath);
						JsonNode confData = JsonNode.Parse(conf);

						spr.FramesH = int.Parse(confData["h"].ToString());
						spr.FramesV = int.Parse(confData["v"].ToString());

						if (spr.FramesH < 1 || spr.FramesV < 1) // Frame amount cannot be lesser than 1.
						{
							throw new Exception();
						}

						
						var originXKeyword = "originX";
						var originYKeyword = "originY";

						if (confData[originXKeyword] == null)
						{ 
							originXKeyword = "offset_x";
						}
						if (confData[originYKeyword] == null)
						{ 
							originYKeyword = "offset_y";
						}

						var xOffsetRaw = confData[originXKeyword].ToString().ToLower();
						var yOffsetRaw = confData[originYKeyword].ToString().ToLower();

						var center = new Point(spr.RawTexture.Width / spr.FramesH / 2, spr.RawTexture.Height / spr.FramesV / 2);

						spr.Offset = SpriteGroupMathParser.Parse(
							xOffsetRaw,
							yOffsetRaw,
							0,
							spr.RawTexture.Width / spr.FramesH,
							0,
							spr.RawTexture.Height / spr.FramesV,
							center
						);

					}
					catch (Exception e)
					{
						throw new Exception("Error while pasring sprite JSON for file: " + file.Name + " | " + e.Message + " | " + e.StackTrace);
					}
				}
				#endregion Reading config.


				if (PathMatchesRegex('/' + dirName + '/' + file.Name, textureRegex)) // Separating atlas sprites from single textures.
				{
					groupData.SingleTextures.Add(spr);
				}
				else
				{
					groupData.Sprites.Add(spr);
				}
			}


			// Recursively repeating for all subdirectories.
			foreach (var dir in dirInfo.GetDirectories())
			{
				ImportTextures(dir.FullName, dirName + dir.Name + '/', groupData, textureRegex);
			}
			// Recursively repeating for all subdirectories.

		}



		private string WildCardToRegular(string value) =>
			"^" + Regex.Escape(value).Replace("\\*", ".*") + "$";



		/// <summary>
		/// Checks if path matches regex filter.
		/// </summary>
		private bool PathMatchesRegex(string path, string[] regexArray)
		{
			var safePath = path.Replace('\\', '/'); // Just to not mess with regex and wildcards.

			foreach (var regex in regexArray)
			{
				if (Regex.IsMatch(safePath, regex, RegexOptions.IgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

	}
}
