
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


        public void Apply(Player player)
        {
            player.GetInventory().AddPowerup(this);
            onApply?.Execute(player);
        }

        public bool CanPickup(Inventory inventory)
        {
            return !inventory.HasPowerup(label);
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
            if (gameTime.TotalGameTime != lastUpdate)
            {
                sprite.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }
    }
}
