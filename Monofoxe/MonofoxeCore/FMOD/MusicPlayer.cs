using System;
using System.Runtime.InteropServices;
using System.IO;

namespace FMOD
{
    public class MusicPlayer
    {
        public const int NUM_SONGS = 2;

        public const int SONG_TITLE_MENU = 0;

        public const int SONG_SECTOR_MAP = 1;

        public const int SONG_DESERT = 2;
        public const int SONG_FOREST = 3;
        public const int SONG_OCEAN = 4;
        public const int SONG_BARREN = 5;
        public const int SONG_ICE = 6;
        public const int SONG_LAVA = 7;
        public const int SONG_MINING = 8;
        public const int SONG_ASTEROID_BELT = 9;
        public const int SONG_ENCOUNTER_HUMAN = 10;
        public const int SONG_ENCOUNTER_CYBORG = 11;
        public const int SONG_ENCOUNTER_AI = 12;
        public const int SONG_ENCOUNTER_ALIEN = 13;

        private FMOD.System FMODSystem;
        private FMOD.Channel Channel;
        private FMOD.Sound[] Songs;

        private static MusicPlayer _instance;

        public static MusicPlayer Instance { get { return _instance; } }

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        public static void Init()
        {
            if (Environment.Is64BitProcess)
                LoadLibrary(Path.GetFullPath("FMOD\\64\\fmod.dll"));
            else
                LoadLibrary(Path.GetFullPath("FMOD\\32\\fmod.dll"));

            _instance = new MusicPlayer();
        }

        public void Unload()
        {
            FMODSystem.release();
        }

        private MusicPlayer()
        {
            FMOD.Factory.System_Create(out FMODSystem);

            FMODSystem.setDSPBufferSize(1024, 10);
            FMODSystem.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)0);

            Songs = new FMOD.Sound[NUM_SONGS];

            //LoadSong(SONG_TITLE_MENU, "");
            LoadSong(1, "m_peace");
            //LoadSong(SONG_ENCOUNTER_HUMAN, "");
            //LoadSong(SONG_ENCOUNTER_CYBORG, "");
            //LoadSong(SONG_ENCOUNTER_AI, "");
            //LoadSong(SONG_ENCOUNTER_ALIEN, "");
        }

        private void LoadSong(int songId, string name)
        {
            FMOD.RESULT r = FMODSystem.createStream("Content/Music/" + name + ".ogg", FMOD.MODE.DEFAULT, out Songs[songId]);
            Console.WriteLine("loading " + songId + ", got result " + r);
        }

        private int _current_song_id;

        public bool IsPlaying()
        {
            bool isPlaying = false;
            
            if (Channel != null)
                Channel.isPlaying(out isPlaying);

            return isPlaying;
        }

        public void Play(int songId)
        {
            Console.WriteLine("Play(" + songId + ")");

            if (_current_song_id != songId)
            {
                Stop();

                if (songId >= 0 && songId < NUM_SONGS && Songs[songId] != null)
                {
                    FMODSystem.playSound(Songs[songId], null, false, out Channel);
                    UpdateVolume();
                    Channel.setMode(FMOD.MODE.LOOP_NORMAL);
                    Channel.setLoopCount(-1);

                    _current_song_id = songId;
                }
            }
        }

        public void UpdateVolume()
        {
            if(Channel != null)
				        Channel.setVolume(1f);
        }

        public void Stop()
        {
            if (IsPlaying())
                Channel.stop();

            _current_song_id = -1;
        }
    }
}
