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
			get => _channel;
			private set => _channel = value;
		}
		public FMOD.Channel _channel;

		#region Properties.

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

		#endregion Properties.



		public Sound(FMOD.System system, FMOD.Sound sound)
		{
			_FMODSystem = system;
			FMODSound = sound;
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

		public uint GetPosition(FMOD.TIMEUNIT timeUnit = FMOD.TIMEUNIT.MS)
		{
			uint position = 0;
			SetLastResult(_channel?.getPosition(out position, timeUnit));
			return position;
		}

		public void SetPosition(uint position, FMOD.TIMEUNIT timeUnit = FMOD.TIMEUNIT.MS) =>
			SetLastResult(_channel?.setPosition(position, timeUnit));

		public void Do3DStuff(Vector2 pos)
		{
			var fmodPos = new FMOD.VECTOR();
			fmodPos.x = pos.X;
			fmodPos.y = pos.Y;
			fmodPos.z = 0;

			_channel?.set3DAttributes(ref fmodPos, ref fmodPos, ref fmodPos);
			_channel?.set3DMinMaxDistance(100, 200);
			
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
