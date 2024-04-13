
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

        private IEffect applyCommand;
        private ISprite sprite;
        private string label;

        public InstantPowerup(ISprite sprite, IEffect applyCommand, string label)
        {
            this.sprite = sprite;
            this.applyCommand = applyCommand;
            this.label = label;
        }


        public void Apply(Player player)
        {
            applyCommand.Execute(player);
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

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
