using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using System;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Cameras
{

	/// <summary>
	/// Game camera. Supports positioning, rotating and scaling.
	/// NOTE: There always should be at least one camera, 
	/// otherwise Draw events won't be triggered.
	/// </summary>
	public abstract class Camera : IDisposable
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
		public Vector3 Position;

		/// <summary>
		/// View size.
		/// </summary>
		public Vector2 Size => new Vector2(Surface.Width, Surface.Height);
		
		/// <summary>
		/// Camera offset.
		/// </summary>
		public Vector3 Offset;

		/// <summary>
		/// View rotation. Measured in degrees.
		/// </summary>
		public Angle Rotation;

		/// <summary>
		/// View zoom.
		/// </summary>
		public float Zoom = 1;

		/// <summary>
		/// If depth buffer is enabled, vertices with Z further than far plane will not be drawn.
		/// </summary>
		public float ZFarPlane = 1;

		/// <summary>
		/// If depth buffer is enabled, vertices with Z closer than near plane will not be drawn.
		/// </summary>
		public float ZNearPlane = 0;


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
		

		public Matrix View;
		public Matrix Projection;
		
		/// <summary>
		/// For a layer or a scene to be rendered, it has to match at least one bit of RenderMask.
		/// </summary>
		public RenderMask RenderMask = RenderMask.Default;

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

		public event Action<int, int> OnResize;

		/// <summary>
		/// Resizes the view.
		/// </summary>
		public void Resize(int w, int h)
		{
			Surface.Resize(w, h);
			_postprocessorBuffer?.Resize(w, h);
			_postprocessorLayerBuffer?.Resize(w, h);
			OnResize?.Invoke(w, h);
		}

		/// <summary>
		/// Removes camera from draw controller list and disposes the surface.
		/// </summary>
		public void Dispose()
		{
			CameraMgr.RemoveCamera(this);
			PostprocessingMode = PostprocessingMode.None;
			Surface.Dispose();
		}
		
		
		public abstract Matrix ConstructViewMatrix();
		
		public abstract Matrix ConstructProjectionMatrix();
			


		/// <summary>
		/// Returns mouse position relative to the camera.
		/// </summary>
		public abstract Vector2 GetRelativeMousePosition();


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
						GraphicsMgr.Device.Clear(Color.Transparent);
						_postprocessorBuffer.Draw(Vector2.Zero);
					}
					else
					{
						Surface.SetTarget(_postprocessorBuffer);
						GraphicsMgr.Device.Clear(Color.Transparent);
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
			var oldEffect = GraphicsMgr.VertexBatch.Effect;

			ApplyPostprocessing();
			
			Surface.Draw(
				PortPosition, 
				PortOffset, 
				Vector2.One * PortScale,
				PortRotation, 
				Color.White
			);
			GraphicsMgr.VertexBatch.Effect = oldEffect;
		}

	}
}
