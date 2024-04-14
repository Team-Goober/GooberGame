
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;

namespace Sprint.Items
{
    internal class InstantPowerup : IPowerup
    {

        /*
         *  Represents a powerup whose effect is instantly activated on pickup and isn't stored in inventory
         */

        private IEffect onApply;
        private ISprite sprite;
        private string label;
        private string description;

        private TimeSpan lastUpdate;

        public InstantPowerup(ISprite sprite, IEffect onApply, string label, string description)
        {
            this.sprite = sprite;
            this.onApply = onApply;
            this.label = label;
            this.description = description;
        }


        public void Apply(Player player)
        {
            onApply.Execute(player);
        }

        public bool CanPickup(Inventory inventory)
        {
            return true;
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
