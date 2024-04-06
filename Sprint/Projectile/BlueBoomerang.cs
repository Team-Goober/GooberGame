using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Functions.SecondaryItem;
using Sprint.Levels;
using Sprint.Music.Sfx;

namespace Sprint.Projectile
{
    internal class BlueBoomerang : DissipatingProjectile
    {
        private const int SPEED = 200;
        private const int TRAVEL = 600;
        private const int RETURN_TRAVEL = 200;
        private bool returned;
        private PlaceSmoke smoke;
        private SfxFactory sfxFactory;

        public BlueBoomerang(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room) :
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.LoopSoundEffect("Magical Boomerang Thrown", this);
            returned = false;
            damage = 2;
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
