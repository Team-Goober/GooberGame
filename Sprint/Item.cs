
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint
{
    internal class Item : IGameObject
    {
        ISprite sprite;
        Vector2 position;

        public Item(Game1 game, ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
