using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using System.Diagnostics;

namespace Monofoxe.Engine
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
		/// NOTE: To change view size, call Resize function.
		/// </summary>
		public Vector2 Size => new Vector2(ViewSurface.Width, ViewSurface.Height);
		
		/// <summary>
		/// Camera offset.
		/// </summary>
		public Vector2 Offset;

		/// <summary>
		/// Viewport coordinates. Sets where on screen view will be drawn.
		/// </summary>
		public Vector2 PortPos;
		
		/// <summary>
		/// View rotation. Measured in degrees.
		/// </summary>
		public int Rotation;

		/// <summary>
		/// View zoom.
		/// </summary>
		public float Zoom = 1;

		/// <summary>
		/// View surface. Everything will be drawn on it.
		/// </summary>
		public RenderTarget2D ViewSurface;

		/// <summary>
		/// Background color for a view surface.
		/// NOTE: It is not recommended to set transparent colors.
		/// </summary>
		public Color BackgroundColor = Color.Azure;

		/// <summary>
		/// If true, camera surface will be drawn automatically.
		/// </summary>
		public bool Autodraw = true;

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
			ViewSurface = new RenderTarget2D(
				DrawCntrl.Device, 
				w, 
				h, 
				false,
				DrawCntrl.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			DrawCntrl.Cameras.Add(this);
		}

		/// <summary>
		/// Resizes camera.
		/// </summary>
		public void Resize(int w, int h)
		{
			ViewSurface.Dispose();
			ViewSurface = new RenderTarget2D(
				DrawCntrl.Device, 
				w, 
				h, 
				false,
				DrawCntrl.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24, 
				0, 
				RenderTargetUsage.PreserveContents
			);
		}

		/// <summary>
		/// Removes camera from draw controller list and disposes surface.
		/// </summary>
		public void Destroy()
		{
			DrawCntrl.Cameras.Remove(this);
			ViewSurface.Dispose();
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
