using System;
using System.Collections.Generic;
using System.Text;

namespace Monofoxe.Engine.Audio
{
    public class Sound
    {
			private FMOD.Sound _sound;
			private FMOD.System _FMODSystem;
			
			public FMOD.Channel Channel 
			{
				get => _channel; 
				private set => _channel = value;
			}
			public FMOD.Channel _channel;


			public Sound(FMOD.System FMODSystem, FMOD.Sound sound)
			{
				_FMODSystem = FMODSystem;
				_sound = sound;
			}

			public void Load()
			{
				
			}

			public void Play()
			{
				_FMODSystem.playSound(_sound, null, false, out _channel);
			}
			public void Pause()
			{
				
			}
			public void Stop()
			{
				
			}


    }
}
