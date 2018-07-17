using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Monofoxe.Engine.Audio
{
	
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
			_FMODSystem.init(32, FMOD.INITFLAGS.CHANNEL_LOWPASS, (IntPtr)0);
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

		
		public static Sound Play(Sound sound) =>
			null;

		
		public static Sound Play(Sound sound, float volume, float pitch) =>
			null;


	}
}
