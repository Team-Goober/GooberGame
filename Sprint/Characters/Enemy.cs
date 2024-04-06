using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;
using System.Runtime.Serialization;
using System;

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
        protected Room room;
        private SfxFactory sfxFactory;

        public Enemy(ISprite sprite, ISprite damagedSprite, Vector2 position, Room room)
        {
            this.defaultSprite = sprite;
            this.sprite = sprite;
            this.damagedSprite = damagedSprite;
            physics = new Physics(position);
            this.room = room;
            sfxFactory = SfxFactory.GetInstance();
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

        public override void TakeDamage()
        {
            damageTimer.Start();
            this.sprite = damagedSprite;
            OnEnemyDamaged?.Invoke(this, EventArgs.Empty);
        }

        public override void Update(GameTime gameTime)
        {
            physics.Update(gameTime);
            sprite.Update(gameTime);
            

            damageTimer.Update(gameTime);
            // damage timer to switch between sprites
            if (damageTimer.JustEnded)
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
