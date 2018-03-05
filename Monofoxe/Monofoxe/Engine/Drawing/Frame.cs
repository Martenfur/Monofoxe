using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using System;
using System.IO;


namespace Monofoxe.Engine.Drawing
{
	public class Frame
	{
		/// <summary>
		/// Texture atlass where frame is stored.
		/// </summary>
		public readonly Texture2D Texture;

		/// <summary>
		/// Position of the frame on the atlass. Note that it may be rotated.
		/// </summary>
		public readonly Rectangle TexturePosition;

		/// <summary>
		/// Width of the frame.
		/// </summary>
		public readonly int W;

		/// <summary>
		/// Height of the frame.
		/// </summary>
		public readonly int H;

		/// <summary>
		/// Tells if frame is rotated on the texture atlass.
		/// </summary>
		public readonly bool IsRotated;


		public Frame(Texture2D texture, Rectangle texturePosition, bool isRotated)
		{
			Texture = texture;
			TexturePosition = texturePosition;
			IsRotated = isRotated;

			if (isRotated)
			{
				W = texturePosition.Height;
				H = texturePosition.Width;
			}
			else
			{
				W = texturePosition.Width;
				H = texturePosition.Height;
			}
		}


		public static Dictionary<string, Frame[]> LoadFrames(Texture2D atlass, string xmlPath)
		{
			Dictionary<string, Frame[]> dictionary = new Dictionary<string, Frame[]>();
			
			XmlDocument xml = new XmlDocument();
			xml.Load(xmlPath);
			
			XmlNodeList nodes = xml.DocumentElement.SelectNodes("sprite");
			
			int previousFrameId = -1;
			string previousFrameKey = "";

			List<Frame> frameList = new List<Frame>();

			foreach(XmlNode node in nodes)
			{
				string filename = Path.GetFileNameWithoutExtension(node.Attributes["n"].Value);

				int frameId = Int32.Parse(filename.Substring(filename.LastIndexOf('_') + 1));
				

				string frameKey = node.Attributes["n"].Value.Replace(filename.Substring(filename.LastIndexOf('_')), "");
				if (previousFrameKey.Length == 0)
				{previousFrameKey = frameKey;}

				Frame f = new Frame(
					atlass, 
					new Rectangle(
						Int32.Parse(node.Attributes["x"].Value),
						Int32.Parse(node.Attributes["y"].Value),
						Int32.Parse(node.Attributes["w"].Value),
						Int32.Parse(node.Attributes["h"].Value)
					),
					node.Attributes["r"] != null && node.Attributes["r"].Value[0] == 'y'
				);

				if (frameId <= previousFrameId)
				{
					if (frameList.Count > 0)
					{
						dictionary.Add(previousFrameKey, frameList.ToArray());
						
						Debug.WriteLine(previousFrameKey + ": " + frameList.Count);
						previousFrameKey = frameKey;

						frameList.Clear();
					}
				}
				
				previousFrameId = frameId;
				frameList.Add(f);
			}

			if (frameList.Count > 0)
			{
				dictionary.Add(previousFrameKey, frameList.ToArray());
				frameList.Clear();
			}
		
			return dictionary;

		}
	}
}
