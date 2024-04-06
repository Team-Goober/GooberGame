using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using Microsoft.Xna.Framework.Audio;
using Sprint.Collision;
using Sprint.Functions.SecondaryItem;
using Sprint.Levels;
using Sprint.Music.Sfx;
using System.Diagnostics;

namespace Sprint.Projectile
{
    internal class Boomerang : DissipatingProjectile
    {
        private const int SPEED = 400;
        private const int TRAVEL = 600;
        private const int RETURN_TRAVEL = 200;
        private bool returned;
        private PlaceSmoke smoke;
        private SfxFactory sfxFactory;
        public readonly double dmg = 1;

        public Boomerang(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room) :
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.LoopSoundEffect("Magical Boomerang Thrown", this);
            returned = false;
        }

        public double DamageAmount()
        {
            return dmg;
        }

        public void SetSmokeCommand(PlaceSmoke smoke)
        {
            this.smoke = smoke;
        }

        public override void Dissipate()
        {
            sfxFactory.EndLoopSoundEffect("Magical Boomerang Thrown", this);
            smoke.Execute();
        }

        public override void Update(GameTime gameTime)
        {
            // Move linearly one way, then flip and come back the other way
            if (!returned && (position - startPos).Length() > RETURN_TRAVEL)
            {
                velocity *= -1;
                returned = true;
            }
            base.Update(gameTime);
        }
    }
}
