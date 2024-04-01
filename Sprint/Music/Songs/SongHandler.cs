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
        private ContentManager content;

        public SongHandler(ContentManager content)
        {
            this.content = content;
        }

        /// <summary>
        /// Play a stage song
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">Controls if the song should loop or not</param>
        public void PlaySong(string song)
        {
            Song songToPlay = content.Load<Song>(song);
            MediaPlayer.Play(songToPlay);
            MediaPlayer.IsRepeating = true;
        }

    }
}
