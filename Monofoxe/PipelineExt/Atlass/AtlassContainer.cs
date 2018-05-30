using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Collections.Generic;

namespace PipelineExt
{
	/// <summary>
	/// Basic atlass container. Stores list of frames or sprites and their texture. 
	/// </summary>
	/// <typeparam name="T">Frame or Sprite.</typeparam>
	public class AtlassContainer<T>
	{
		public List<T> Items;
		public TextureContent Texture;

		public AtlassContainer() =>
			Items = new List<T>();
		
		public void Add(T item) =>
			Items.Add(item);
	}
}
