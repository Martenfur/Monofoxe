using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Xna.Framework;


namespace PipelineExt
{
	[ContentImporter(".json", DefaultProcessor = "AtlassProcessor", 
	DisplayName = "Atlass Importer")]
	public class AtlassImporter : ContentImporter<AtlassFrames>
	{
		public override AtlassFrames Import(string filename, ContentImporterContext context)
		{
			AtlassFrames atlassFrames = new AtlassFrames();
			
			var textureImporter = new TextureImporter();
			TextureContent texture = textureImporter.Import(Path.ChangeExtension(filename, ".png"), context);

			atlassFrames.Texture = texture;

			// Parsing config.
			string json = File.ReadAllText(filename);

			JToken framesData = JObject.Parse(json)["frames"];
			// Parsing config.

			foreach(JProperty token in framesData)
			{
				JToken frameData = token.Value;
			
				Point size = new Point(
					Int32.Parse(frameData["sourceSize"]["w"].ToString()), 
					Int32.Parse(frameData["sourceSize"]["h"].ToString())
				);
				
				Point origin = new Point(
						Int32.Parse(frameData["spriteSourceSize"]["x"].ToString()), 
						Int32.Parse(frameData["spriteSourceSize"]["y"].ToString())
				);
				
				Frame frame = new Frame(
					token.Name,
					size,
					origin,
					new Rectangle(
						Int32.Parse(frameData["frame"]["x"].ToString()),
						Int32.Parse(frameData["frame"]["y"].ToString()),
						Int32.Parse(frameData["frame"]["w"].ToString()),
						Int32.Parse(frameData["frame"]["h"].ToString())
					)
				);

				atlassFrames.Add(frame);
			}

			return atlassFrames;
			
		}
	}
}
