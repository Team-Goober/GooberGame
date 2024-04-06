using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;
using System.Runtime.Serialization;
using System;
using System.Threading.Tasks.Dataflow;
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
        public event EventHandler OnEnemyDamaged;
        public event EventHandler OnEnemyDied;
        protected Room room;
        private SfxFactory sfxFactory;
        protected double health;

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
            damageTimer.Start();
            this.sprite = damagedSprite;
            health -= dmg;
            // Trigger death when health is at or below 0
            if (health <= 0.0)
            {
                EnemyDeathEvent?.Invoke();
                Die();
            }
            else
            {
                OnEnemyDamaged?.Invoke(this, EventArgs.Empty);
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


        // Remove enemy from game
        public override void Die()
        {
            room.GetScene().Remove(this);
            sfxFactory.PlaySoundEffect("Enemy Death");
        }
    }
}
