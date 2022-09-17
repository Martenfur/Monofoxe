using System;

namespace Monofoxe.Engine.Utils
{

	public enum NoiseType
	{
		Carmody,
		Gustavson
	}

	public static class PerlinNoise
	{

		// Random seed
		private static int _seed = new Random((int)DateTime.Now.Ticks).Next();
		public static int Seed
		{
			get => _seed;
			set
			{
				_seed = value;
				_reseed(); // Carmody
				_recalculatePermutations(); // Gustavson
			}
		}


		/* Based off the Simplex implementation by Stephen Carmody
		 * http://stephencarmody.wikispaces.com/Simplex+Noise
		 */
		#region Carmody Variables and Properties
		private static int i, j, k;
		private static int[] A = new int[] { 0, 0, 0 };
		private static float u, v, w, s;
		private static float onethird = 0.333333333f;
		private static float onesixth = 0.166666667f;
		private static int[] T;
		#endregion


		/* Based off the Simplex implementation by Stefan Gustavson
		 * http://webstaff.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf
		 * (https://gist.github.com/1759876)
		 */
		#region Gustavson Variables and Properties
		private static int[][] grad3 = new int[][] {
			new int[] {1,1,0}, new int[] {-1,1,0}, new int[] {1,-1,0}, new int[] {-1,-1,0},
			new int[] {1,0,1}, new int[] {-1,0,1}, new int[] {1,0,-1}, new int[] {-1,0,-1},
			new int[] {0,1,1}, new int[] {0,-1,1}, new int[] {0,1,-1}, new int[] {0,-1,-1}
		};

		private static int[] p = {
			151,160,137,91,90,15,
			131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
			190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
			88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
			77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
			102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
			135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
			5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
			223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
			129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
			251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
			49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
			138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
		};
		private static int[] perm = new int[512];
		private static int _halfLength = 256;
		#endregion


		/// <summary>
		/// Constructor
		/// </summary>
		static PerlinNoise() { for (int i = 0; i < 512; i++) perm[i] = p[i & 255]; }

		#region Fractional Brownian Motion
		/// <summary>
		/// Generates a 2D Fractional Brownian Motion noise on top of Simplex noise generated
		/// </summary>
		/// <param name="type">Carmody or Gustavson Noise</param>
		/// <param name="height">Height of the grid being generated</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="octaves">Number of layers</param>
		/// <param name="frequency">Level of detail</param>
		/// <param name="amplitude">How tall we want stuff</param>
		/// <param name="gain">Persistence, makes amplitude shrink or not shrink (0.5f is standard with Lacunarity of 2.0f)</param>
		/// <param name="lacunarity">Multiplied with frequency every octave (2.0f is standard)</param>
		/// <returns>Noise value for input parameters</returns>
		static float FractionalBrownianMotion(NoiseType type, int height, float x, float y, int octaves, float frequency, float amplitude, float gain = 0.5f, float lacunarity = 2.0f)
		{
			// For each grid point, get the value
			float total = 0.0f;
			frequency = 1.0f / height;
			amplitude = gain;

			// For each octave generate noise and add it towards the total
			for (i = 0; i < octaves; ++i)
			{
				if (type == NoiseType.Carmody)
					total += CarmodyNoise(x * frequency, 0, y * frequency) * amplitude;
				else
					total += GustavsonNoise(x * frequency, y * frequency) * amplitude;
				frequency *= lacunarity;
				amplitude *= gain;
			}

			//now that we have the value, put it in
			return total;
		}

		/// <summary>
		/// Generates a 3D Fractional Brownian Motion noise on top of Simplex noise generated
		/// </summary>
		/// <param name="type">Carmody or Gustavson Noise</param>
		/// <param name="height">Height of the grid being generated</param>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="z">Z position</param>
		/// <param name="octaves">Number of layers</param>
		/// <param name="frequency">Level of detail</param>
		/// <param name="amplitude">How tall we want stuff</param>
		/// <param name="gain">Persistence, makes amplitude shrink or not shrink (0.5f is standard with Lacunarity of 2.0f)</param>
		/// <param name="lacunarity">Multiplied with frequency every octave (2.0f is standard)</param>
		/// <returns>Noise value for input parameters</returns>
		static float FractionalBrownianMotion(NoiseType type, int height, float x, float y, float z, int octaves, float frequency, float amplitude, float gain = 0.5f, float lacunarity = 2.0f)
		{
			// For each grid point, get the value
			float total = 0.0f;
			frequency = 1.0f / height;
			amplitude = gain;

			// For each octave generate noise and add it towards the total 
			for (i = 0; i < octaves; ++i)
			{
				if (type == NoiseType.Carmody)
					total += CarmodyNoise(x * frequency, y * frequency, z * frequency) * amplitude;
				else
					total += GustavsonNoise(x * frequency, y * frequency, z * frequency) * amplitude;
				frequency *= lacunarity;
				amplitude *= gain;
			}


			return total;
		}
		#endregion

