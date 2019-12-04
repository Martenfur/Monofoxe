using System.Collections.Generic;

namespace Monofoxe.Engine.EC
{
	internal class EntityDepthComparer : IComparer<Entity>
	{
		public int Compare(Entity x, Entity y) =>
			y.Depth - x.Depth;
	}
}
