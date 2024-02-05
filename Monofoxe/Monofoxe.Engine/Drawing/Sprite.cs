using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;
using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable sprite. 
	/// </summary>
	public class Sprite : ICloneable
	{
		public Vector2 Position;

		public string Name { get; internal set; }

		/// <summary>
		/// An array of sprite's frames.
		/// </summary>
		private Frame[] _frames;

		/// <summary>
		/// Sprite width. Can be accessed only if all sprite frames have the same size.
		/// </summary>
		public float Width 
		{
			get
			{
				if (SingleFrameSize)
				{
					return _frames[0].Width;
				}	
				throw new Exception("To use this variable, all frame sizes must be identical!");
			}
		}
		
		/// <summary>
		/// Sprite height. Can be accessed only if all sprite frames have the same size.
		/// </summary>
		public float Height
		{
			get
			{
				if (SingleFrameSize)
				{
					return _frames[0].Height;
				}
				throw new Exception("To use this variable, all frame sizes must be identical!");
			}
		}


		public Vector2 Scale = Vector2.One;

		public Vector2 Origin;

		public Angle Rotation;

		public double Animation;

		public Color Color = Color.White;

		public Vector4 ZDepth = Vector4.Zero;

		/// <summary>
		/// True, if size of every frame is identical.
		/// </summary>
		public bool SingleFrameSize;


		/// <summary>
		/// Amount of sprite's frames.
		/// </summary>
		public int FramesCount => _frames.Length;

		/// <summary>
		/// Returns a frame with given index.
		/// </summary>
		public Frame this[int id]
		{
			get => _frames[id];
		}


		public Sprite(Frame[] frames, int originX, int originY, string name = "")
		{
			_frames = new Frame[frames.Length];
			foreach(var frame in frames)
			{
				frame.ParentSprite = this;
			}
			Array.Copy(frames, _frames, frames.Length);
			Origin = new Vector2(originX, originY);
			
			SingleFrameSize = CheckIdenticalFrameSizes();
			Name = name;
		}

		public Sprite(Frame[] frames, Vector2 origin, string name = "")
		{
			_frames = new Frame[frames.Length];
			foreach(var frame in frames)
			{
				frame.ParentSprite = this;
			}
			Array.Copy(frames, _frames, frames.Length);
			Origin = origin;
			SingleFrameSize = CheckIdenticalFrameSizes();
			Name = name;
		}

		public Sprite(Frame frame, int originX, int originY, string name = "")
		{
			_frames = new Frame[]{frame};
			frame.ParentSprite = this;
			Origin = new Vector2(originX, originY);
			
			SingleFrameSize = true;
			Name = name;
		}
		

		/// <summary>
		/// Checks if all frames got identical sizes.
		/// </summary>
		private bool CheckIdenticalFrameSizes()
		{
			for(var i = 1; i < _frames.Length; i += 1)
			{
				if (_frames[0].Width != _frames[i].Width || _frames[0].Height != _frames[i].Height)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns sprite frame based on an animation value from 0 to 1.
		/// </summary>
		private Frame GetFrame(double animation) =>
			_frames[Math.Max(0, Math.Min(_frames.Length - 1, (int)(animation * _frames.Length)))];
		
		
		public void Draw() =>
			GetFrame(Animation).Draw(Position, Origin, Scale, Rotation, Color, ZDepth);
		
		// Vectors.

		public void Draw(Vector2 position) =>
			_frames[0].Draw(position, Origin, Scale, Rotation, Color, ZDepth);

		public void Draw(Vector2 position, double animation) =>
			GetFrame(animation).Draw(position, Origin, Scale, Rotation, Color, ZDepth);
		
		public void Draw(Vector2 position, double animation, Vector2 origin, Vector2 scale, Angle rotation, Color color) =>
			GetFrame(animation).Draw(position, origin, scale, rotation, color, ZDepth);

		public void Draw(
			Vector2 position, 
			double animation, 
			Vector2 origin, 
			Vector2 scale, 
			Angle rotation, 
			Color color, 
			Vector4 zDepth
		) =>
			GetFrame(animation).Draw(position, origin, scale, rotation, color, zDepth);

		// Vectors.


		// Rectangles.

		public void Draw(RectangleF destRect, double animation) =>
			GetFrame(animation).Draw(destRect, Rotation, Color);

		public void Draw(RectangleF destRect, double animation, Angle rotation, Color color) =>
			GetFrame(animation).Draw(destRect, rotation, color);
			
		public void Draw(RectangleF destRect, double animation, RectangleF srcRect) => 
			GetFrame(animation).Draw(destRect, srcRect, Rotation, Color, ZDepth);

		public void Draw(
			RectangleF destRect, 
			double animation, 
			RectangleF srcRect, 
			Angle rotation, 
			Color color
		) =>
			GetFrame(animation).Draw(destRect, srcRect, rotation, color, ZDepth);
		
		public void Draw(
			RectangleF destRect, 
			double animation, 
			RectangleF srcRect, 
			Angle rotation, 
			Color color, 
			Vector4 zDepth
		) =>
			GetFrame(animation).Draw(destRect, srcRect, rotation, color, zDepth);

		// Rectangles.


		public object Clone()
		{
			var frames = new List<Frame>();

			foreach(var frame in _frames)
			{
				frames.Add((Frame)frame.Clone());
			}

			var sprite = new Sprite(frames.ToArray(), Origin, Name);
			sprite.Position = Position;
			sprite.Scale = Scale;
			sprite.Animation = Animation;
			sprite.Rotation = Rotation;
			sprite.Color = Color;
			return sprite;
		}

	}
}
