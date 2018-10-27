using System;
using System.Collections.Generic;
using Monofoxe.Tiled.MapStructure;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;

namespace Pipefoxe.Tiled
{
	
	static class TilesetParser
	{
		/// <summary>
		/// Tells pipeline, that this tileset won't be used in the game completely.
		/// If this tileset property is set to "true", tileset won't be loaded into final map.
		/// </summary>
		const string _ignoreTilesetFlag = "__ignoreTileset";

		/// <summary>
		/// Tells pipeline, that this tileset won't need Tiled texture.
		/// If this tileset property is set to "true", game won't require tileset texture during
		/// map loading.
		/// NOTE: You should still provide the texture to the tileset on your own, if you want to use it in game with default tilesets.
		/// </summary>
		const string _ignoreTilesetTextureFlag = "__ignoreTilesetTexture";


		public static TiledMapTileset[] Parse(XmlNodeList nodes)
		{
			var tilesets = new List<TiledMapTileset>();

			foreach(XmlNode tilesetXml in nodes)
			{
				var tileset = ParseTileset(tilesetXml);
				if (tileset != null)
				{
					tilesets.Add(tileset);
				}
			}
			return tilesets.ToArray();
		}

		static TiledMapTileset ParseTileset(XmlNode tilesetXml)
		{
			var tileset = new TiledMapTileset();
			tileset.FirstGID = int.Parse(tilesetXml.Attributes["firstgid"].Value);
			
			if (tilesetXml.Attributes["source"] != null)
			{
				// If there is "source" field, that means, tileset is external.
				var doc = new XmlDocument();
				try
				{
					doc.Load(TiledMapImporter.RootDir + tilesetXml.Attributes["source"].Value);
					tilesetXml = doc["tileset"];
				}
				catch(Exception e)
				{
					throw new Exception("Error loading external tileset! " + e.StackTrace);
				}
			}
			tileset.Properties = TiledMapImporter.GetProperties(tilesetXml);

			// This means, that tileset won't be used in the game and should be ignored.
			if (tileset.Properties.ContainsKey(_ignoreTilesetFlag) && tileset.Properties[_ignoreTilesetFlag] == "true")
			{
				return null;
			}

			#region Main fields.
			tileset.Name = tilesetXml.Attributes["name"].Value;
			tileset.TileWidth = int.Parse(tilesetXml.Attributes["tilewidth"].Value);
			tileset.TileHeight = int.Parse(tilesetXml.Attributes["tileheight"].Value);
			tileset.TileCount = int.Parse(tilesetXml.Attributes["tilecount"].Value);
			tileset.Columns = int.Parse(tilesetXml.Attributes["columns"].Value);
				
			tileset.Margin = TiledMapImporter.GetXmlIntSafe(tilesetXml, "margin");
			tileset.Spacing = TiledMapImporter.GetXmlIntSafe(tilesetXml, "spacing");
			
			if (tilesetXml.Attributes["backgroundcolor"] != null)
			{
				tileset.BackgroundColor = TiledMapImporter.StringToColor(tilesetXml.Attributes["backgroundcolor"].Value);
			}

			if (tilesetXml["tileoffset"] != null)
			{
				tileset.Offset = new Vector2(
					float.Parse(tilesetXml["tileoffset"].Attributes["x"].Value, CultureInfo.InvariantCulture),
					float.Parse(tilesetXml["tileoffset"].Attributes["y"].Value, CultureInfo.InvariantCulture)
				);
			}
			#endregion Main fields.
			
			// Turning tile xml into usable dictionary. 
			var tilesXml = tilesetXml.SelectNodes("tile");
			
			// List won't suit, because some tile ids may be skipped.
			var tiles = new Dictionary<int, XmlNode>();
			foreach(XmlNode tile in tilesXml)
			{
				tiles.Add(int.Parse(tile.Attributes["id"].Value), tile);
			}
			// Turning tile xml into usable dictionary.

			/*
			 * It is very problematic to load Texture2D without
			 * GraphicsDevice, so textures are imported separately
			 * through Pipeline. At this stage map will just remember their 
			 * relative paths, and will pick textures up later.
			 */
			TiledMapImporter.__Log(tileset.Name + ":");
			if (tilesetXml["image"] != null)
			{
				/*
				 * NOTE: Single image tilesets can still have 
				 * <tile> tags with properties.
				 */
				// Single-image tileset.
				var texturePaths = new string[1];

				texturePaths[0] = tilesetXml["image"].Attributes["source"].Value;
				tileset.TexturePaths = texturePaths;

				TiledMapImporter.__Log("Single image tileset: " + tileset.TexturePaths[0]);

				var currentID = 0;
				for(var y = 0; y < tileset.Height; y += 1)
				{
					for(var x = 0; x < tileset.Width; x += 1)
					{
						var tile = new TiledMapTilesetTile();	
						tile.Tileset = tileset;
						tile.GID = tileset.FirstGID + currentID;
						tile.TextureID = 0;
						tile.TexturePosition = new Rectangle(
							tileset.Margin + x * (tileset.TileWidth + tileset.Spacing), 
							tileset.Margin + y * (tileset.TileHeight + tileset.Spacing), 
							tileset.TileWidth, 
							tileset.TileHeight
						);

						if (tiles.ContainsKey(currentID))
						{
							tileset.Properties = TiledMapImporter.GetProperties(tiles[currentID]);
						}
						else
						{
							tileset.Properties = new Dictionary<string, string>();
						}
						currentID += 1;
					}
				}
				// Single-image tileset.
			}
			else
			{
				// Image collection tileset.
				var texturePaths = new List<string>();
				
				foreach(KeyValuePair<int, XmlNode> nodePair in tiles)
				{
					var tile = new TiledMapTilesetTile();	
					tile.Tileset = tileset;
					tile.GID = tileset.FirstGID + nodePair.Key;
					
					var texturePath = nodePair.Value["image"].Attributes["source"].Value;
					if (texturePaths.Contains(texturePath))
					{
						// Avoiding duplicates.
						tile.TextureID = texturePaths.IndexOf(texturePath);
					}
					else
					{
						tile.TextureID = texturePaths.Count;
						texturePaths.Add(texturePath);
					}
					TiledMapImporter.__Log("Image collection tileset: " + texturePath);

					tile.TexturePosition = new Rectangle(
						0, 0,
						int.Parse(nodePair.Value["image"].Attributes["width"].Value),
						int.Parse(nodePair.Value["image"].Attributes["height"].Value)
					);
					
					tile.Properties = TiledMapImporter.GetProperties(nodePair.Value);
				}
				tileset.TexturePaths = texturePaths.ToArray();
				// Image collection tileset.	
			}

			return tileset;
		}


	}
}
