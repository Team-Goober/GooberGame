using Sprint.Levels;
using Sprint.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Items;

namespace Sprint.Interfaces.Powerups
{
    internal interface IPowerup
    {
        // Returns true if inventory is able to apply this item
        public bool CanPickup(Inventory inventory);

        // Apply any changes that must occur when an item is picked up
        // Adds to inventory and potentially runs behavior
        public void Apply(Player player);

        // Removes from inventory and undoes any changes that were made to the player
        public void Undo(Player player);

        // Label that identifies powerup type
        public string GetLabel();

        // Descriptive text of powerup
        public string GetDescription();

        // Returns the effect that causes powerup behavior
        public IEffect GetEffect();

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime);

        public void Update(GameTime gameTime);

    }
}
