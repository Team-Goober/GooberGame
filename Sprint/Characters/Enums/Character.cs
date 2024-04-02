
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint.Characters
{
    public abstract class Character : IGameObject
    {
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void Die();

        public abstract void TakeDamage();


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
