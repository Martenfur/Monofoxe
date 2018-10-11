using System.Collections.Generic;
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
		public RenderTarget2D Surface {get; private set;}

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



		private Dictionary<string, HashSet<string>> _filter = new Dictionary<string, HashSet<string>>();

		public FilterType FilterType = FilterType.None;


		public List<Effect> PostprocessorEffects {get; private set;} = new List<Effect>();

		public bool PostprocessingEnabled
		{
			get => _postprocessingEnabled;

			set
			{
				if (_postprocessingEnabled != value)
				{
					_postprocessingEnabled = value;
					if (value)
					{
						for(var i = 0; i < _postprocessorBuffers.Length; i += 1)
						{
							_postprocessorBuffers[i] = Surface = new RenderTarget2D(
								DrawMgr.Device, 
								Surface.Width, 
								Surface.Height, 
								false,
								DrawMgr.Device.PresentationParameters.BackBufferFormat,
								DrawMgr.Device.PresentationParameters.DepthStencilFormat, 
								0, 
								RenderTargetUsage.PreserveContents
							);
						}
					}
					else
					{
						for(var i = 0; i < _postprocessorBuffers.Length; i += 1)
						{
							_postprocessorBuffers[i].Dispose();
							_postprocessorBuffers[i] = null;
						}
					}
				}
			}
		}

		private bool _postprocessingEnabled = false;

		internal RenderTarget2D[] _postprocessorBuffers = new RenderTarget2D[2];


		public Camera(int w, int h, int priority = 0)
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
			
			Priority = priority; // Also adds camera to camera list.
		}

		/// <summary>
		/// Removes camera from draw controller list and disposes surface.
		/// </summary>
		public void Destroy()
		{
			CameraMgr.RemoveCamera(this);
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


		public void ApplyPostprocessing()
		{
			if (PostprocessingEnabled)
			{
				var sufraceChooser = false;//((PostprocessorEffects.Count % 2) == 0);
				
				foreach(var effect in PostprocessorEffects)
				{
					DrawMgr.Effect = effect;
					if (sufraceChooser)
					{
						DrawMgr.SetSurfaceTarget(Surface);
						DrawMgr.Device.Clear(Color.TransparentBlack);
						DrawMgr.DrawSurface(_postprocessorBuffers[0], Vector2.Zero);
					}
					else
					{
						DrawMgr.SetSurfaceTarget(_postprocessorBuffers[0]);
						DrawMgr.Device.Clear(Color.TransparentBlack);
						DrawMgr.DrawSurface(Surface, Vector2.Zero);
					}
					
					DrawMgr.ResetSurfaceTarget();
					DrawMgr.Effect = null;
					sufraceChooser = !sufraceChooser;
				}

				if ((PostprocessorEffects.Count % 2) != 0)
				{
					DrawMgr.SetSurfaceTarget(Surface);
					DrawMgr.Device.Clear(Color.TransparentBlack);
					DrawMgr.DrawSurface(_postprocessorBuffers[0], Vector2.Zero);
					
					DrawMgr.ResetSurfaceTarget();
				}
			}
		}

	}
}
