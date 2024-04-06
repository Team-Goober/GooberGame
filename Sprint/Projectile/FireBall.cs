using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;

namespace Sprint.Projectile
{
    internal class FireBall : DissipatingProjectile
    {

        private const int SPEED = 300;
        private const int TRAVEL = 100;
        private Timer sitTimer;
        private SfxFactory sfxFactory;

        public FireBall(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room) :
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            sitTimer = new Timer(1);
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.PlaySoundEffect("Flames Shot");
            damage = 1;
        }

        public override void Dissipate()
        {
            if (sitTimer.Ended)
            {
                sitTimer.Start();
                velocity = Vector2.Zero;
            }
        }

        public override void Update(GameTime gameTime)
        {
            sitTimer.Update(gameTime);

            base.Update(gameTime);

            if (sitTimer.JustEnded)
            {
                Delete();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }
    }
}
