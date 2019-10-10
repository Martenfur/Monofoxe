
namespace Monofoxe.Engine.Resources
{

	/// <summary>
	/// Interface for the container of game resources.
	/// Resources can be accessed by the string key.
	/// </summary>
	public interface IResourceBox
	{
		bool Loaded {get;}

		void Load();
		void Unload();
	}
}
