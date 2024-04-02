using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using XMLData;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.IO;

namespace Sprint.Music.Sfx
{
    public class SfxFactory
    {

        private Dictionary<string, SoundEffect> soundEffects;
        private LevelData levelData;

        public SfxFactory()
        {
            soundEffects = new Dictionary<string, SoundEffect>();
        }

        /// <summary>
        /// Load songs from XML data
        /// </summary>
        /// <returns> List of SfxData </returns>
        private List<SfxData> LoadXML()
        {
            levelData = Goober.content.Load<LevelData>("LevelOne/Level1");
            return levelData.Sfxs;
        }

        /// <summary>
        /// Makes all songs and adds to soundEffect list
        /// </summary>
        private void MakeSongs()
        {
            List<SfxData> songNames = LoadXML();

            foreach (var songName in songNames)
            {
                if(!soundEffects.ContainsKey(songName.SoundName))
                    soundEffects.Add(songName.SoundName , Goober.content.Load<SoundEffect>(songName.SoundName)); 
            }
        }

        /// <summary>
        /// Play a sound effect
        /// </summary>
        /// <param name="name">Name of the sound effect to play</param>
        public void PlaySoundEffect(string name)
        {
            MakeSongs();
            soundEffects[name].Play();
        }

        /// <summary>
        /// Return an instance of the sound effect that can be used multiple times
        /// </summary>
        /// <param name="name">Name of the sound effect instance to return</param>
        /// <returns>An instance of the sound effect</returns>
        public SoundEffectInstance GetSoundEffect(string name)
        {
            MakeSongs();
            return soundEffects[name].CreateInstance();
        }

    }
}
