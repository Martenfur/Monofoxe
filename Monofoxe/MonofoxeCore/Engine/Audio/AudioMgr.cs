using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Audio
{
	// TODO: FIgure out how to make this stuff cross-platform.
	// Maybe add an interface and make a singleton?

	public static class AudioMgr
	{
		private static FMOD.System _FMODSystem;
		public static FMOD.RESULT LastResult {get; internal set;}

		private static string _audioPath = "Content/Music/";
		private static string _sfxExtension = ".wav";
		private static string _musicExtension = ".ogg";

		public static int ListenerCount 
		{
			get 
			{
				var listeners = 0;
				LastResult = _FMODSystem.get3DNumListeners(out listeners);
				return listeners;
			}
			set => LastResult = _FMODSystem.set3DNumListeners(value);
		}


		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		internal static void Init()
		{
			if(Environment.Is64BitProcess)
			{
				LoadLibrary(Path.GetFullPath("FMOD/x64/fmod.dll"));
			}
			else
			{
				LoadLibrary(Path.GetFullPath("FMOD/x32/fmod.dll"));
			}

			FMOD.Factory.System_Create(out _FMODSystem);
			_FMODSystem.setDSPBufferSize(1024, 10);
			_FMODSystem.init(32, FMOD.INITFLAGS.CHANNEL_LOWPASS | FMOD.INITFLAGS.CHANNEL_DISTANCEFILTER, (IntPtr)0);
		}
		
		public static FMOD.ChannelGroup CreateChannelGroup(string name)
		{
			FMOD.ChannelGroup group;
			_FMODSystem.createChannelGroup(name, out group);
			return group;
		}


		public static void Unload() =>
			_FMODSystem.release();


		internal static void Update() =>
			_FMODSystem.update();
		
		

		public static Sound LoadSound(string name, FMOD.MODE mode = FMOD.MODE.DEFAULT)
		{
			FMOD.Sound newSound;
			LastResult = _FMODSystem.createSound(_audioPath + name + _sfxExtension, mode, out newSound);
			
			return new Sound(_FMODSystem, newSound);
		}
		
		public static Sound LoadStreamedSound(string name, FMOD.MODE mode = FMOD.MODE.DEFAULT)
		{
			FMOD.Sound newSound;
			LastResult = _FMODSystem.createStream(_audioPath + name + _musicExtension, mode, out newSound);
			
			return new Sound(_FMODSystem, newSound);
		}



		public static FMOD.VECTOR GetFmodVector(float x = 0, float y = 0, float z = 0)
		{
			var vector = new FMOD.VECTOR();
			vector.x = x;
			vector.y = y;
			vector.z = z;
			return vector;
		}

		public static FMOD.VECTOR Vector3ToFmodVector(Vector3 v)
		{
			var vector = new FMOD.VECTOR();
			vector.x = v.X;
			vector.y = v.Y;
			vector.z = v.Z;
			return vector;
		}

		public static FMOD.VECTOR Vector2ToFmodVector(Vector2 v)
		{
			var vector = new FMOD.VECTOR();
			vector.x = v.X;
			vector.y = v.Y;
			vector.z = 0;
			return vector;
		}



		public static Sound PlaySound(Sound sound, FMOD.ChannelGroup group = null, bool paused = false)
		{
			FMOD.Channel channel;
			LastResult = _FMODSystem.playSound(sound.FMODSound, group, paused, out channel);
			return new Sound(_FMODSystem, sound.FMODSound, channel);
		}


		public static void SetListenerPosition(Vector2 pos, int listenerId = 0)
		{
			var fmodPos = Vector2ToFmodVector(pos);
			var fmodZeroVec = GetFmodVector();
			// Apparently, you cannot just pass zero vector and call it a day.
			var fmodForward = Vector2ToFmodVector(Vector2.UnitY);
			var fmodUp = Vector3ToFmodVector(Vector3.UnitZ);

			LastResult = _FMODSystem.set3DListenerAttributes(listenerId, ref fmodPos, ref fmodZeroVec, ref fmodForward, ref fmodUp);
		}

		public static void SetListenerAttributes(Vector2 pos, Vector2 velocity, Vector2 forward, int listenerId = 0)
		{
			var fmodPos = Vector2ToFmodVector(pos);
			var fmodVelocity = Vector2ToFmodVector(velocity);
			var fmodForward = Vector2ToFmodVector(forward);
			var fmodZeroVec = GetFmodVector();
			var fmodUp = Vector3ToFmodVector(Vector3.UnitZ);

			LastResult = _FMODSystem.set3DListenerAttributes(listenerId, ref fmodPos, ref fmodVelocity, ref fmodForward, ref fmodUp);		
		}


	}
}
