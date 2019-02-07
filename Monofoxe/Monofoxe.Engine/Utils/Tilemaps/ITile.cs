
namespace Monofoxe.Engine.Utils.Tilemaps
{
	/// <summary>
	/// Tile interface. 
	/// </summary>
	public interface ITile
	{
		int Index {get; set;}
		bool IsBlank {get;}
		bool FlipHor {get; set;}
		bool FlipVer {get; set;}
	}
}
