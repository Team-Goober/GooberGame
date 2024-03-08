using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using System;

namespace Sprint.Projectile
{
    abstract class SimpleProjectile : IProjectile, IMovingCollidable
    {

        protected GameObjectManager objManager;
        protected Vector2 position;
        protected ISprite sprite;

        public Rectangle BoundingBox => new((int)(position.X - 4 * 3),
            (int)(position.Y - 4 * 3),
            8, 8);

        public virtual CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.PROJECTILE };

        public SimpleProjectile(ISprite sprite, Vector2 startPos, GameObjectManager objManager)
        {
            this.objManager = objManager;
            this.sprite = sprite;
            this.position = startPos;
        }


        public virtual void Create()
        {
            objManager.Add(this);
        }

        public virtual void Delete()
        {
            objManager.Remove(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Move(Vector2 distance)
        {
            position += distance;
        }
    }
}