		#region Carmody Simplex Generator
		/// <summary>
		/// Carmody's implementation of a Simplex Noise generator
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="z">Z position</param>
		/// <param name="doReseed">Force reseeding of the permutations array</param>
		/// <param name="doNormalize">Normalize to [0 .. 1] instead</param>
		/// <returns>A value in the range of about [-1 .. 1]</returns>
		public static float CarmodyNoise(float x, float y, bool doReseed = false, bool doNormalize = true) =>
			CarmodyNoise(x, y, 0, doReseed, doNormalize);

		/// <summary>
		/// Carmody's implementation of a Simplex Noise generator
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="z">Z position</param>
		/// <param name="doReseed">Force reseeding of the permutations array</param>
		/// <param name="doNormalize">Normalize to [0 .. 1] instead</param>
		/// <returns>A value in the range of about [-1 .. 1]</returns>
		public static float CarmodyNoise(float x, float y, float z, bool doReseed = false, bool doNormalize = true)
		{
			if (doReseed)
				_reseed();

			// Skew input space to relative coordinate in simplex cell
			s = (x + y + z) * onethird;
			i = _fastfloor(x + s);
			j = _fastfloor(y + s);
			k = _fastfloor(z + s);

			// Unskew cell origin back to (x, y , z) space
			s = (i + j + k) * onesixth;
			u = x - i + s;
			v = y - j + s;
			w = z - k + s; ;

			A[0] = A[1] = A[2] = 0;

			// For 3D case, the simplex shape is a slightly irregular tetrahedron.
			// Determine which simplex we're in
			int hi = u >= w ? u >= v ? 0 : 1 : v >= w ? 1 : 2;
			int lo = u < w ? u < v ? 0 : 1 : v < w ? 1 : 2;

			// Check whether to normalize to [0 .. 1]
			if (doNormalize)
				return Normalize(_k(hi) + _k(3 - hi - lo) + _k(lo) + _k(0), -0.347f, 0.347f);

			return NormalizeSigned(_k(hi) + _k(3 - hi - lo) + _k(lo) + _k(0), -0.347f, 0.347f);
		}
		#endregion

