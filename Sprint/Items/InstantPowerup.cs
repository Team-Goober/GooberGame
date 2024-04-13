
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;

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

        public InstantPowerup(ISprite sprite, IEffect onApply, string label)
        {
            this.sprite = sprite;
            this.onApply = onApply;
            this.label = label;
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
