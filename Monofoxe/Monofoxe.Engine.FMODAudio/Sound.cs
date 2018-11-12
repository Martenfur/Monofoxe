using System;
using Microsoft.Xna.Framework;

// DO NOT include FMOD namespace in ANY of your classes.
// Use FMOD.SomeClass instead.
// FMOD classes seriously interfere with System namespace.

namespace Monofoxe.Engine.FMODAudio
{
	/// <summary>
	/// FMOD sound wrapper. Takes horrible FMOD wrapper and makes it look pretty.
	/// Basically, a sound instance.
	/// 
	/// NOTE: You can set sound parameters only AFTER you've started playing sound.
	/// Otherwise they will have no effect!
	/// 
	/// NOTE: My wrappers don't provide full FMOD functionality. For example,
	/// DSPs and advanced 3D stuff are largely left untouched. I may extend my audio
	/// classes to add new features. For now, you have to use FMOD classes directly.
	/// </summary>
	public class Sound
	{
		private FMOD.System _FMODSystem;

		/// <summary>
		/// FMOD sound object.
		/// 
		/// NOTE: It is not recommended to use it directly. 
		/// If you'll really need some of its features, 
		/// just implement them in this class.
		/// </summary>
		public FMOD.Sound FMODSound {get; private set;}
		
		/// <summary>
		/// FMOD channel object.
		/// 
		/// NOTE: It is not recommended to use it directly. 
		/// If you'll really need some of its features, 
		/// just implement them in this class.
		/// 
		/// NOTE: ALWAYS check for null!!!
		/// </summary>
		public FMOD.Channel Channel
		{
			get;
			private set;
		}
		private FMOD.Channel _channel; // Can't use "out" on properties. 


		#region Properties.

		/// <summary>
		/// Amount of loops. 
		/// > 0 - Specific count.
		/// 0 - No loops.
		/// -1 - Infinite loops.
		/// </summary>
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

		/// <summary>
		/// Sound pitch. Affects speed too.
		/// 1 - Normal pitch.
		/// </summary>
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

		/// <summary>
		/// Low pass filter. Makes sound muffled.
		/// 1 - No filtering.
		/// 0 - Full filtering.
		/// </summary>
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

		/// <summary>
		/// Tells if sound is playing.
		/// </summary>
		public bool IsPlaying
		{
			get
			{
				var isPlaying = false;
				SetLastResult(_channel?.isPlaying(out isPlaying));
				return isPlaying;
			}
		}

		/// <summary>
		/// Sound mode. Mainly used for 3D sound.
		/// </summary>
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



		/// <summary>
		/// Plays sound. 
		/// 
		/// NOTE: if you want to create new sound instance, use AudioMgr.PlaySound.
		/// </summary>
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
		
		
		

		/// <summary>
		/// Sets sound mode to 3D and plays it at the given position.
		/// </summary>
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



		/// <summary>
		/// Sets 3D attributes.
		/// </summary>
		/// <param name="pos">Sound position.</param>
		/// <param name="velocity">Sound velocity.</param>
		/// <param name="altPanPos">Panning position.</param>
		public void Set3DAttributes(Vector2 pos, Vector2 velocity, Vector2 altPanPos = default(Vector2))
		{
			var fmodPos = AudioMgr.Vector2ToFmodVector(pos);
			var fmodVelocity = AudioMgr.Vector2ToFmodVector(velocity);
			var fmodAltPanPos = AudioMgr.Vector2ToFmodVector(altPanPos);
			SetLastResult(_channel?.set3DAttributes(ref fmodPos, ref fmodVelocity, ref fmodAltPanPos));
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="minDistance"></param>
		/// <param name="maxDistance"></param>
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
		/// Unloads sound from the game.
		/// Recommended to use only at the end of the game.
		/// </summary>
		public void Unload()
		{
			Stop();
			FMODSound.release();
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
