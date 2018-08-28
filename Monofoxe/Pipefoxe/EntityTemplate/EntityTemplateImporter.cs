using Microsoft.Xna.Framework.Content.Pipeline;
using System.Text;
using System.IO;


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
namespace Pipefoxe.EntityTemplate
{
	/// <summary>
	/// Imports entity template json. 
	/// </summary>
	[ContentImporter(".json", DefaultProcessor = "EntityTemplateProcessor", 
	DisplayName = "Entity Template Importer - Monofoxe")]
	public class EntityTemplateImporter : ContentImporter<byte[]>
	{
		public override byte[] Import(string filename, ContentImporterContext context) =>
			Encoding.UTF8.GetBytes(File.ReadAllText(filename)); // Converting UTF8 to UTF16. If we won't do this, there will be derpy symbols.
	}
		
}
