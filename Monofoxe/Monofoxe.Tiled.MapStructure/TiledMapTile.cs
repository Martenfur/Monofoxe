
namespace Monofoxe.Tiled.MapStructure
{
	public struct TiledMapTile
	{
		public int GID;
		public bool FlipHor;
		public bool FlipVer;

		/// <summary>
		/// Diagonal flip is weird diagonal axis filp, which enables rotation.
		/// 
		/// Graphical:
		/// 0 0 ' 0 0     0 0 ' 0 0
		/// 0 ' ' 0 0 ==> 0 ' 0 0 '
		/// ' 0 ' 0 0 ==> ' ' ' ' '
		/// 0 0 ' 0 0 ==> 0 0 0 0 '
		/// 0 ' ' ' 0     0 0 0 0 0
		/// 
		/// Vertices:
		/// 0 1 => 0 2
		/// 2 3 => 1 3
		/// 
		/// (90 deg rotation) + (-1 xscale)
		/// </summary>
		public bool FlipDiag;

		public bool IsBlank => GID == 0;	
	}
}
