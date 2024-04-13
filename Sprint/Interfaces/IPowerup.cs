using Sprint.Levels;
using Sprint.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Items;

namespace Sprint.Interfaces
{
    internal interface IPowerup
    {

        public void Apply(Player player);

        public bool CanPickup(Inventory inventory);

        public string GetLabel();

        public IEffect GetEffect();

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime);

        public void Update(GameTime gameTime);

    }
}
