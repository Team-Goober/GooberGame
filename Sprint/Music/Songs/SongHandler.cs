using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Sprint.Levels;
using Sprint.Loader;
using Sprint.Sprite;
using XMLData;

namespace Sprint.Music.Songs
{
    public class SongHandler
    {
        private float currentVolume;
        private static SongHandler instance;
        private int volumeCounter;
        private bool isMuted;

        public SongHandler()
        {
            currentVolume = 0.1f;
            volumeCounter = 1;
            isMuted = false;
        }

        /// <summary>
        /// Get an instance of the SongHandler, create a new one on first run
        /// </summary>
        /// <returns>The instance of SongHandler</returns>
        public static SongHandler GetInstance()
        {
            return instance ??= new SongHandler();
        }

        /// <summary>
        /// Play a stage song
        /// </summary>
        /// <param name="song">Name of the song to play</param>
        public void PlaySong(string song)
        {
            Song songToPlay = Goober.content.Load<Song>(song);
            MediaPlayer.Play(songToPlay);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = currentVolume;
        }

        /// <summary>
        /// Set song volume higher
        /// </summary>
        public void VolumeUp()
        {
            if (volumeCounter == 10)
            {
                return;
            }
            volumeCounter++;
            currentVolume += .1f;
            MediaPlayer.Volume = currentVolume;
        }

        /// <summary>
        /// Set song volume Lower
        /// </summary>
        public void VolumeDown()
        {
            if (volumeCounter == 0 )
            {
                return;
            }

            volumeCounter--;
            currentVolume -= .1f;
            MediaPlayer.Volume = currentVolume;
        }

        /// <summary>
        /// Toggle mute for Songs
        /// </summary>
        public void MuteToggle()
        {
            if (!isMuted)
            {
                MediaPlayer.Volume = 0.0f;
                isMuted = true;
            }
            else
            {
                MediaPlayer.Volume = currentVolume;
                isMuted = false;
            }
        }
    }
}
