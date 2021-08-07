using Microsoft.Xna.Framework;
using Monofoxe.Engine.EC;

namespace Monofoxe.Samples.ECDemo
{
	/// <summary>
	/// Basic position component. 
	/// </summary>
	public class PositionComponent : Component
	{

		/// <summary>
		/// Entity position on the scene.
		/// </summary>
		public Vector2 Position;
		
		/// <summary>
		/// Starting entity position on the scene.
		/// </summary>
		public Vector2 StartingPosition;

		public Vector2 PreviousPosition;


		public PositionComponent(Vector2 position)
		{
			Position = position;
			PreviousPosition = position;
			StartingPosition = position;
		}
	}
}
