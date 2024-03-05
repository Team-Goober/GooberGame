using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Characters;

namespace Sprint
{
    internal class Tiles : IGameObject, ICollidable
    {
        ISprite sprite;
        Vector2 position;
        Rectangle bounds;

        public Tiles(Goober game, ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
            // TODO: replace with value from files
            float size = 16;
            bounds = new Rectangle((int)(position.X - size/2), (int)(position.Y - size/2), (int)size, (int)size);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public Rectangle GetBoundingBox()
        {
            return bounds;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
