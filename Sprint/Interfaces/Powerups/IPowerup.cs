using Sprint.Levels;
using Sprint.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Items;

namespace Sprint.Interfaces.Powerups
{
    internal interface IPowerup
    {

        // Apply any changes that must occur when an item is picked up
        // Adds to inventory and potentially runs behavior
        public void Apply(Player player);

        // Returns true if inventory is able to apply this item
        public bool CanPickup(Inventory inventory);

        // Label that identifies powerup type
        public string GetLabel();

        // Descriptive text of powerup
        public string GetDescription();

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime);

        public void Update(GameTime gameTime);

    }
}
