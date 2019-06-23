using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// A set of cool whooshies for transition effects. 
	/// Easings accept linear value from 0 to 1 and return smoothed out version. 
	/// Can be used as static functions or instantiated.
	/// 
	/// Port of this code: https://github.com/nicolausYes/easing-functions
	/// 
	/// Help on easings and visualization: https://easings.net/en
	/// </summary>
	public class Easing
	{
		/// <summary>
		/// Current easing function.
		/// </summary>
		public readonly Func<double, double> Func;
		
		/// <summary>
		/// This constructor is used to create easings. You can create your own!
		/// </summary>
		public Easing(Func<double, double> func)
		{
			Func = func;
		}

		/// <summary>
		/// Returns easing value for t.
		/// NOTE: t should be in range from 0 to 1.
		/// </summary>
		public float GetEasing(float t) => (float)Func(t);

		/// <summary>
		/// Returns easing value for t.
		/// NOTE: t should be in range from 0 to 1.
		/// </summary>
		public double GetEasing(double t) => Func(t);

		#region Default easings.

		public static readonly Easing EaseInSine = new Easing(EaseInSineFunc);
		public static readonly Easing EaseOutSine = new Easing(EaseOutSineFunc);
		public static readonly Easing EaseInOutSine = new Easing(EaseInOutSineFunc);
		public static readonly Easing EaseInQuad = new Easing(EaseInQuadFunc);
		public static readonly Easing EaseOutQuad = new Easing(EaseOutQuadFunc);
		public static readonly Easing EaseInOutQuad = new Easing(EaseInOutQuadFunc);
		public static readonly Easing EaseInCubic = new Easing(EaseInCubicFunc);
		public static readonly Easing EaseOutCubic = new Easing(EaseOutCubicFunc);
		public static readonly Easing EaseInOutCubic = new Easing(EaseInOutCubicFunc);
		public static readonly Easing EaseInQuart = new Easing(EaseInQuartFunc);
		public static readonly Easing EaseOutQuart = new Easing(EaseOutQuartFunc);
		public static readonly Easing EaseInOutQuart = new Easing(EaseInOutQuartFunc);
		public static readonly Easing EaseInQuint = new Easing(EaseInQuintFunc);
		public static readonly Easing EaseOutQuint = new Easing(EaseOutQuintFunc);
		public static readonly Easing EaseInOutQuint = new Easing(EaseInOutQuintFunc);
		public static readonly Easing EaseInExpo = new Easing(EaseInExpoFunc);
		public static readonly Easing EaseOutExpo = new Easing(EaseOutExpoFunc);
		public static readonly Easing EaseInOutExpo = new Easing(EaseInOutExpoFunc);
		public static readonly Easing EaseInCirc = new Easing(EaseInCircFunc);
		public static readonly Easing EaseOutCirc = new Easing(EaseOutCircFunc);
		public static readonly Easing EaseInOutCirc = new Easing(EaseInOutCircFunc);
		public static readonly Easing EaseInBack = new Easing(EaseInBackFunc);
		public static readonly Easing EaseOutBack = new Easing(EaseOutBackFunc);
		public static readonly Easing EaseInOutBack = new Easing(EaseInOutBackFunc);
		public static readonly Easing EaseInElastic = new Easing(EaseInElasticFunc);
		public static readonly Easing EaseOutElastic = new Easing(EaseOutElasticFunc);
		public static readonly Easing EaseInOutElastic = new Easing(EaseInOutElasticFunc);
		public static readonly Easing EaseInBounce = new Easing(EaseInBounceFunc);
		public static readonly Easing EaseOutBounce = new Easing(EaseOutBounceFunc);
		public static readonly Easing EaseInOutBounce = new Easing(EaseInOutBounceFunc);

		#endregion Default easings.


		#region Easing functions.

		public static double EaseInSineFunc(double t)
		{
			return Math.Sin(1.5707963 * t);
		}

		public static double EaseOutSineFunc(double t)
		{
			return 1 + Math.Sin(1.5707963 * (--t));
		}

		public static double EaseInOutSineFunc(double t)
		{
			return 0.5 * (1 + Math.Sin(3.1415926 * (t - 0.5)));
		}

		public static double EaseInQuadFunc(double t)
		{
			return t * t;
		}

		public static double EaseOutQuadFunc(double t)
		{
			return t * (2 - t);
		}

		public static double EaseInOutQuadFunc(double t)
		{
			return t < 0.5 ? 2 * t * t : t * (4 - 2 * t) - 1;
		}

		public static double EaseInCubicFunc(double t)
		{
			return t * t * t;
		}

		public static double EaseOutCubicFunc(double t)
		{
			return 1 + (--t) * t * t;
		}

		public static double EaseInOutCubicFunc(double t)
		{
			return t < 0.5 ? 4 * t * t * t : 1 + (--t) * (2 * (--t)) * (2 * t);
		}

		public static double EaseInQuartFunc(double t)
		{
			t *= t;
			return t * t;
		}

		public static double EaseOutQuartFunc(double t)
		{
			t = (--t) * t;
			return 1 - t * t;
		}

		public static double EaseInOutQuartFunc(double t)
		{
			if (t < 0.5)
			{
				t *= t;
				return 8 * t * t;
			}
			else
			{
				t = (--t) * t;
				return 1 - 8 * t * t;
			}
		}

		public static double EaseInQuintFunc(double t)
		{
			double t2 = t * t;
			return t * t2 * t2;
		}

		public static double EaseOutQuintFunc(double t)
		{
			double t2 = (--t) * t;
			return 1 + t * t2 * t2;
		}

		public static double EaseInOutQuintFunc(double t)
		{
			double t2;
			if (t < 0.5)
			{
				t2 = t * t;
				return 16 * t * t2 * t2;
			}
			else
			{
				t2 = (--t) * t;
				return 1 + 16 * t * t2 * t2;
			}
		}

		public static double EaseInExpoFunc(double t)
		{
			return (Math.Pow(2, 8 * t) - 1) / 255;
		}

		public static double EaseOutExpoFunc(double t)
		{
			return 1 - Math.Pow(2, -8 * t);
		}

		public static double EaseInOutExpoFunc(double t)
		{
			if (t < 0.5)
			{
				return (Math.Pow(2, 16 * t) - 1) / 510;
			}
			else
			{
				return 1 - 0.5 * Math.Pow(2, -16 * (t - 0.5));
			}
		}

		public static double EaseInCircFunc(double t)
		{
			return 1 - Math.Sqrt(1 - t);
		}

		public static double EaseOutCircFunc(double t)
		{
			return Math.Sqrt(t);
		}

		public static double EaseInOutCircFunc(double t)
		{
			if (t < 0.5)
			{
				return (1 - Math.Sqrt(1 - 2 * t)) * 0.5;
			}
			else
			{
				return (1 + Math.Sqrt(2 * t - 1)) * 0.5;
			}
		}

		public static double EaseInBackFunc(double t)
		{
			return t * t * (2.70158 * t - 1.70158);
		}

		public static double EaseOutBackFunc(double t)
		{
			return 1 + (--t) * t * (2.70158 * t + 1.70158);
		}

		public static double EaseInOutBackFunc(double t)
		{
			if (t < 0.5)
			{
				return t * t * (7 * t - 2.5) * 2;
			}
			else
			{
				return 1 + (--t) * t * 2 * (7 * t + 2.5);
			}
		}

		public static double EaseInElasticFunc(double t)
		{
			double t2 = t * t;
			return t2 * t2 * Math.Sin(t * Math.PI * 4.5);
		}

		public static double EaseOutElasticFunc(double t)
		{
			double t2 = (t - 1) * (t - 1);
			return 1 - t2 * t2 * Math.Cos(t * Math.PI * 4.5);
		}

		public static double EaseInOutElasticFunc(double t)
		{
			double t2;
			if (t < 0.45)
			{
				t2 = t * t;
				return 8 * t2 * t2 * Math.Sin(t * Math.PI * 9);
			}
			else if (t < 0.55)
			{
				return 0.5 + 0.75 * Math.Sin(t * Math.PI * 4);
			}
			else
			{
				t2 = (t - 1) * (t - 1);
				return 1 - 8 * t2 * t2 * Math.Sin(t * Math.PI * 9);
			}
		}

		public static double EaseInBounceFunc(double t)
		{
			return Math.Pow(2, 6 * (t - 1)) * Math.Abs(Math.Sin(t * Math.PI * 3.5));
		}

		public static double EaseOutBounceFunc(double t)
		{
			return 1 - Math.Pow(2, -6 * t) * Math.Abs(Math.Cos(t * Math.PI * 3.5));
		}

		public static double EaseInOutBounceFunc(double t)
		{
			if (t < 0.5)
			{
				return 8 * Math.Pow(2, 8 * (t - 1)) * Math.Abs(Math.Sin(t * Math.PI * 7));
			}
			else
			{
				return 1 - 8 * Math.Pow(2, -8 * t) * Math.Abs(Math.Sin(t * Math.PI * 7));
			}
		}

		#endregion Easing functions.


	}
}
