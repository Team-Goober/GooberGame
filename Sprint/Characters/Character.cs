
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;

namespace Sprint.Characters
{
    public abstract class Character : IGameObject, IMovingCollidable
    {
        public abstract Rectangle BoundingBox { get; }
        public abstract CollisionTypes[] CollisionType { get; }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void Die();

        public abstract void TakeDamage(double damage);

        public abstract Vector2 GetPosition();
        public abstract void Move(Vector2 distance);
    }
}
