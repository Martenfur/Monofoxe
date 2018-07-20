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
			
			var pos = GetFmodVector(32, 32, 0);
			var vel = GetFmodVector(0, 0, 0);
			var forw = GetFmodVector(1, 0, 0);
			var up = GetFmodVector(0, 0, 1);
			_FMODSystem.set3DNumListeners(1);
			_FMODSystem.set3DListenerAttributes(1, ref pos, ref vel, ref forw, ref up);
			
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

		public static FMOD.VECTOR GetFmodVector(float x, float y, float z)
		{
			var vector = new FMOD.VECTOR();
			vector.x = x;
			vector.y = y;
			vector.z = z;
			return vector;
		}

		public static FMOD.VECTOR GetFmodVector(Vector3 v)
		{
			var vector = new FMOD.VECTOR();
			vector.x = v.X;
			vector.y = v.Y;
			vector.z = v.Z;
			return vector;
		}

	//	public void Play(bool paused = false) =>
		//	LastResult = _FMODSystem.playSound(_sound, null, paused, out _channel);


		//public void Play(FMOD.ChannelGroup group, bool paused = false) =>
		//	LastResult = _FMODSystem.playSound(_sound, group, paused, out _channel);

	}
}
