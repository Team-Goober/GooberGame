using System;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using XMLData;

namespace Sprint.Music.Sfx
{
    public class SfxFactory
    {

        private Dictionary<string, SoundEffect> soundEffects;
        private LevelData levelData;
        private static SfxFactory instance;
        private float currentVolume;
        private int volumeCounter;
        private bool isMuted;


        public SfxFactory()
        {
            soundEffects = new Dictionary<string, SoundEffect>();
            currentVolume = 0.1f;
            volumeCounter = 1;
            isMuted = false;
        }

        /// <summary>
        /// Get an instance of the SfxFactory, create a new one on first run
        /// </summary>
        /// <returns>The instance of SfxFactory</returns>
        public static SfxFactory GetInstance()
        {
            return instance ??= new SfxFactory();
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
        public void MakeSongs()
        {
            //All song names loaded in
            List<SfxData> songNames = LoadXML();

            //Add each song to the SFX dictionary
            foreach (var songName in songNames)
            {
                if(!soundEffects.ContainsKey(songName.SoundName))
                    soundEffects.Add(songName.SoundName , Goober.content.Load<SoundEffect>(songName.SoundName)); 
            }

            //Set base volume
            SoundEffect.MasterVolume = currentVolume;
        }

        /// <summary>
        /// Play a sound effect
        /// </summary>
        /// <param name="name">Name of the sound effect to play</param>
        public void PlaySoundEffect(string name)
        {
            soundEffects[name].Play();
        }

        /// <summary>
        /// Return an instance of the sound effect that can be used multiple times
        /// </summary>
        /// <param name="name">Name of the sound effect instance to return</param>
        /// <returns>An instance of the sound effect</returns>
        public SoundEffectInstance GetSoundEffect(string name)
        {
            return soundEffects[name].CreateInstance();
        }

        /// <summary>
        /// Set SFX volume higher
        /// </summary>
        public void VolumeUp()
        {
            if (volumeCounter == 1)
            {
                return;
            }

            volumeCounter++;
            currentVolume += .1f;
            SoundEffect.MasterVolume = currentVolume;
        }

        /// <summary>
        /// Set SFX volume Lower
        /// </summary>
        public void VolumeDown()
        {
            if (volumeCounter == 0)
            {
                return;
            }

            volumeCounter--;
            currentVolume -= .1f;
            SoundEffect.MasterVolume = currentVolume;
        }

        /// <summary>
        /// Toggle mute for SFX
        /// </summary>
        public void MuteToggle()
        {
            if (!isMuted)
            {
                SoundEffect.MasterVolume = 0.0f;
                isMuted = true;
            }
            else
            {
                SoundEffect.MasterVolume = currentVolume;
                isMuted = false;
            }
        }

    }
}
