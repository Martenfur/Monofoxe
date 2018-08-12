
namespace Monofoxe.Engine.ECS
{
	public interface IComponent
	{
		string Tag {get;}
		Entity Owner {get; set;}
	}
}
