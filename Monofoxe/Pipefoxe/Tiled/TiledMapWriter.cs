using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Monofoxe.Tiled.MapStructure;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Pipefoxe.Tiled
{
	[ContentTypeWriter]
	public class TiledMapWriter : ContentTypeWriter<TiledMap>
	{
		protected override void Write(ContentWriter output, TiledMap value)
		{
			/*output.WriteObject(value.BackgroundColor);
			output.Write(value.Width);
			output.Write(value.Height);
			output.Write(value.TileWidth);
			output.Write(value.TileHeight);
			//output.WriteObject(value.Tilesets);
			//output.WriteObject(value.TileLayers);
			output.WriteObject(value.Properties);*/

			MemoryStream m = new MemoryStream();
			var formatter = new BinaryFormatter();
			formatter.Serialize(m, value);
			byte[] buf = m.ToArray(); //or File.WriteAllBytes(filename, m.ToArray())
			output.Write(buf);


		}



		public override string GetRuntimeType(TargetPlatform targetPlatform) =>
			"Monofoxe.Tiled.MapStructure.TiledMap, Monofoxe.Tiled";



		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Tiled.ContentReaders.TiledMapReader, Monofoxe.Tiled";
	}
}

