using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;

namespace Sprint.Characters
{
    public class Enemy : Character, IMovingCollidable
    {
        protected ISprite sprite;
        protected Physics physics;

        public Enemy(ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            physics = new Physics(position);
        }

        public Rectangle BoundingBox => new((int)(physics.Position.X - 8 * 3),
            (int)(physics.Position.Y - 8 * 3),
            16 * 3,
            16 * 3);

        public CollisionTypes[] CollisionType => new CollisionTypes[] {CollisionTypes.ENEMY, CollisionTypes.CHARACTER};

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, physics.Position, gameTime);
        }

        public void Move(Vector2 distance)
        {
            physics.SetPosition(physics.Position + distance);
        }

        public override void Update(GameTime gameTime)
        {
            physics.Update(gameTime);
            sprite.Update(gameTime);
        }
    }
}
