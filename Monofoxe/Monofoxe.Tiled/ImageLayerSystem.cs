using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;


namespace Monofoxe.Utils.Tilemaps
{
	/// <summary>
	/// 
	/// </summary>
	public class ImageLayerSystem : BaseSystem
	{
		public override string Tag => "imageLayer";
		
		public override void Create(Component image)
		{
			System.Console.WriteLine("I am create event!");
		}

		public override void Draw(List<Component> images)
		{
			foreach(ImageLayerComponent image in images)
			{
				DrawMgr.DrawFrame(image.Frame, Vector2.Zero, -image.Offset);
			}
		}

	}
}
