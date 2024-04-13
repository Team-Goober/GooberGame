
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;

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

        public PassivePowerup(ISprite sprite, IEffect onApply, string label)
        {
            this.sprite = sprite;
            this.onApply = onApply;
            this.label = label;
        }


        public void Apply(Player player)
        {
            player.GetInventory().AddPowerup(this);
            onApply.Execute(player);
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

        public IEffect GetEffect()
        {
            return onApply;
        }


        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
