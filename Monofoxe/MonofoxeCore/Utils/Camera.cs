using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;

namespace Monofoxe.Utils
{

	/// <summary>
	/// Game cameras. Support positioning, rotating and scaling.
	/// NOTE: There always should be at least one camera, 
	/// otherwise Draw events won't be triggered.
	/// </summary>
	public class Camera
	{

		/// <summary>
		/// View coordinates.
		/// NOTE: They don't take in account offset and rotation.
		/// </summary>
		public Vector2 Pos;

		/// <summary>
		/// View size.
		/// </summary>
		public Vector2 Size 
		{
			get =>	new Vector2(Surface.Width, Surface.Height);
			set
			{
				Surface.Dispose();
				Surface = new RenderTarget2D(
					DrawMgr.Device, 
					(int)value.X, 
					(int)value.Y, 
					false,
					DrawMgr.Device.PresentationParameters.BackBufferFormat,
					DepthFormat.Depth24, 
					0, 
					RenderTargetUsage.PreserveContents
				);
			}
		}

		/// <summary>
		/// Camera offset.
		/// </summary>
		public Vector2 Offset;

		/// <summary>
		/// View rotation. Measured in degrees.
		/// </summary>
		public int Rotation;

		/// <summary>
		/// View zoom.
		/// </summary>
		public float Zoom = 1;


		/// <summary>
		/// Viewport coordinates. Sets where on screen view will be drawn.
		/// </summary>
		public Vector2 PortPos;
		
		/// <summary>
		/// Viewport coordinates. Sets where on screen view will be drawn.
		/// </summary>
		public Vector2 PortOffset;

		/// <summary>
		/// Viewport scale.
		/// </summary>
		public float PortScale = 1;
		
		/// <summary>
		/// Viewport rotation.
		/// </summary>
		public float PortRotation = 0;
		


		/// <summary>
		/// Camera surface. Everything will be drawn on it.
		/// </summary>
		public RenderTarget2D Surface;

		/// <summary>
		/// Background color for a view surface.
		/// NOTE: It is not recommended to set transparent colors.
		/// </summary>
		public Color BackgroundColor = Color.Azure;

		/// <summary>
		/// If true, camera surface will be drawn automatically.
		/// </summary>
		public bool Visible = true;

		/// <summary>
		/// If false, camera won't trigger any Draw events.
		/// </summary>
		public bool Enabled = true;

		/// <summary>
		/// If true, clears camera surface every step.
		/// </summary>
		public bool ClearBackground = true;

		/// <summary>
		/// Transformation matrix.
		/// </summary>
		public Matrix TransformMatrix;


		public Camera(int w, int h)
		{
			Surface = new RenderTarget2D(
				DrawMgr.Device, 
				w, 
				h, 
				false,
				DrawMgr.Device.PresentationParameters.BackBufferFormat,
				DrawMgr.Device.PresentationParameters.DepthStencilFormat, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			DrawMgr.Cameras.Add(this);
			
		}

		/// <summary>
		/// Removes camera from draw controller list and disposes surface.
		/// </summary>
		public void Destroy()
		{
			DrawMgr.Cameras.Remove(this);
			Surface.Dispose();
		}
		
		public void UpdateTransformMatrix()
		{
			TransformMatrix = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) * // Coordinates.
				Matrix.CreateRotationZ(MathHelper.ToRadians(-Rotation)) *                  // Rotation.
				Matrix.CreateScale(Vector3.One * Zoom) *                                   // Scale.
				Matrix.CreateTranslation(new Vector3(Offset.X, Offset.Y, 0));              // Offset.									
		}

	}
}
