
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;

namespace Sprint.Items
{
    internal class PassivePowerup : IPowerup
    {

        private IPowerupCommand applyCommand;
        private ISprite sprite;
        private string label;

        public PassivePowerup(ISprite sprite, IPowerupCommand applyCommand, string label)
        {
            this.sprite = sprite;
            this.applyCommand = applyCommand;
            this.label = label;
        }


        public void Apply(Player player)
        {
            player.GetInventory().AddPowerup(this);
            applyCommand.Execute(player);
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

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
