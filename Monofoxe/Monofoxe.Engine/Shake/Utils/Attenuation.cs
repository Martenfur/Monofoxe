/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Shake.Utils
{
	public struct Attenuation
	{
		/// <summary>
		/// Radius in which shake doesn't lose strength.
		/// </summary>
		public float ClippingDistance;

		/// <summary>
		/// Defines how fast strength falls with distance.
		/// </summary>
		public float FalloffScale;

		/// <summary>
		/// Power of the falloff function.
		/// </summary>
		public Degree FalloffDegree;


		public Attenuation(float clippingDistance = 100, float falloffScale = 128, Degree falloffDegree = Degree.Quadratic)
		{
			ClippingDistance = clippingDistance;
			FalloffScale = falloffScale;
			FalloffDegree = falloffDegree;
		}


		/// <summary>
		/// Returns multiplier for the strength of a shake, based on source and camera positions.
		/// </summary>
		public float Attenuate(Vector2 sourcePosition, Vector2 cameraPosition)
		{
			Vector2 vec = cameraPosition - sourcePosition;
			float distance = vec.Length();
			float strength = MathHelper.Clamp(1 - (distance - ClippingDistance) / FalloffScale, 0, 1);

			return Power.Evaluate(strength, FalloffDegree);
		}
	}
}
