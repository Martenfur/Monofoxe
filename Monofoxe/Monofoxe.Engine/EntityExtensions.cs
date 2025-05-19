using Monofoxe.Engine.EC;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine
{
	public static class EntityExtensions
	{
		public static PositionComponent GetPosition(this Entity entity) =>
			entity.GetComponent<PositionComponent>();
	}
}
