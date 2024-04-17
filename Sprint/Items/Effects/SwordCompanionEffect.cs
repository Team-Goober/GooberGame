
using Sprint.Characters;
using Sprint.Characters.Companions;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Markup;

namespace Sprint.Items.Effects
{
    internal class SwordCompanionEffect : IEffect
    {

        public string spriteName;
        public string spriteFile;

        private Stack<Companion> companions = new(); // All active swords
        private Player player;

        public void Execute(Player player)
        {
            // Create new companion on first execution
            if(this.player == null)
            {
                this.player = player;
            }
            // Add new word to room
            ISprite sprite = player.GetSpriteLoader().BuildSprite(spriteFile, spriteName);
            Companion companion = new SwordCompanion(sprite, player, companions.Count % 2 == 0, companions.Count / 2 % 2 == 0);
            companion.SetDisable(false);
            companions.Push(companion);
        }

        public void Reverse(Player player)
        {
            // Disable all
            while(companions.Count > 0)
            {
                companions.Pop().SetDisable(true);
            }
            // Disconnect signal
            this.player = null;
        }

        public IEffect Clone()
        {
            return new SwordCompanionEffect() { spriteName = spriteName, spriteFile = spriteFile };
        }

    }
}
