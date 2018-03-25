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
		public float X = 0, 
		             Y = 0;

		/// <summary>
		/// View width.
		/// NOTE: To change view size, call Resize function.
		/// </summary>
		public int W 
		{
			get
			{
				return ViewSurface.Width;
			}
		}
		
		/// <summary>
		/// View height.
		/// NOTE: To change view size, call Resize function.
		/// </summary>
		public int H
		{
			get
			{
				return ViewSurface.Height;
			}
		}

		/// <summary>
		/// Camera offset.
		/// </summary>
		public int OffsetX = 0, 
		           OffsetY = 0;

		/// <summary>
		/// Viewport coordinates. Sets where on screen view will be drawn.
		/// </summary>
		public int PortX = 0, 
		           PortY = 0;
		
		/// <summary>
		/// View rotation. Measured in degrees.
		/// </summary>
		public int Rotation = 0;

		/// <summary>
		/// View scale.
		/// </summary>
		public float ScaleX = 1,
		             ScaleY = 1;

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
			ViewSurface = new RenderTarget2D(DrawCntrl.Device, w, h, false,
                                           DrawCntrl.Device.PresentationParameters.BackBufferFormat,
                                           DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
			DrawCntrl.Cameras.Add(this);
		}

		
		public void Resize(int w, int h)
		{
			ViewSurface.Dispose();
			ViewSurface = new RenderTarget2D(DrawCntrl.Device, w, h, false,
                                           DrawCntrl.Device.PresentationParameters.BackBufferFormat,
                                           DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
		}


		public void Destroy()
		{
			DrawCntrl.Cameras.Remove(this);
			ViewSurface.Dispose();
		}
		
		public void UpdateTransformMatrix()
		{
			TransformMatrix = Matrix.CreateTranslation(new Vector3(-X, -Y, 0)) *          // Coordinates.
		                    Matrix.CreateRotationZ(MathHelper.ToRadians(-Rotation)) *   // Rotation.
		                    Matrix.CreateScale(new Vector3(ScaleX, ScaleY, 1)) *	      // Scale.
		                    Matrix.CreateTranslation(new Vector3(OffsetX, OffsetY, 0)); // Offset.									
		}

	}
}
