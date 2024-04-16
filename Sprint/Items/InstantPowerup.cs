
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


        public bool CanPickup(Inventory inventory)
        {
            // Can always pick up an instant effect
            return true;
        }

        public void Apply(Player player)
        {
            // Run behavior command
            // Don't add to player inventory
            onApply?.Execute(player);
        }

        public void Undo(Player player)
        {
            // Can't reverse changes, and not in inventory
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
