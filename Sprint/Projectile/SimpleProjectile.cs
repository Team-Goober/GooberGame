using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Projectile
{
    abstract class SimpleProjectile : IProjectile, IMovingCollidable
    {

        protected Room room;
        protected Vector2 position;
        protected ISprite sprite;
        protected bool isEnemy;
        protected double damage;

        public virtual Rectangle BoundingBox => new((int)(position.X - CharacterConstants.PROJECTILE_SIDE_LENGTH/2 * CharacterConstants.COLLIDER_SCALE),
            (int)(position.Y - CharacterConstants.PROJECTILE_SIDE_LENGTH/2 * CharacterConstants.COLLIDER_SCALE),
            CharacterConstants.PROJECTILE_SIDE_LENGTH * CharacterConstants.COLLIDER_SCALE, 
            CharacterConstants.PROJECTILE_SIDE_LENGTH * CharacterConstants.COLLIDER_SCALE);

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


        public virtual void Hit(Character subject)
        {
            if (isEnemy)
            {
                // Enemy projectiles only damage player a half heart
                subject.TakeDamage(0.5);
            }
            else
            {
                // Player projectiles do custom damage
                subject.TakeDamage(damage);
            }
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
