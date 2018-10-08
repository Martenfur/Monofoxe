using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using System.Collections.Generic;

namespace Monofoxe.Utils
{

	/// <summary>
	/// Type of filtering for camera.
	/// </summary>
	public enum FilterType
	{
		/// <summary>
		/// Triggers rendering, if filter DOES contain layer.
		/// </summary>
		Inclusive,
	
		/// <summary>
		/// Triggers rendering, if filter DOES NOT contain layer.
		/// </summary>
		Exclusive,

		/// <summary>
		/// Renders all layers.
		/// </summary>
		None,
	}


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
		/// NOTE: Changing it will break mouse position!
		/// </summary>
		public float PortRotation = 0;
		


		/// <summary>
		/// Camera surface. Everything will be drawn on it.
		/// </summary>
		public RenderTarget2D Surface;

		/// <summary>
		/// Background color for a view surface.
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



		private Dictionary<string, HashSet<string>> _filter = new Dictionary<string, HashSet<string>>();

		public FilterType FilterType = FilterType.None;


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


		/// <summary>
		/// Returns mouse position relative to the camera.
		/// </summary>
		public Vector2 GetRelativeMousePosition()
		{
			/*
			 * Well, I am giving up.
			 * Mouse *works* with port position, offset and scale,
			 * but rotation breaks everything for some reason.
			 * Maybe a hero of the future will fix it, but this is 
			 * so rare usecase, that it doesn't really worth the hassle. :S
			 * TODO: Fix port rotation problems.
			 */
			var transformMatrix = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
				Matrix.CreateRotationZ(MathHelper.ToRadians(PortRotation - Rotation)) *            
				Matrix.CreateScale(Vector3.One * Zoom) *
				Matrix.CreateTranslation(new Vector3(Offset.X, Offset.Y, 0));
			
			var matrix =  Matrix.Invert(transformMatrix);
			var mouseVec = (Input.ScreenMousePos - PortPos) / PortScale + PortOffset;

			var transformedMouseVec = Vector3.Transform(new Vector3(mouseVec.X, mouseVec.Y, 0), matrix);
			return new Vector2(transformedMouseVec.X, transformedMouseVec.Y);
		}


		public void AddFilterEntry(string sceneName, string layerName)
		{
			if (_filter.ContainsKey(sceneName))
			{
				_filter[sceneName].Add(layerName);
			}
			else
			{
				var newSet = new HashSet<string>();
				newSet.Add(layerName);
				_filter.Add(sceneName, newSet);
			}
		}

		public void RemoveFilterEntry(string sceneName, string layerName)
		{
			if (_filter.ContainsKey(sceneName))
			{
				_filter[sceneName].Remove(layerName);
				if (_filter[sceneName].Count == 0)
				{
					_filter.Remove(sceneName);
				}
			}
		}

		/// <summary>
		/// Returns true, if given layer is filtered out.
		/// </summary>
		public bool Filter(string sceneName, string layerName)
		{
			if (FilterType == FilterType.None)
			{
				return false;
			}

			var result = false;

			if (_filter.ContainsKey(sceneName))
			{
				result = _filter[sceneName].Contains(layerName);
			}

			if (FilterType == FilterType.Inclusive)
			{
				return !result;
			}
			else
			{
				// NOTE: Add additional check here, if any other filter types will be created.
				return result;
			}

		}

	}
}
