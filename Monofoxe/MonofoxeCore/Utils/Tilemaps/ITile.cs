
namespace Monofoxe.Utils.Tilemaps
{
	public interface ITile
	{
		int Index {get;}
		bool IsBlank {get;}
		bool FlipHor {get; set;}
		bool FlipVer {get; set;}
	}
}
