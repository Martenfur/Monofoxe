using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Beizer curve.
	/// Stolen from: https://github.com/shamim-akhtar/bezier-curve.
	/// </summary>
	public class BezierCurve
	{

		// A look up table for factorials. Capped to 16.
		private static float[] Factorial = new float[]
		{
			1.0f,
			1.0f,
			2.0f,
			6.0f,
			24.0f,
			120.0f,
			720.0f,
			5040.0f,
			40320.0f,
			362880.0f,
			3628800.0f,
			39916800.0f,
			479001600.0f,
			6227020800.0f,
			87178291200.0f,
			1307674368000.0f,
			20922789888000.0f,
		};

		private static float Binomial(int n, int i)
		{
			float ni;
			float a1 = Factorial[n];
			float a2 = Factorial[i];
			float a3 = Factorial[n - i];

			ni = a1 / (a2 * a3);

			return ni;
		}

		private static float Bernstein(int n, int i, float t)
		{
			float t_i = MathF.Pow(t, i);
			float t_n_minus_i = MathF.Pow((1 - t), (n - i));

			float basis = Binomial(n, i) * t_i * t_n_minus_i;

			return basis;
		}


		/// <summary>
		/// Returns an interpolated point. t must be between 0 and 1.
		/// </summary>
		public static Vector3 GetVector3Point(float t, Vector3[] controlPoints)
		{
			int N = controlPoints.Length - 1;

			if (N > 16)
			{
				throw new Exception("The maximum control points allowed is 16.");
			}


			if (t <= 0)
			{
				return controlPoints[0];
			}
			if (t >= 1)
			{
				return controlPoints[controlPoints.Length - 1];
			}


			Vector3 p = new Vector3();

			for (int i = 0; i < controlPoints.Length; ++i)
			{
				Vector3 bn = Bernstein(N, i, t) * controlPoints[i];

				p += bn;
			}

			return p;
		}


		/// <summary>
		/// Returns an interpolated point. t must be between 0 and 1.
		/// </summary>
		public static Vector2 GetVector2Point(float t, Vector2[] controlPoints)
		{
			int N = controlPoints.Length - 1;

			if (N > 16)
			{
				throw new Exception("The maximum control points allowed is 16.");
			}


			if (t <= 0)
			{
				return controlPoints[0];
			}
			if (t >= 1)
			{
				return controlPoints[controlPoints.Length - 1];
			}


			Vector2 p = new Vector2();

			for (int i = 0; i < controlPoints.Length; ++i)
			{
				Vector2 bn = Bernstein(N, i, t) * controlPoints[i];

				p += bn;
			}

			return p;
		}


		/// <summary>
		/// Returns an array of points spaced with passed interval.
		/// </summary>
		public static Vector2[] GetVector2Points(Vector2[] controlPoints, float interval = 0.01f)
		{
			int N = controlPoints.Length - 1;

			if (N > 16)
			{
				throw new Exception("The maximum control points allowed is 16.");
			}


			List<Vector2> points = new List<Vector2>();

			for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
			{
				Vector2 p = new Vector2();

				for (int i = 0; i < controlPoints.Length; ++i)
				{
					Vector2 bn = Bernstein(N, i, t) * controlPoints[i];

					p += bn;
				}

				points.Add(p);
			}

			return points.ToArray();
		}


		/// <summary>
		/// Returns an array of points spaced with passed interval.
		/// </summary>
		public static Vector3[] GetVector3Points(Vector3[] controlPoints, float interval = 0.01f)
		{
			int N = controlPoints.Length - 1;

			if (N > 16)
			{
				throw new Exception("The maximum control points allowed is 16.");
			}


			List<Vector3> points = new List<Vector3>();

			for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
			{
				Vector3 p = new Vector3();

				for (int i = 0; i < controlPoints.Length; ++i)
				{
					Vector3 bn = Bernstein(N, i, t) * controlPoints[i];

					p += bn;
				}

				points.Add(p);
			}

			return points.ToArray();
		}

		public static void Draw(Vector2[] controlPoints, float interval = 0.01f)
		{
			var points = GetVector2Points(controlPoints, interval);

			for (var i = 0; i < points.Length - 1; i += 1)
			{
				LineShape.Draw(points[i], points[i + 1]);
			}
		}
	}
}
