
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint
{
    public abstract class Character : IGameObject
    {
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        
        public enum Directions
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            STILL
        }

    }
}
