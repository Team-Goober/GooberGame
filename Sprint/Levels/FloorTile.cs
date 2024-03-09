using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Levels
{
    internal class FloorTile : ITile
    {
        ISprite sprite;
        Vector2 position;
        Rectangle bounds;

        public FloorTile(ISprite sprite, Vector2 position, Vector2 size)
        {
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
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
