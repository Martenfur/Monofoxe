using System;
using System.Collections.Generic;
using Monofoxe.Tiled.MapStructure;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace Pipefoxe.Tiled
{
	[ContentImporter(".tmx", DefaultProcessor = "TiledMapProcessor",//"PassThroughProcessor", 
	DisplayName = "Tiled Map Importer - Monofoxe")]
	public class TiledMapImporter : ContentImporter<TiledMap>
	{
		static StringBuilder _logs = new StringBuilder();

		public static string RootDir;

		public override TiledMap Import(string filename, ContentImporterContext context)
		{
			RootDir = Path.GetDirectoryName(filename) + '/';
			
			var xml = new XmlDocument();
			xml.Load(filename);

			try
			{
				var map = MapParser.Parse(xml);

				__SaveLog(RootDir);
				return map;
			}
			catch(Exception e)
			{
				__SaveLog(RootDir);
				throw new Exception(e.StackTrace);
			}
		}



		
		public static void __Log(string text)
		{
			_logs.Append(text + Environment.NewLine);
		}



		public static void __SaveLog(string path)
		{
			File.WriteAllText(path + "log.log", _logs.ToString());
		}



		/// <summary>
		/// Converts string in hex format #RRGGBB or #AARRGGBB to Color.
		/// </summary>
		public static Color StringToColor(string colorStr)
		{
			colorStr = colorStr.Replace("#", "");

			var channels = new byte[colorStr.Length / 2];

			for(var i = 0; i < channels.Length; i += 1)
			{
				channels[i] = Convert.ToByte(colorStr.Substring(i * 2, 2), 16);
			}

			if (channels.Length == 3)
			{
				// #RRGGBB
				return new Color(channels[0], channels[1], channels[2]);
			}
			else
			{
				// #AARRGGBB
				return new Color(channels[1], channels[2], channels[3], channels[0]);
			}
		}

		/// <summary>
		/// Returns properties from xml node. 
		/// </summary>
		public static Dictionary<string, string> GetProperties(XmlNode node)
		{
			var dictionary = new Dictionary<string, string>();
			
			if (node["properties"] != null)
			{
				try
				{
					var nodeList = node["properties"].SelectNodes("property");
					foreach(XmlNode propertyXml in nodeList)
					{
						dictionary.Add(propertyXml.Attributes["name"].Value, propertyXml.Attributes["value"].Value);
					}
				}
				catch(Exception e)
				{
					throw new Exception("Error while parsing properties!" + e.StackTrace);
				}
			}
			
			return dictionary;
		}

		public static string GetXmlStringSafe(XmlNode node, string attribute)
		{
			if (node.Attributes[attribute] != null)
			{
				return node.Attributes[attribute].Value;
			}
			return "";
		}

		public static int GetXmlIntSafe(XmlNode node, string attribute)
		{
			if (node.Attributes[attribute] != null)
			{
				return int.Parse(node.Attributes[attribute].Value);
			}
			return 0;
		}

		public static float GetXmlFloatSafe(XmlNode node, string attribute)
		{
			if (node.Attributes[attribute] != null)
			{
				return float.Parse(node.Attributes[attribute].Value, CultureInfo.InvariantCulture);
			}
			return 0f;
		}

		public static bool GetXmlBoolSafe(XmlNode node, string attribute)
		{
			if (node.Attributes[attribute] != null)
			{
				return node.Attributes[attribute].Value == "1" || node.Attributes[attribute].Value == "true";
			}
			return true;
		}

	}
}
