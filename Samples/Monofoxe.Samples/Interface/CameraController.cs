using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Samples.Interface
{
	/// <summary>
	/// Controls the camera.
	/// </summary>
	public class CameraController : Entity
	{
		Camera Camera;

		float _cameraSpeed = 400;

		float _minZoom = 0.1f;
		float _maxZoom = 30f;

		float _zoomSpeed = 1;

		float _rotationSpeed = 120;

		public const Buttons UpButton = Buttons.Up;
		public const Buttons DownButton = Buttons.Down;
		public const Buttons LeftButton = Buttons.Left;
		public const Buttons RightButton = Buttons.Right;
		public const Buttons ZoomInButton = Buttons.Z;
		public const Buttons ZoomOutButton = Buttons.X;
		public const Buttons RotateRightButton = Buttons.V;
		public const Buttons RotateLeftButton = Buttons.C;


		public CameraController(Layer layer, Camera camera) : base(layer)
		{
			Camera = camera;
			Camera.Offset = Camera.Size.ToVector3() / 2;
			Reset();
		}

		public override void Update()
		{
			base.Update();

			// Movement.
			var movementVector3 = new Vector3(
				Input.CheckButton(LeftButton).ToInt() - Input.CheckButton(RightButton).ToInt(),
				Input.CheckButton(UpButton).ToInt() - Input.CheckButton(DownButton).ToInt(),
				0
			);
			movementVector3 = Vector3.Transform(
				movementVector3, 
				Matrix.CreateRotationZ(Camera.Rotation.RadiansF)
			); // Rotating by the camera's rotation, so camera will always move relatively to screen. 
			
			var rotatedMovementVector = new Vector2(movementVector3.X, movementVector3.Y);
			
			Camera.Position += TimeKeeper.Global.Time(_cameraSpeed / Camera.Zoom) * rotatedMovementVector.ToVector3();
			// Movement.

			// Zoom.
			var zoomDirection = Input.CheckButton(ZoomInButton).ToInt() - Input.CheckButton(ZoomOutButton).ToInt();
			Camera.Zoom += TimeKeeper.Global.Time(_zoomSpeed) * zoomDirection;
			
			if (Camera.Zoom < _minZoom)
			{
				Camera.Zoom = _minZoom;
			}
			if (Camera.Zoom > _maxZoom)
			{
				Camera.Zoom = _maxZoom;
			}
			// Zoom.

			// Rotation.
			var rotationDirection = Input.CheckButton(RotateLeftButton).ToInt() - Input.CheckButton(RotateRightButton).ToInt();
			Camera.Rotation += TimeKeeper.Global.Time(_rotationSpeed) * rotationDirection;
			// Rotation.
			
		}

		public void Reset()
		{
			Camera.Zoom = 1;
			Camera.Rotation = Angle.Right;
			Camera.Position = Camera.Offset;
		}

	}
}
