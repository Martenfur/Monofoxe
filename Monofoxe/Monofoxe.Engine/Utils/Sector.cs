
namespace Monofoxe.Engine.Utils
{
	public struct Sector
	{
		/// <summary>
		/// Center direction of the sector.
		/// </summary>
		public Angle Direction;

		public Angle Arc;

		public float Radius;

		public Angle LeftBound => Direction - Arc / 2;
		public Angle RightBound => Direction + Arc / 2;


		/// <summary>
		/// Returns true is a sector contains given angle.
		/// </summary>
		public bool Contains(Angle angle) =>
			LeftBound.Difference(angle) >= 0 && RightBound.Difference(angle) <= 0;		
	}
}
