using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using System;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Cameras
{

	/// <summary>
	/// Game camera. Support positioning, rotating and scaling.
	/// NOTE: There always should be at least one camera, 
	/// otherwise Draw events won't be triggered.
	/// </summary>
	public class Camera : IDisposable
	{
		/// <summary>
		/// Priority of a camera. 
		/// Higher priority = earlier drawing.
		/// </summary>
		public int Priority
		{
			get => _priority;

			set
			{
				_priority = value;
				CameraMgr.UpdateCameraPriority(this);
			}
		}
		private int _priority;


		/// <summary>
		/// View coordinates.
		/// NOTE: They don't take into account offset and rotation.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// View size.
		/// </summary>
		public Vector2 Size => new Vector2(Surface.Width, Surface.Height);
		
		/// <summary>
		/// Camera offset.
		/// </summary>
		public Vector2 Offset;

		/// <summary>
		/// View rotation. Measured in degrees.
		/// </summary>
		public Angle Rotation;

		/// <summary>
		/// View zoom.
		/// </summary>
		public float Zoom = 1;


		/// <summary>
		/// Viewport coordinates. Sets where on screen view will be drawn.
		/// </summary>
		public Vector2 PortPosition;
		
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
		public Angle PortRotation;
		


		/// <summary>
		/// Camera surface. Everything will be drawn on it.
		/// NOTE: This reference can change to another surface!
		/// </summary>
		public Surface Surface {get; private set;}

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
		public Matrix TransformMatrix {get; private set;}



		private Dictionary<string, HashSet<string>> _filter = 
			new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Mode for filtering out certain layers.
		/// </summary>
		public FilterMode FilterMode = FilterMode.None;


		/// <summary>
		/// Shaders applied to the surface.
		/// NOTE: Last shader will be applied directly during drawing of the surface.
		/// If you draw it yourself, don't forget to apply last shader.
		/// </summary>
		public List<Effect> PostprocessorEffects {get; private set;} = new List<Effect>();

		/// <summary>
		/// Enables usage of shaders on camera surface and layers.
		/// NOTE: Additional surfaces will be created.
		/// </summary>
		public PostprocessingMode PostprocessingMode
		{
			get => _postprocessingMode;

			set
			{
				if (_postprocessingMode != value)
				{
					_postprocessingMode = value;

					if (_postprocessingMode != PostprocessingMode.None)
					{
						_postprocessorBuffer = new Surface(Surface.Width, Surface.Height);
					}
					else
					{
						_postprocessorBuffer?.Dispose();
						_postprocessorBuffer = null;
					}

					if (_postprocessingMode == PostprocessingMode.CameraAndLayers)
					{
						_postprocessorLayerBuffer = new Surface(Surface.Width, Surface.Height);
					}
					else
					{
						_postprocessorLayerBuffer?.Dispose();
						_postprocessorLayerBuffer = null;
					}
				}
			}
		}

		private PostprocessingMode _postprocessingMode = PostprocessingMode.None;

		internal Surface _postprocessorBuffer;
		internal Surface _postprocessorLayerBuffer;


		public Camera(int w, int h, int priority = 0)
		{
			Surface = new Surface(w, h);
			
			Priority = priority; // Also adds camera to camera list.
		}

		/// <summary>
		/// Resizes the view.
		/// </summary>
		public void Resize(int w, int h) =>
			Surface.Resize(w, h);

		/// <summary>
		/// Removes camera from draw controller list and disposes the surface.
		/// </summary>
		public void Dispose()
		{
			CameraMgr.RemoveCamera(this);
			PostprocessingMode = PostprocessingMode.None;
			Surface.Dispose();
		}
		
		/// <summary>
		/// Updates camera's transform matrix.
		/// </summary>
		public void UpdateTransformMatrix()
		{
			TransformMatrix = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * // Coordinates.
				Matrix.CreateRotationZ(-Rotation.RadiansF) *                  // Rotation.
				Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *                                   // Scale.
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
			var transformMatrix = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
				Matrix.CreateRotationZ((PortRotation - Rotation).RadiansF) *            
				Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
				Matrix.CreateTranslation(new Vector3(Offset.X, Offset.Y, 0));
			
			var matrix =  Matrix.Invert(transformMatrix);
			var mouseVec = (Input.ScreenMousePosition - PortPosition) / PortScale + PortOffset;

			var transformedMouseVec = Vector3.Transform(new Vector3(mouseVec.X, mouseVec.Y, 0), matrix);
			return new Vector2(transformedMouseVec.X, transformedMouseVec.Y);
		}


		public void AddFilterEntry(string sceneName, string layerName)
		{
			if (_filter.TryGetValue(sceneName, out HashSet<string> filterSet))
			{
				filterSet.Add(layerName);
			}
			else
			{
				var newSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				newSet.Add(layerName);
				_filter.Add(sceneName, newSet);
			}
		}

		public void RemoveFilterEntry(string sceneName, string layerName)
		{
			if (_filter.TryGetValue(sceneName, out HashSet<string> filterSet))
			{
				filterSet.Remove(layerName);
				if (filterSet.Count == 0)
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
			if (FilterMode == FilterMode.None)
			{
				return false;
			}

			var result = false;
			
			// NOTE: Here was a strange TO-DO with just said "fix this".
			// Haven't found any bugs here, so just removed it. Probably
			// was an already fixed bug with case-sensitivity. In case of any 
			// camera filter-related bugs, this place if the first suspect.
			// Thanks, past me.

			if (_filter.TryGetValue(sceneName, out HashSet<string> filterSet))
			{
				result = filterSet.Contains(layerName);
			}

			if (FilterMode == FilterMode.Inclusive)
			{
				return !result;
			}
			else
			{
				// NOTE: Add additional check here, if any other filter types will be created.
				return result;
			}

		}



		/// <summary>
		/// Applies shaders to the camera surface.
		/// </summary>
		private void ApplyPostprocessing()
		{
			if (PostprocessingMode != PostprocessingMode.None && PostprocessorEffects.Count > 0)
			{
				var sufraceChooser = false;
				
				for(var i = 0; i < PostprocessorEffects.Count - 1; i += 1)
				{
					
					GraphicsMgr.VertexBatch.Effect = PostprocessorEffects[i];
					if (sufraceChooser)
					{
						Surface.SetTarget(Surface);
						GraphicsMgr.Device.Clear(Color.TransparentBlack);
						_postprocessorBuffer.Draw(Vector2.Zero);
					}
					else
					{
						Surface.SetTarget(_postprocessorBuffer);
						GraphicsMgr.Device.Clear(Color.TransparentBlack);
						Surface.Draw(Vector2.Zero);
					}
					
					Surface.ResetTarget();
					sufraceChooser = !sufraceChooser;
				}
				
				if ((PostprocessorEffects.Count % 2) == 0)
				{
					// Swapping surfaces.
					var buffer = Surface;
					Surface = _postprocessorBuffer;
					_postprocessorBuffer = buffer;
				}

				GraphicsMgr.VertexBatch.Effect = PostprocessorEffects[PostprocessorEffects.Count - 1];
			}
		}

		internal void Render()
		{
			ApplyPostprocessing();
			
			Surface.Draw(
				PortPosition, 
				PortOffset, 
				Vector2.One * PortScale,
				PortRotation, 
				Color.White
			);
			GraphicsMgr.VertexBatch.Effect = null;
		}

	}
}
