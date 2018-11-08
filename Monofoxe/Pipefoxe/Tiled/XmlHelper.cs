using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Pipefoxe.Tiled
{
	static class XmlHelper
	{
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

		public static bool GetXmlBoolSafe(XmlNode node, string attribute, bool defaultValue = true)
		{
			if (node.Attributes[attribute] != null)
			{
				return node.Attributes[attribute].Value == "1" || node.Attributes[attribute].Value == "true";
			}
			return defaultValue;
		}

		public static object GetXmlEnumSafe<T>(XmlNode node, string attribute, T defaultValue)
		{
			if (node.Attributes[attribute] != null)
			{
				return Enum.Parse(typeof(T), node.Attributes[attribute].Value, true);
			}
			return defaultValue;
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

	}
}
