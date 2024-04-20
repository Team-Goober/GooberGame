using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;
using Sprint.Characters;
using Microsoft.Xna.Framework.Graphics;
using System;
using Sprint.Collision;

namespace Sprint.Projectile
{
    internal class LavaBucket : DissipatingProjectile
    {

        private const int SPEED = 800;
        private const int TRAVEL = 700;
        private const int DISCONNECT_RADIUS = 50; // Distance from pulled character that pulling should stop at
        private SfxFactory sfxFactory;
        private Character shooter; // Character that shot the hook and holds the rope
        private Character target; // Character pierced by the hook
        private Vector2 targetOffset; // Offset from center of target that the hook is stuck in
        private Timer pullSoundTimer; // Time between each play of the pull sound effect
        private int pullsRemaining; // Number of loops of the timer before the hook despawns
        private bool heavyShooter; // If true, shooter is heavy and should drag a target instead of being dragged
        // Three modes of the sequence
        private enum LavaState
        {
            MOVING,
            STUCK,
            PULLING
        }
        LavaState state;

        public override CollisionTypes[] CollisionType
        {
            get
            {
                // Don't collide when done moving
                if( state != LavaState.MOVING )
                {
                    return new CollisionTypes[] { };
                }
                else
                {
                    return base.CollisionType;
                }
            }
        }

        public LavaBucket(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room, Character shooter) : 
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            this.shooter = shooter;
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.PlaySoundEffect("Arrow Shot");
            damage = CharacterConstants.TINY_DMG;
            state = LavaState.MOVING;
            pullSoundTimer = new Timer(0.25);
            pullSoundTimer.SetLooping(true);
            pullsRemaining = 20;
            heavyShooter = false;
        }

        public void SetHeavyShooter(bool heavy)
        {
            heavyShooter = heavy;
        }

        public bool GetHeavyShooter()
        {
            return heavyShooter;
        }

        public override void Dissipate()
        {
            if(state == LavaState.MOVING ) {
                sprite.SetAnimation("lava");
                sfxFactory.PlaySoundEffect("Arrow Hit");
                // Stop moving
                state = LavaState.STUCK;
            }
        }

        public void Retract()
        {
            if (state == LavaState.MOVING)
            {
                // If haven't hit a target yet, don't pull back
                sfxFactory.PlaySoundEffect("Arrow Shot");
                Delete();
            }else if(state == LavaState.STUCK)
            {
                // If stuck in target, pull the user towards it
                state = LavaState.PULLING;
                // Start playing sound
                sfxFactory.PlaySoundEffect("Arrow Shot");
                pullSoundTimer.Start();
            }

        }

        public override void Hit(Character subject)
        {
            // Do damage
            base.Hit(subject);

            sprite.SetAnimation("lava");
            // Update target to reflect piercing the subject
            target = subject;
            targetOffset = position - target.GetPosition();

            // Should delete as response to target's death if target is an enemy
            if(target is Enemy)
                (target as Enemy).EnemyDeathEvent += Delete;
        }

        public override void Delete()
        {
            // Remove event response if needed
            if (target is Enemy)
                (target as Enemy).EnemyDeathEvent -= Delete;
            base.Delete();
        }

        public override void Update(GameTime gameTime)
        {
            // Handle projectile movement
            if (state == LavaState.MOVING)
            {
                // Only do velocity movement while still moving
                base.Update(gameTime);
            }
            else if (state == LavaState.STUCK || state == LavaState.PULLING)
            {
                if(target != null)
                {
                    // If pierced a target, follow its position
                    position = target.GetPosition() + targetOffset;

                }
                else
                {
                    sprite.SetAnimation("lava");
                    Delete();
                }
            }

            if(target is Enemy)
            {
                base.Hit(target);
            }





            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw hook
            base.Draw(spriteBatch, gameTime);

            // Calculate dimentions of rope box
            Vector2 distance = shooter.GetPosition() - position;
            Vector2 dimensions = new Vector2(5, distance.Length());
            float rotation = -(float)Math.Atan2(distance.X, distance.Y);



        }
    }
}
