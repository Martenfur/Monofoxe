using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Pipefoxe.SpriteGroup
{
	public static class ClassGenerator
	{
		private static char _variableKeychar = '#';

		/// <summary>
		/// Used when file name begins with a char, not allowed in variable names.
		/// </summary>
		private static string _paddingStr = "S";



		public static void Generate(string templatePath, string outPath, List<RawSprite> sprites, string groupName)
		{
			Console.WriteLine("Starting class generation.");
			Console.WriteLine();

			string[] lines = File.ReadAllLines(templatePath);

			var customVariableNames = new List<string>();
			var customVariableValues = new List<string>();
			
			var codeTemplate = new StringBuilder();

			var parseVariables = true;

			#region Extracting custom variables.
			foreach(string l in lines)
			{
				if (parseVariables)
				{
					// Parsing variables.				
					if (l.Length > 0 && l[0] == _variableKeychar)
					{
						Match nameMatch = Regex.Match(l, _variableKeychar + @"(.+?)\s*="); //. - At least one char. ? - non-greedy, \s* - optional space
						Match valueMatch = Regex.Match(l, '"' + @"(.+?)" + '"', RegexOptions.IgnoreCase);
						
						if (nameMatch.Success && valueMatch.Success)
						{
							customVariableNames.Add(nameMatch.Value.Substring(1,nameMatch.Value.Length - 2).Replace(" ", ""));
							customVariableValues.Add(valueMatch.Value.Substring(1,valueMatch.Value.Length - 2));					
						}
						else
						{
							throw(new Exception("ERROR: Cannot parse variable. Aborting class generating."));
						}
					}
					
					if ((l.Length > 0 && l[0] != _variableKeychar))
					{
						parseVariables = false;
					}
					// Parsing variables.
				}
				
				if (!parseVariables) // Not using else, because in first if parseValues may change.
				{
					// Copying rest of the code.
					codeTemplate.Append(l + Environment.NewLine);
					// Copying rest of the code.
				}
				
			}
			#endregion Extracting custom variables.
			
			
			
			var code = new StringBuilder();
			code.Append(codeTemplate);
				
			#region Resolving name conflicts.

			/*
			 * Some sprites may have identical names.
			 * Even though this is bad practice, generator 
			 * allows this and resolves name conflicts.
			 */

			var spriteNames = new List<string>();
			var spriteOccurences = new Dictionary<string, int>();
			for(var i = 0; i < sprites.Count; i += 1)
			{
				var name = ToCamelCase(Path.GetFileName(sprites[i].Name));
				if (spriteOccurences.ContainsKey(name))
				{
					spriteNames.Add(name + '_' + spriteOccurences[name]);
					spriteOccurences[name] += 1;
				}
				else
				{
					spriteOccurences.Add(name, 1);
					spriteNames.Add(name);
				}
			}
			#endregion Resolving name conflicts.
			

			#region Assembling variables from templates.
			var completeVariableValues = new List<StringBuilder>();

			for(var k = 0; k < customVariableValues.Count; k += 1)
			{
				completeVariableValues.Add(new StringBuilder());
				var lastSprite = sprites.Last();

				var i = 0;
				foreach(RawSprite sprite in sprites)
				{
					var v = customVariableValues[k]
						.Replace("<sprite_name>", spriteNames[i])
						.Replace("<hash_sprite_name>", '"' + sprite.Name + '"');
					i += 1;

					completeVariableValues[k].Append(v);

					if (sprite != lastSprite)
					{
						completeVariableValues[k].AppendLine();
					}
					
				}
			}
			#endregion Assembling variables from templates.
		
			
			code = code.Replace("<group_name>", ToCamelCase(groupName));
			
			for(var i = 0; i < customVariableNames.Count; i += 1)
			{
				code = code.Replace('<' + customVariableNames[i] + '>', completeVariableValues[i].ToString());
			}

			// Now tabs are all messed up. Fixing this just to make code look pretteh.
			FixTabulation(code);
			
			File.WriteAllText(outPath + '/' + ToCamelCase(groupName) + ".cs", code.ToString());
		}



		/// <summary>
		/// Fixes tabs in code.
		/// </summary>
		private static void FixTabulation(StringBuilder str)
		{
			var buffer = str.ToString().Replace("\t", ""); // Removing all tabs from code.
			str.Clear();
			str.Append(buffer);
			
			var bracketCount = 0;

			var strPtr = 0;
			for(var i = 0; i < buffer.Length - 1; i += 1)
			{
				if (buffer[i] == '{')
				{
					bracketCount += 1;
				}
				if (buffer[i + 1] == '}')
				{
					bracketCount -= 1;
				}
				if (buffer[i] == '\n')
				{
					str.Insert(strPtr + 1, "\t", bracketCount);	
					strPtr += bracketCount;
				}	
				strPtr += 1;
			}
		}
	

		
		/// <summary>
		/// Converts regular string to compiler-accepted CamelCase variable name.
		/// some_stuff => SomeStuff
		/// </summary>
		public static string ToCamelCase(string str)
		{
			// Removing prohibited symbols from string.
			Regex rgx = new Regex("[^a-zA-Z0-9 _]"); 
			str = rgx.Replace(str, "");

			if (str.Length == 0) 
			{
				return "";
			}
			// Removing prohibited symbols from string.
			
			string[] words = str.Split(new char[]{'_', ' '});
			StringBuilder upper = new StringBuilder();
			
			if (Char.IsDigit(str[0])) // Variable name cannot begin with a digit.
			{
				upper.Append(_paddingStr);
			}
			if (str[0] == '_') // For variables which begin with underscore.
			{
				upper.Append('_');
			}
					
			// Making each word begin with an uppercase letter.
			foreach(string word in words) 
			{
				if (word.Length > 0)
				{
					upper.Append(Char.ToUpper(word[0]) + word.Substring(1, word.Length - 1));
				}
			}
			// Making each word begin with an uppercase letter.

			return upper.ToString();
		}

	}
}
