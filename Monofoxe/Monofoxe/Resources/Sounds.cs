using Monofoxe.Engine.Audio;


namespace Resources
{
	public static class Sounds
	{
		public static Sound snd1;
		public static Sound snd2;
		public static Sound snd3;
		

		public static void Load()
		{
			snd1 = AudioMgr.LoadStreamedSound("Music/m_mission", FMOD.MODE._3D);
			snd2 = AudioMgr.LoadStreamedSound("Music/m_peace");
			snd3 = AudioMgr.LoadSound("Sounds/punch", FMOD.MODE._3D);
		}

		public static void Unload()
		{
			snd1.Unload();
			snd2.Unload();
			snd3.Unload();
		}

	}
}
