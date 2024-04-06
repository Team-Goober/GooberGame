using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;
using System.Runtime.Serialization;
using System;
using System.Threading.Tasks.Dataflow;
using Sprint.Items;
using System.Diagnostics;

namespace Sprint.Characters
{
    internal class Enemy : Character, IMovingCollidable
    {
        protected ISprite sprite;
        protected ISprite defaultSprite;
        protected ISprite damagedSprite;
        protected Physics physics;

        private Timer damageTimer;
        
        protected Room room;
        private SfxFactory sfxFactory;
        protected double health;

        private Item drop = null; // Item to drop upon death

        public Enemy(ISprite sprite, ISprite damagedSprite, Vector2 position, Room room)
        {
            this.defaultSprite = sprite;
            this.sprite = sprite;
            this.damagedSprite = damagedSprite;
            physics = new Physics(position);
            this.room = room;
            sfxFactory = SfxFactory.GetInstance();
            damageTimer = new Timer(0.1);
        }

        public Rectangle BoundingBox => new((int)(physics.Position.X - 8 * 3),
            (int)(physics.Position.Y - 8 * 3),
            16 * 3,
            16 * 3);

        public virtual CollisionTypes[] CollisionType => new CollisionTypes[] {CollisionTypes.ENEMY, CollisionTypes.CHARACTER};

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, physics.Position, gameTime);
        }

        public void Move(Vector2 distance)
        {
            physics.SetPosition(physics.Position + distance);
        }

        public delegate void EnemyDeathDelegate();
        public event EnemyDeathDelegate EnemyDeathEvent;

        public override void TakeDamage(double dmg)
        {
            // Only take damage if not in invulnerable frames
            if (damageTimer.Ended)
            {
                damageTimer.Start();
                this.sprite = damagedSprite;
                health -= dmg;
                // Trigger death when health is at or below 0
                if (health <= 0.0)
                {
                    EnemyDeathEvent?.Invoke();
                    Die();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            physics.Update(gameTime);
            sprite.Update(gameTime);
            

            damageTimer?.Update(gameTime);
            // damage timer to switch between sprites
            if (damageTimer.JustEnded && damageTimer != null)
            {
                // switch back to default sprite (non-damaged)
                this.sprite = this.defaultSprite;

            }
        }

        public void GiveDrop(Item drop)
        {
            this.drop = drop;
        }

        // Remove enemy from game
        public override void Die()
        {
            room.GetScene().Remove(this);
            // Handle scene
            sfxFactory.PlaySoundEffect("Enemy Death");
            // Handle item drop
            if(drop != null)
            {
                drop.SetPosition(physics.Position);
                room.GetScene().Add(drop);
            }
        }
    }
}
