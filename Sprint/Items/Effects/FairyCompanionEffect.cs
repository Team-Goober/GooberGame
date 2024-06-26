﻿
using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Characters.Companions;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System.Collections.Generic;

namespace Sprint.Items.Effects
{
    internal class FairyCompanionEffect : IEffect
    {

        private string spriteName;
        private string spriteFile;

        private Stack<Companion> companions = new(); // All active fairies
        private Player player;

        public FairyCompanionEffect(string spriteName, string spriteFile)
        {
            this.spriteName = spriteName;
            this.spriteFile = spriteFile;
        }
        public void Execute(Player player)
        {
            // Create new companion on first execution
            if(this.player == null)
            {
                this.player = player;
                // Connect to player health updates to save player if health is zero
                player.OnPlayerHealthChange += onPlayerHealthChanged;
            }
            // Add new fairy to room
            ISprite sprite = this.player.GetSpriteLoader().BuildSprite(spriteFile, spriteName);
            Companion companion = new Companion(sprite, this.player, companions.Count % 2 == 0, companions.Count / 2 % 2 == 0, new Vector2(100, 50));
            companion.SetDisable(false);
            companions.Push(companion);
        }

        private void onPlayerHealthChanged(double prevHp, double nextHp)
        {
            // Check if player about to die
            if(nextHp <= 0.0)
            {
                // Try to decrement number of fairies in inventory
                bool usedFairy = player.GetInventory().TryConsumeStack("fairy");
                if (usedFairy)
                {
                    // Remove a fairy from the stack
                    Companion fairy = companions.Pop();
                    // Remove fairy from the room
                    fairy.SetDisable(true);

                    // Give player health back
                    player.Heal(3f);
                }
            }
        }


        public void Reverse(Player player)
        {
            // Disable all
            while(companions.Count > 0)
            {
                companions.Pop().SetDisable(true);
            }
            // Disconnect signal
            player.OnPlayerHealthChange -= onPlayerHealthChanged;
            this.player = null;
        }

    }
}
