using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;

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
namespace Pipefoxe.SpriteGroup
{
	/// <summary>
	/// Atlas importer. Parses json, loads texture and generates 
	/// frame array, which will be passed to AtlasProcessor.
	/// All atlases should come in json-png pairs.
	/// Importer is oriented to TexturePacker JSON format. 
	/// </summary>
	[ContentImporter(".atlas", DefaultProcessor = "SpriteGroupProcessor", 
	DisplayName = "Sprite Group Importer - Monofoxe")]
	public class SpriteGroupImporter : ContentImporter<SpriteGroupData>
	{
		public override SpriteGroupData Import(string filename, ContentImporterContext context)
		{
			var groupData = new SpriteGroupData();
			
			string[] textureRegex;

			try
			{
				// Parsing config.
				var json = File.ReadAllText(filename);
				JToken configData = JObject.Parse(json);

				groupData.TextureSize = Int32.Parse(configData["textureSize"].ToString());
				groupData.TexturePadding = Int32.Parse(configData["texturePadding"].ToString());
				groupData.RootDir = Path.GetDirectoryName(filename) + '/' + configData["rootDir"].ToString();
				groupData.GroupName = configData["groupName"].ToString();
				groupData.ClassTemplatePath = configData["classTemplatePath"].ToString();
				groupData.ClassDir = configData["classDir"].ToString();

				JArray textureWildcards = (JArray)configData["singleTexturesWildcards"];

				textureRegex = new string[textureWildcards.Count];
				for(var i = 0; i < textureWildcards.Count; i += 1)
				{
					textureRegex[i] = WildCardToRegular(textureWildcards[i].ToString());
				}
				// Parsing config.
			}
			catch(Exception)
			{
				throw(new InvalidContentException("Incorrect JSON format!"));
			}
			
			ImportTextures(groupData.RootDir, "", groupData, textureRegex);

			var fileData = new StringBuilder();

			foreach(RawSprite spr in groupData.Sprites)
			{
				fileData.Append(spr.Name + Environment.NewLine);
			}

			File.WriteAllText(Environment.CurrentDirectory + "/log.log", fileData.ToString());

			return groupData;
			
		}

		private void ImportTextures(string dirPath, string dirName, SpriteGroupData groupData, string[] textureRegex)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

			foreach(FileInfo file in dirInfo.GetFiles("*.png"))
			{
				var spr = new RawSprite();
				spr.Name = dirName + Path.GetFileNameWithoutExtension(file.Name);
				spr.RawTexture = Image.FromFile(file.FullName);

				var configPath = Path.ChangeExtension(file.FullName, ".json");
				
				#region Reading config.
				if (File.Exists(configPath))
				{
					try
					{
						var conf = File.ReadAllText(configPath);
						JToken confData = JObject.Parse(conf); 			

						var v = Int32.Parse(confData["v"].ToString());
						var h = Int32.Parse(confData["h"].ToString());

						if (v > 0 && h > 0)
						{
							spr.FramesV = v;
							spr.FramesH = h;
						}

						spr.Offset = new Point(Int32.Parse(confData["offset_x"].ToString()), Int32.Parse(confData["offset_y"].ToString()));
					}
					catch(Exception)
					{
						throw(new Exception("Incorrect JSON format!"));
					}
				}
				#endregion Reading config.
				
				if (PathMatchesRegex('/' + dirName + '/' + file.Name, textureRegex))
				{
					groupData.Textures.Add(spr);
				}
				else
				{
					groupData.Sprites.Add(spr);
				}
			}


			// Recursively repeating for all subdirectories.
			foreach(DirectoryInfo dir in dirInfo.GetDirectories())
			{
				ImportTextures(dir.FullName, dirName + dir.Name + '/', groupData, textureRegex);
			}
			// Recursively repeating for all subdirectories.

		}

		private string WildCardToRegular(string value) =>
			"^" + Regex.Escape(value).Replace("\\*", ".*") + "$"; 
		
		private bool PathMatchesRegex(string path, string[] regexArray)
		{
			var safePath = path.Replace('\\', '/'); // Just to not mess with regex and wildcards.

			foreach(string regex in regexArray)
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
