using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	public class BezierCurve
	{
		public static void Draw(Vector2[] controlPoints, float interval = 0.01f)
		{
			var points = GameMath.BezierCurvePoints(controlPoints, interval);

			for (var i = 0; i < points.Length - 1; i += 1)
			{
				LineShape.Draw(points[i], points[i + 1]);
			}
		}
	}
}
