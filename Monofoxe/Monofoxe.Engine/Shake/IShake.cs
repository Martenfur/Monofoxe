/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Monofoxe.Engine.Shake.Utils;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Shake
{
	public interface IShake
	{
		/// <summary>
		/// Represents current position and rotation of the camera according to the shake.
		/// </summary>
		Displacement CurrentDisplacement { get; }

		/// <summary>
		/// Shake system will dispose the shake on the first frame when this is true.
		/// </summary>
		bool IsFinished { get; }

		/// <summary>
		/// Shaker calls this when the shake is added to the list of active shakes.
		/// </summary>
		void Initialize(Vector2 cameraPosition, float cameraRotation);

		/// <summary>
		/// Shaker calls this every frame on active shakes.
		/// </summary>
		void Update(TimeKeeper time, Vector2 cameraPosition, float cameraRotation);
	}
}
