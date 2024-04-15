
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;

namespace Sprint.Items
{
    internal class PassivePowerup : IPowerup
    {

        /*
         *  Represents a powerup which is kept in the inventory and applies a change to the player. Cannot have multiple
         */

        private IEffect onApply;
        private ISprite sprite;
        private string label;
        private string description;

        private TimeSpan lastUpdate;

        public PassivePowerup(ISprite sprite, IEffect onApply, string label, string description)
        {
            this.sprite = sprite;
            this.onApply = onApply;
            this.label = label;
            this.description = description;
        }
        public bool CanPickup(Inventory inventory)
        {
            // Only pickup if player doesn't already have one
            return !inventory.HasPowerup(label);
        }

        public void Apply(Player player)
        {
            // Add this powerup to inventory listing
            player.GetInventory().AddPowerup(this);
            // Run behavior
            onApply?.Execute(player);
        }

        public void Undo(Player player)
        {
            // Remove this powerup from inventory listing
            player.GetInventory().DeletePowerup(this);
            // Undo behavior
            onApply?.Reverse(player);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public string GetLabel()
        {
            return label;
        }

        public string GetDescription()
        {
            return description;
        }

        public void Update(GameTime gameTime)
        {
            // Only update if haven't already updated on this cycle
            if (gameTime.TotalGameTime != lastUpdate)
            {
                sprite.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }
    }
}
