using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json.Linq;
using System.Drawing;

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
namespace Pipefoxe.Atlas
{
	/// <summary>
	/// Atlas importer. Parses json, loads texture and generates 
	/// frame array, which will be passed to AtlasProcessor.
	/// All atlases should come in json-png pairs.
	/// Importer is oriented to TexturePacker JSON format. 
	/// </summary>
	[ContentImporter(".atlas", DefaultProcessor = "AtlasProcessor", 
	DisplayName = "Atlas Importer - Monofoxe")]
	public class AtlasImporter : ContentImporter<SpriteGroupData>
	{
		public override SpriteGroupData Import(string filename, ContentImporterContext context)
		{
			var groupData = new SpriteGroupData();
			
			try
			{
				// Parsing config.
				var json = File.ReadAllText(filename);
				JToken configData = JObject.Parse(json);

				groupData.TextureSize = configData["textureSize"];
				groupData.TexturePadding = configData["texturePadding"];
				groupData.RootDir = configData["rootDir"];
				groupData.GroupName = configData["groupName"];
				groupData.ClassTemplatePath = configData["classTemplatePath"];

				// TODO: Make a wildcard or something.
				JToken token = configData["singleTextures"];
				// Parsing config.
			}
			catch(Exception)
			{
				throw(new InvalidContentException("Incorrect JSON format!"));
			}
			
			ImportTextures(groupData.RootDir, "", groupData);

			return groupData;
			
		}

		private void ImportTextures(string dirPath, string dirName, SpriteGroupData groupData)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

			foreach(FileInfo file in dirInfo.GetFiles("*.png"))
			{
				var spr = new RawSprite();
				spr.Name = dirName + '/' + Path.ChangeExtension(file.Name, "");
				spr.Texture = Image.FromFile(file.FullName);

				var configPath = Path.ChangeExtension(file.FullName, ".json");
				
				#region Reading config.
				if (File.Exists(configPath))
				{
					try
					{
						var conf = File.ReadAllText(configPath);
						JToken confData = JObject.Parse(conf)["frames"]; 			

						if (v >= 1 || h >= 1)
						{				
							spr.FramesV = Int32.Parse(confData["v"]);
							spr.FramesH = Int32.Parse(confData["h"]);
						}

						spr.Offset = Int32.Parse(confData["offset_x"], confData["offset_y"]);
					}
					catch(Exception)
					{
						return;
					}
				}
				#endregion Reading config.

				// TODO: Add sprite\texture filter here.
				groupData.Sprites.Add(spr);
			}


			// Recursively repeating for all subdirectories.
			foreach(DirectoryInfo dir in dirInfo.GetDirectories())
			{
				ImportTextures(dir.FullName, dirName + '/' + dir.Name + '/', outputDir);
			}
			// Recursively repeating for all subdirectories.

		}


	}
}
