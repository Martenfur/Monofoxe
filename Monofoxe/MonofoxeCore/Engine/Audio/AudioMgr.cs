using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

// DO NOT include FMOD namespace in ANY of your classes.
// Use FMOD.SomeClass instead.
// FMOD classes seriously interfere with System namespace.


namespace Monofoxe.Engine.Audio
{
	// TODO: Figure out how to make this stuff cross-platform.
	// Maybe add an interface and make a singleton?

	/// <summary>
	/// Audio manager. Controls main audiosystem parameters.
	/// </summary>
	public static class AudioMgr
	{
		public static FMOD.System FMODSystem;
		public static FMOD.RESULT LastResult {get; internal set;}
		
		private static string _sfxExtension = ".wav";
		private static string _musicExtension = ".ogg";

		public static int ListenerCount 
		{
			get 
			{
				var listeners = 0;
				LastResult = FMODSystem.get3DNumListeners(out listeners);
				return listeners;
			}
			set => LastResult = FMODSystem.set3DNumListeners(value);
		}

		static bool _isWindows = true; // Temporary var. 

		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		//[DllImport("libdl.so.2")] // UNTESTED
		//static extern IntPtr dlopen(string filename, int flags);


		internal static void Init()
		{
			if (_isWindows)
			{
				if (Environment.Is64BitProcess)
				{
					LoadLibrary(Path.GetFullPath("FMOD/x64/fmod.dll"));
				}
				else
				{
					LoadLibrary(Path.GetFullPath("FMOD/x32/fmod.dll"));
				}
			}
			else
			{
			//	Console.WriteLine("BOI" + Environment.CurrentDirectory + "/FMOD/x64/fmod.so");
			//	dlopen(Environment.CurrentDirectory + "/FMOD/x64/fmod.so", 1);
			}

			//System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
			
			FMOD.System system;
			FMOD.Factory.System_Create(out system);
			FMODSystem = system;

			FMODSystem.setDSPBufferSize(1024, 10);
			FMODSystem.init(32, FMOD.INITFLAGS.CHANNEL_LOWPASS | FMOD.INITFLAGS.CHANNEL_DISTANCEFILTER, (IntPtr)0);
		}
		

		public static FMOD.ChannelGroup CreateChannelGroup(string name)
		{
			FMOD.ChannelGroup group;
			FMODSystem.createChannelGroup(name, out group);
			return group;
		}


		internal static void Update() =>
			FMODSystem.update();
		
		public static void Unload() =>
			FMODSystem.release();
		
		
		/// <summary>
		/// Loads sound from file.
		/// Use this function to load short sound effects.
		/// </summary>
		public static Sound LoadSound(string name, FMOD.MODE mode = FMOD.MODE.DEFAULT)
		{
			FMOD.Sound newSound;
			LastResult = FMODSystem.createSound(AssetMgr.ContentDir + '/' + AssetMgr.AudioDir + '/' + name + _sfxExtension, mode, out newSound);
			
			return new Sound(FMODSystem, newSound);
		}
				
		/// <summary>
		/// Loads sound stream from file.
		/// Use this function to load music and lond ambience tracks.
		/// </summary>
		public static Sound LoadStreamedSound(string name, FMOD.MODE mode = FMOD.MODE.DEFAULT)
		{
			FMOD.Sound newSound;
			LastResult = FMODSystem.createStream(AssetMgr.ContentDir + '/' + AssetMgr.AudioDir + '/' + name + _musicExtension, mode, out newSound);
			
			return new Sound(FMODSystem, newSound);
		}


		#region Vector converters.
		
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
		
		#endregion Vector converters.


		/// <summary>
		/// Plays given sound and returns separate instance of it.
		/// </summary>
		public static Sound PlaySound(Sound sound, FMOD.ChannelGroup group = null, bool paused = false)
		{
			FMOD.Channel channel;
			LastResult = FMODSystem.playSound(sound.FMODSound, group, paused, out channel);
			return new Sound(FMODSystem, sound.FMODSound, channel);
		}
		
		/// <summary>
		/// Plays given sound and returns separate instance of it.
		/// </summary>
		public static Sound PlaySoundAt(
			Sound sound, 
			FMOD.ChannelGroup group = null, 
			bool paused = false, 
			Vector2 pos = default(Vector2), 
			Vector2 velocity = default(Vector2)
		)
		{
			FMOD.Channel channel;
			LastResult = FMODSystem.playSound(sound.FMODSound, group, paused, out channel);
			var newSound = new Sound(FMODSystem, sound.FMODSound, channel);
			newSound.Mode = FMOD.MODE._3D;
			newSound.Set3DAttributes(pos, velocity);

			return newSound;
		}



		public static void SetListenerPosition(Vector2 pos, int listenerId = 0)
		{
			var fmodPos = Vector2ToFmodVector(pos);
			var fmodZeroVec = GetFmodVector();
			// Apparently, you cannot just pass zero vector and call it a day.
			var fmodForward = Vector2ToFmodVector(Vector2.UnitY);
			var fmodUp = Vector3ToFmodVector(Vector3.UnitZ);

			LastResult = FMODSystem.set3DListenerAttributes(listenerId, ref fmodPos, ref fmodZeroVec, ref fmodForward, ref fmodUp);
		}

		public static void SetListenerAttributes(Vector2 pos, Vector2 velocity, Vector2 forward, int listenerId = 0)
		{
			var fmodPos = Vector2ToFmodVector(pos);
			var fmodVelocity = Vector2ToFmodVector(velocity);
			var fmodForward = Vector2ToFmodVector(forward);
			var fmodZeroVec = GetFmodVector();
			var fmodUp = Vector3ToFmodVector(Vector3.UnitZ);

			LastResult = FMODSystem.set3DListenerAttributes(listenerId, ref fmodPos, ref fmodVelocity, ref fmodForward, ref fmodUp);		
		}


	}
}