		#region Gustavson Simplex Generator
		/// <summary>
		/// Gustavson's implementation of a 2D Simplex Noise generator
		/// </summary>
		/// <param name="xin">X position</param>
		/// <param name="yin">Y Position</param>
		/// <param name="doRecalculate">Force recalculations of permutations (can also be done by setting the NoiseHelper.Seed property)</param>
		/// <param name="doNormalize">Normalize the output to 0.0 .. 1.0</param>
		/// <returns>A value in the range of about -1.0 .. 1.0</returns>
		public static float GustavsonNoise(float xin, float yin, bool doRecalculate = false, bool doNormalize = true)
		{
			if (doRecalculate)
				_recalculatePermutations();

			float n0, n1, n2; // Noise contributions from the three corners
							  // Skew the input space to determine which simplex cell we're in
			float F2 = 0.5f * ((float)Math.Sqrt(3.0f) - 1.0f);
			float s = (xin + yin) * F2; // Hairy factor for 2D
			int i = _fastfloor(xin + s);
			int j = _fastfloor(yin + s);
			float G2 = (3.0f - (float)Math.Sqrt(3.0f)) / 6.0f;
			float t = (i + j) * G2;
			float X0 = i - t; // Unskew the cell origin back to (x,y) space
			float Y0 = j - t;
			float x0 = xin - X0; // The x,y distances from the cell origin
			float y0 = yin - Y0;
			// For the 2D case, the simplex shape is an equilateral triangle.
			// Determine which simplex we are in.
			int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
			if (x0 > y0) { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
			else { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)
										  // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
										  // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
										  // c = (3-Sqrt(3))/6
			float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
			float y1 = y0 - j1 + G2;
			float x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
			float y2 = y0 - 1.0f + 2.0f * G2;
			// Work out the hashed gradient indices of the three simplex corners
			int ii = i & 255;
			int jj = j & 255;
			int gi0 = perm[ii + perm[jj]] % 12;
			int gi1 = perm[ii + i1 + perm[jj + j1]] % 12;
			int gi2 = perm[ii + 1 + perm[jj + 1]] % 12;
			// Calculate the contribution from the three corners
			float t0 = 0.5f - x0 * x0 - y0 * y0;
			if (t0 < 0)
				n0 = 0.0f;
			else
			{
				t0 *= t0;
				n0 = t0 * t0 * dot(grad3[gi0], x0, y0);  // (x,y) of grad3 used for 2D gradient
			}
			float t1 = 0.5f - x1 * x1 - y1 * y1;
			if (t1 < 0)
				n1 = 0.0f;
			else
			{
				t1 *= t1;
				n1 = t1 * t1 * dot(grad3[gi1], x1, y1);
			}
			float t2 = 0.5f - x2 * x2 - y2 * y2;
			if (t2 < 0)
				n2 = 0.0f;
			else
			{
				t2 *= t2;
				n2 = t2 * t2 * dot(grad3[gi2], x2, y2);
			}

			// Check whether to return normalized [0,1]
			if (doNormalize)
				return Normalize(70.0f * (n0 + n1 + n2), -1, 1);

			// Add contributions from each corner to get the final noise value.
			// The result is scaled to return values in the interval [-1,1].
			return 70.0f * (n0 + n1 + n2);
		}

		/// <summary>
		/// Gustavson's implementation of a 3D Simplex Noise generator
		/// </summary>
		/// <param name="xin">X position</param>
		/// <param name="yin">Y Position</param>
		/// <param name="zin">Z position</param>
		/// <param name="doRecalculate">Force recalculations of permutations (can also be done by setting the NoiseHelper.Seed property)</param>
		/// <returns>A value in the range of about -1.0 .. 1.0</returns>
		public static float GustavsonNoise(float xin, float yin, float zin, bool doRecalculate = false, bool doNormalize = true)
		{
			if (doRecalculate)
				_recalculatePermutations();

			float n0, n1, n2, n3; // Noise contributions from the four corners
								  // Skew the input space to determine which simplex cell we're in
			float F3 = 1.0f / 3.0f;
			float s = (xin + yin + zin) * F3; // Very nice and simple skew factor for 3D
			int i = _fastfloor(xin + s);
			int j = _fastfloor(yin + s);
			int k = _fastfloor(zin + s);
			float G3 = 1.0f / 6.0f; // Very nice and simple unskew factor, too
			float t = (i + j + k) * G3;
			float X0 = i - t; // Unskew the cell origin back to (x,y,z) space
			float Y0 = j - t;
			float Z0 = k - t;
			float x0 = xin - X0; // The x,y,z distances from the cell origin
			float y0 = yin - Y0;
			float z0 = zin - Z0;
			// For the 3D case, the simplex shape is a slightly irregular tetrahedron.
			// Determine which simplex we are in.
			int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
			int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
			if (x0 >= y0)
			{
				if (y0 >= z0)
				{ i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
				else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
				else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
			}
			else
			{ // x0<y0
				if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
				else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
				else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
			}
			// A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
			// a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
			// a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
			// c = 1/6.
			float x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
			float y1 = y0 - j1 + G3;
			float z1 = z0 - k1 + G3;
			float x2 = x0 - i2 + 2.0f * G3; // Offsets for third corner in (x,y,z) coords
			float y2 = y0 - j2 + 2.0f * G3;
			float z2 = z0 - k2 + 2.0f * G3;
			float x3 = x0 - 1.0f + 3.0f * G3; // Offsets for last corner in (x,y,z) coords
			float y3 = y0 - 1.0f + 3.0f * G3;
			float z3 = z0 - 1.0f + 3.0f * G3;
			// Work out the hashed gradient indices of the four simplex corners
			int ii = i & 255;
			int jj = j & 255;
			int kk = k & 255;
			int gi0 = perm[ii + perm[jj + perm[kk]]] % 12;
			int gi1 = perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]] % 12;
			int gi2 = perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]] % 12;
			int gi3 = perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]] % 12;
			// Calculate the contribution from the four corners
			float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
			if (t0 < 0)
				n0 = 0.0f;
			else
			{
				t0 *= t0;
				n0 = t0 * t0 * dot(grad3[gi0], x0, y0, z0);
			}
			float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
			if (t1 < 0)
				n1 = 0.0f;
			else
			{
				t1 *= t1;
				n1 = t1 * t1 * dot(grad3[gi1], x1, y1, z1);
			}
			float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
			if (t2 < 0)
				n2 = 0.0f;
			else
			{
				t2 *= t2;
				n2 = t2 * t2 * dot(grad3[gi2], x2, y2, z2);
			}
			float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
			if (t3 < 0) n3
				= 0.0f;
			else
			{
				t3 *= t3;
				n3 = t3 * t3 * dot(grad3[gi3], x3, y3, z3);
			}

			// Check whether to return normalized [0,1]
			if (doNormalize)
				return Normalize(32.0f * (n0 + n1 + n2 + n3), -1, 1);

			// Add contributions from each corner to get the final noise value.
			// The result is scaled to stay just inside [-1,1]
			return 32.0f * (n0 + n1 + n2 + n3);
		}
		#endregion

		#region Random Helpers
		/// <summary>
		/// Normalizes a float to the range of 0 to 1
		/// </summary>
		/// <param name="value">The float to be normalized</param>
		/// <param name="min">The minimum value in the range</param>
		/// <param name="max">The maximum value in the range</param>
		/// <returns>The normalized float</returns>
		public static float Normalize(float value, float min, float max)
		{
			return (value - min) / (Math.Abs(min) + Math.Abs(max));
		}

		/// <summary>
		/// Normalizes a float to the range of -1 to 1
		/// </summary>
		/// <param name="value">The float to be normalized</param>
		/// <param name="min">The minimum value in the range</param>
		/// <param name="max">The maximum value in the range</param>
		/// <returns>The normalized float</returns>
		public static float NormalizeSigned(float value, float min, float max)
		{
			return value / ((Math.Abs(min) + Math.Abs(max)) * 0.5f);
		}
		#endregion

		#region Private Helper Functions
		private static int _fastfloor(float n)
		{
			return n > 0 ? (int)n : (int)n - 1;
		}
		#endregion

		#region Gustavson Private Helper Functions
		private static void _recalculatePermutations()
		{
			Random rand;
			if (_seed != -1)
				rand = new Random(_seed);
			else
				rand = new Random();

			// Randomize permutations array
			for (int i = 0; i < p.Length; i++)
				p[i] = rand.Next(_halfLength);

			for (int i = 0; i < 512; i++)
				perm[i] = p[i & 255];
		}

		private static float dot(int[] g, float x, float y)
		{
			return g[0] * x + g[1] * y;
		}
		private static float dot(int[] g, float x, float y, float z)
		{
			return g[0] * x + g[1] * y + g[2] * z;
		}
		private static float dot(int[] g, float x, float y, float z, float w)
		{
			return g[0] * x + g[1] * y + g[2] * z + g[3] * w;
		}
		#endregion

		#region Carmody Private Helper Functions
		private static void _reseed()
		{
			Random rand;
			if (_seed != -1)
				rand = new Random(_seed);
			else
				rand = new Random();

			T = new int[] {
				rand.Next(int.MinValue, int.MaxValue),
				rand.Next(int.MinValue, int.MaxValue),
				rand.Next(int.MinValue, int.MaxValue),
				rand.Next(int.MinValue, int.MaxValue),
				rand.Next(int.MinValue, int.MaxValue),
				rand.Next(int.MinValue, int.MaxValue),
				rand.Next(int.MinValue, int.MaxValue),
				rand.Next(int.MinValue, int.MaxValue)
			};
		}

		private static float _k(int a)
		{
			s = (A[0] + A[1] + A[2]) * onesixth;
			float x = u - A[0] + s;
			float y = v - A[1] + s;
			float z = w - A[2] + s;
			float t = 0.6f - x * x - y * y - z * z;
			int h = shuffle(i + A[0], j + A[1], k + A[2]);
			A[a]++;
			if (t < 0) return 0;
			int b5 = h >> 5 & 1;
			int b4 = h >> 4 & 1;
			int b3 = h >> 3 & 1;
			int b2 = h >> 2 & 1;
			int b = h & 3;
			float p = b == 1 ? x : b == 2 ? y : z;
			float q = b == 1 ? y : b == 2 ? z : x;
			float r = b == 1 ? z : b == 2 ? x : y;
			p = b5 == b3 ? -p : p;
			q = b5 == b4 ? -q : q;
			r = b5 != (b4 ^ b3) ? -r : r;
			t *= t;
			return 8 * t * t * (p + (b == 0 ? q + r : b2 == 0 ? q : r));
		}

		private static int shuffle(int i, int j, int k)
		{
			return b(i, j, k, 0) + b(j, k, i, 1) + b(k, i, j, 2) + b(i, j, k, 3) +
				   b(j, k, i, 4) + b(k, i, j, 5) + b(i, j, k, 6) + b(j, k, i, 7);
		}

		private static int b(int i, int j, int k, int B)
		{
			return T[b(i, B) << 2 | b(j, B) << 1 | b(k, B)];
		}

		private static int b(int N, int B)
		{
			return N >> B & 1;
		}
		#endregion
	}
}
