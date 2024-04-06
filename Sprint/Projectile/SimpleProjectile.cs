using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using System;

namespace Sprint.Projectile
{
    abstract class SimpleProjectile : IProjectile, IMovingCollidable
    {

        protected Room room;
        protected Vector2 position;
        protected ISprite sprite;
        protected bool isEnemy;
        public double dmg;

        public Rectangle BoundingBox => new((int)(position.X - 4 * 3),
            (int)(position.Y - 4 * 3),
            8*3, 8*3);

        public virtual CollisionTypes[] CollisionType {
            get
            {
                if (isEnemy)
                {
                    return new CollisionTypes[] { CollisionTypes.ENEMY_PROJECTILE };
                }
                else
                {
                    return new CollisionTypes[] { CollisionTypes.PROJECTILE };
                }
            }
        }


        public double DamageAmount()
        {
            return dmg;
        }

        public SimpleProjectile(ISprite sprite, Vector2 startPos, bool isEnemy, Room room)
        {
            this.room = room;
            this.sprite = sprite;
            this.position = startPos;
            this.isEnemy = isEnemy;
        }


        public virtual void Create()
        {
            room.GetScene().Add(this);
        }

        public virtual void Delete()
        {
            room.GetScene().Remove(this);
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
