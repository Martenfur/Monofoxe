using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Cameras
{
	public class Camera2D : Camera
	{
	
		public Vector2 Position2D
		{
			get => Position.ToVector2();
			set
			{
				Position.X = value.X;
				Position.Y = value.Y;
			}
		}


		public Camera2D(int w, int h, int priority = 0) : base(w, h, priority)
		{
		}


		public override Matrix ConstructViewMatrix()
		{
			return Matrix.CreateTranslation(-Position) *       // Position.
				Matrix.CreateRotationZ(-Rotation.RadiansF) *     // Rotation.
				Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * // Scale.
				Matrix.CreateTranslation(Offset);                // Offset.									
		}

		public override Matrix ConstructProjectionMatrix() =>
			Matrix.CreateOrthographicOffCenter(0, Size.X, Size.Y, 0, ZNearPlane, ZFarPlane);

			
		/// <summary>
		/// Returns mouse position relative to the camera.
		/// </summary>
		public override Vector2 GetRelativeMousePosition()
		{
			/*
			 * Well, I am giving up.
			 * Mouse *works* with port position, offset and scale,
			 * but rotation breaks everything for some reason.
			 * Maybe a hero of the future will fix it, but this is 
			 * so rare usecase, that it doesn't really worth the hassle. :S
			 * TODO: Fix port rotation problems.
			 */
			var transformMatrix = Matrix.CreateTranslation(-Position) *
				Matrix.CreateRotationZ((PortRotation - Rotation).RadiansF) *
				Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
				Matrix.CreateTranslation(Offset);

			var matrix = Matrix.Invert(transformMatrix);
			var mouseVec = (Input.ScreenMousePosition - PortPosition) / PortScale + PortOffset;

			var transformedMouseVec = Vector3.Transform(new Vector3(mouseVec.X, mouseVec.Y, 0), matrix);
			return new Vector2(transformedMouseVec.X, transformedMouseVec.Y);
		}

	}
}
