using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

// DO NOT reference FMOD in ANY of your classes.
// Use FMOD.SomeClass instead.
// FMOD classes seriously interfere with System.

namespace Monofoxe.Engine.Audio
{
	public class Sound
	{
		private FMOD.System _FMODSystem;

		public FMOD.Sound FMODSound {get; private set;}
		public FMOD.Channel Channel
		{
			get;
			private set;
		}
		private FMOD.Channel _channel; // Can't use out on properties. 

		#region Properties.

		public int Loops
		{
			get
			{
				var loops = 0;
				SetLastResult(_channel?.getLoopCount(out loops));
				return loops;
			}
			set => SetLastResult(_channel?.setLoopCount(value));
		}

		public float Pitch
		{
			get
			{
				var pitch = 1f;
				SetLastResult(_channel?.getPitch(out pitch));
				return pitch;
			}
			set => SetLastResult(_channel?.setPitch(value));
		}

		public float Volume
		{
			get
			{
				var volume = 1f;
				SetLastResult(_channel?.getVolume(out volume));
				return volume;
			}
			set => SetLastResult(_channel?.setVolume(value));	
		}

		public float LowPass
		{
			get
			{
				var lowPassGain = 1f;
				SetLastResult(_channel?.getLowPassGain(out lowPassGain));
				return lowPassGain;
			}
			set => SetLastResult(_channel?.setLowPassGain(value));
		}

		public bool IsPlaying
		{
			get
			{
				var isPlaying = false;
				SetLastResult(_channel?.isPlaying(out isPlaying));
				return isPlaying;
			}
		}

		public FMOD.MODE Mode
		{
			get
			{
				var mode = FMOD.MODE.DEFAULT;
				SetLastResult(_channel?.getMode(out mode));
				return mode;
			}
			set => SetLastResult(_channel?.setMode(value));
		}

		#endregion Properties.



		public Sound(FMOD.System system, FMOD.Sound sound, FMOD.Channel channel = null)
		{
			_FMODSystem = system;
			FMODSound = sound;
			_channel = channel;
		}


		public void Play(FMOD.ChannelGroup group = null, bool paused = false) =>
			SetLastResult(_FMODSystem.playSound(FMODSound, group, paused, out _channel));
		
		public void Pause() =>
			SetLastResult(_channel?.setPaused(true));

		public void Resume() =>
			SetLastResult(_channel?.setPaused(false));

		public void Stop()
		{
			SetLastResult(_channel?.stop());
			_channel = null;
		}

		public uint GetTrackPosition(FMOD.TIMEUNIT timeUnit = FMOD.TIMEUNIT.MS)
		{
			uint position = 0;
			SetLastResult(_channel?.getPosition(out position, timeUnit));
			return position;
		}

		public void SetTrackPosition(uint position, FMOD.TIMEUNIT timeUnit = FMOD.TIMEUNIT.MS) =>
			SetLastResult(_channel?.setPosition(position, timeUnit));
		
		
		//TODO: Add Vector3 functions, if you will need them.

		public void PlayAt(
			FMOD.ChannelGroup group = null, 
			bool paused = false, 
			Vector2 pos = default(Vector2), 
			Vector2 velocity = default(Vector2)
		)
		{
			FMODSound.setMode(FMOD.MODE._3D);
			Set3DAttributes(pos, velocity);
			SetLastResult(_FMODSystem.playSound(FMODSound, group, paused, out _channel));
		}

		public void Set3DAttributes(Vector2 pos, Vector2 velocity)
		{
			var fmodPos = AudioMgr.Vector2ToFmodVector(pos);
			var fmodVelocity = AudioMgr.Vector2ToFmodVector(velocity);
			var zeroVec = AudioMgr.GetFmodVector();
			SetLastResult(_channel?.set3DAttributes(ref fmodPos, ref fmodVelocity, ref zeroVec));
		}

		public void Set3DAttributes(Vector2 pos, Vector2 velocity, Vector2 altPanPos)
		{
			var fmodPos = AudioMgr.Vector2ToFmodVector(pos);
			var fmodVelocity = AudioMgr.Vector2ToFmodVector(velocity);
			var fmodAltPanPos = AudioMgr.Vector2ToFmodVector(altPanPos);
			SetLastResult(_channel?.set3DAttributes(ref fmodPos, ref fmodVelocity, ref fmodAltPanPos));
		}



		public void Set3DMinMaxDistance(float minDistance, float maxDistance) =>
			SetLastResult(_channel?.set3DMinMaxDistance(minDistance, maxDistance));

		public Tuple<float, float> Get3DMinMaxDistance()
		{
			float minDistance = 0, 
				maxDistance = 0;
			SetLastResult(_channel?.get3DMinMaxDistance(out minDistance, out maxDistance));
			return new Tuple<float, float>(minDistance, maxDistance);
		}
		


		/// <summary>
		/// Sets last result to the Audio Manager.
		/// NOTE: There is very high probability that this code will change, so 
		/// making it separate function will make life a bit easier.
		/// </summary>
		private void SetLastResult(FMOD.RESULT? result)
		{
			if (result != null)
			{
				AudioMgr.LastResult = (FMOD.RESULT)result;
			}
		}

	}
}
