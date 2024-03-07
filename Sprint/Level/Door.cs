using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Level;

namespace Sprint
{
    internal class Door : IGameObject, ICollidable
    {
        ISprite sprite;
        Vector2 position;
        Rectangle bounds;
        Room otherSide;

        public Door(ISprite sprite, Vector2 position, Vector2 size, Room otherSide)
        {
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)(position.X), (int)(position.Y), (int)(size.X), (int)(size.Y));
            this.otherSide = otherSide;
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

        public Room GetAdjacentRoom()
        {
            return otherSide;
        }

    }
}
