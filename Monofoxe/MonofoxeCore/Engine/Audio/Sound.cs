using System;
using System.Collections.Generic;
using System.Text;

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
				if(_channel != null)
				{
					SetLastResult(_channel.getPitch(out pitch));
				}
				return pitch;
			}
			set
			{
				if(_channel != null)
				{
					SetLastResult(_channel.setPitch(value));
				}
			}
		}

		public float Volume
		{
			get
			{
				var volume = 1f;
				if(_channel != null)
				{
					SetLastResult(_channel.getVolume(out volume));
				}
				return volume;
			}
			set
			{
				if(_channel != null)
				{
					SetLastResult(_channel.setVolume(value));
				}
			}
		}

		public float LowPass
		{
			get
			{
				var lowPassGain = 1f;
				if(_channel != null)
				{
					SetLastResult(_channel.getLowPassGain(out lowPassGain));
				}
				return lowPassGain;
			}
			set
			{
				if(_channel != null)
				{
					SetLastResult(_channel.setLowPassGain(value));
				}
			}
		}

		public float TrackPosition;

		public bool IsPlaying;

		

		#endregion



		public Sound(FMOD.System system, FMOD.Sound sound)
		{
			_FMODSystem = system;
			FMODSound = sound;
		}



		public void Play(bool paused = false)
		{
			SetLastResult(_FMODSystem.playSound(FMODSound, null, paused, out _channel));
		}


		public void Play(FMOD.ChannelGroup group, bool paused = false)
		{
			SetLastResult(_FMODSystem.playSound(FMODSound, group, paused, out _channel));
		}


		public void Pause()
		{
			if(_channel != null)
			{
				SetLastResult(_channel.setPaused(true));
			}
		}

		public void Resume()
		{
			if(_channel != null)
			{
				SetLastResult(_channel.setPaused(false));
			}
		}


		public void Stop()
		{
			if(_channel != null)
			{
				SetLastResult(_channel.stop());
			}
		}


		/// <summary>
		/// Sets last result to the Audio Manager.
		/// NOTE: There is very high probability that this code will change, so 
		/// making it separate function will make life a bit easier.
		/// </summary>
		private void SetLastResult(FMOD.RESULT result) =>
			AudioMgr.LastResult = result;

	}
}
