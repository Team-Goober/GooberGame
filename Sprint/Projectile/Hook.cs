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
    internal class Hook : DissipatingProjectile
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
        private enum HookState
        {
            MOVING,
            STUCK,
            PULLING
        }
        HookState state;

        public override CollisionTypes[] CollisionType
        {
            get
            {
                // Don't collide when done moving
                if( state != HookState.MOVING )
                {
                    return new CollisionTypes[] { };
                }
                else
                {
                    return base.CollisionType;
                }
            }
        }

        public Hook(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room, Character shooter) : 
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            this.shooter = shooter;
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.PlaySoundEffect("Arrow Shot");
            damage = CharacterConstants.TINY_DMG;
            state = HookState.MOVING;
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
            if(state == HookState.MOVING ) {
                sfxFactory.PlaySoundEffect("Arrow Hit");
                // Stop moving
                state = HookState.STUCK;
            }
        }

        public void Retract()
        {
            if (state == HookState.MOVING)
            {
                // If haven't hit a target yet, don't pull back
                sfxFactory.PlaySoundEffect("Arrow Shot");
                Delete();
            }else if(state == HookState.STUCK)
            {
                // If stuck in target, pull the user towards it
                state = HookState.PULLING;
                // Start playing sound
                sfxFactory.PlaySoundEffect("Arrow Shot");
                pullSoundTimer.Start();
            }

        }

        public override void Hit(Character subject)
        {
            // Do damage
            base.Hit(subject);
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
            if (state == HookState.MOVING)
            {
                // Only do velocity movement while still moving
                base.Update(gameTime);
            }
            else if (state == HookState.STUCK || state == HookState.PULLING)
            {
                if(target != null)
                {
                    // If pierced a target, follow its position
                    position = target.GetPosition() + targetOffset;
                }
            }

            // Handle subject and target movement
            if (state == HookState.PULLING)
            {
                pullSoundTimer.Update(gameTime);
                Vector2 rope = position - shooter.GetPosition();
                if (rope.Length() <= DISCONNECT_RADIUS)
                {
                    // If player is close enough, the pull is done
                    Delete();
                }
                else
                {
                    // Otherwise, move player towards this
                    // Speed based on shooting speed and length of rope to simulate tension
                    float speed = (1 + rope.Length() / TRAVEL) * (2 * SPEED / 3);

                    if(target != null && heavyShooter)
                    {
                        // If shooter is heavy, pull in target
                        target.Move(Vector2.Normalize(-rope) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        // By default, pull shooter towards hook
                        shooter.Move(Vector2.Normalize(rope) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }

                    // Play sound periodically
                    if (pullSoundTimer.JustEnded)
                    {
                        sfxFactory.PlaySoundEffect("Arrow Shot");
                        // Count down until despawn
                        pullsRemaining--;
                        // Despawn
                        if (pullsRemaining <= 0)
                            Delete();
                    }
                }
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

            // Draw rope from shooter to hook
            Texture2D ropeColor;
            ropeColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            ropeColor.SetData(new Color[] { Color.PaleGoldenrod });
            Vector2 drawPos = new((position.X - dimensions.X / 2), position.Y);
            spriteBatch.Draw(ropeColor, drawPos, new(0,0,1,1), Color.White, rotation, new Vector2(0.5f, 0), dimensions, SpriteEffects.None, 0f);

        }
    }
}
