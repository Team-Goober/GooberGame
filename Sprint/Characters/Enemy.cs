﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using System.Runtime.Serialization;

namespace Sprint.Characters
{
    public class Enemy : Character, IMovingCollidable
    {
        protected ISprite sprite;
        protected ISprite damagedSprite;
        protected Physics physics;
        SceneObjectManager objectManager;
        private float timer = 0;
        private Timer damageTimer = new Timer(0.3);

        public Enemy(ISprite sprite,ISprite damagedSprite, Vector2 position, SceneObjectManager objectManager)
        {
            this.sprite = sprite;
            this.damagedSprite = damagedSprite;
            physics = new Physics(position);
            this.objectManager = objectManager;


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

        public override void TakeDamage()
        {
            damageTimer.Start();
            this.sprite = damagedSprite;
            if (damageTimer.JustEnded)
            {
                // switch back to default sprite (non-damaged)
                this.sprite = sprite;
            }
        }

        public override void Update(GameTime gameTime)
        {
            physics.Update(gameTime);
            sprite.Update(gameTime);

            // damage timer to switch between sprites
            damageTimer.Update(gameTime);
            if (damageTimer.JustEnded)
            {
                // switch back to default sprite (non-damaged)
                this.sprite = sprite;
            }
        }

        // Remove enemy from game
        public override void Die()
        {
            objectManager.Remove(this);
        }
    }
}
