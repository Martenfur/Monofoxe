/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Shake.Utils
{
	/// <summary>
	/// Representation of translation and rotation. 
	/// </summary>
	public struct Displacement
	{
		public Vector2 Position;
		public float Angle;

		public Displacement(Vector2 position, float angle)
		{
			Position = position;
			Angle = angle;
		}

		public Displacement(Vector2 position)
		{
			Position = position;
			Angle = 0;
		}

		public static Displacement Zero =>
			new Displacement(Vector2.Zero, 0);

		public Displacement Normalized =>
			new Displacement(Position.SafeNormalize(), Angle);

		public static Displacement operator +(Displacement a, Displacement b) =>
			new Displacement(a.Position + b.Position, b.Angle + a.Angle);

		public static Displacement operator -(Displacement a, Displacement b) =>
			new Displacement(a.Position - b.Position, b.Angle - a.Angle);

		public static Displacement operator -(Displacement disp) =>
			new Displacement(-disp.Position, -disp.Angle);

		public static Displacement operator *(Displacement coords, float number) =>
			new Displacement(coords.Position * number, coords.Angle * number);

		public static Displacement operator *(float number, Displacement coords) =>
			coords * number;

		public static Displacement operator /(Displacement coords, float number) =>
			new Displacement(coords.Position / number, coords.Angle / number);

		public static Displacement Scale(Displacement a, Displacement b) =>
			new Displacement(a.Position * b.Position, b.Angle * a.Angle);

		public static Displacement Lerp(Displacement a, Displacement b, float t) =>
			new Displacement(Vector2.Lerp(a.Position, b.Position, t), MathHelper.Lerp(a.Angle, b.Angle, t));

		public Displacement ScaledBy(float posScale, float rotScale) =>
			new Displacement(Position * posScale, Angle * rotScale);

		public static Displacement InsideUnitSpheres() =>
			new Displacement(RandomExt.Global.NextInsideUnitCircle(), 0);
	}
}
