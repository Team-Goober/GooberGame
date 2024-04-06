using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Projectile
{
    internal class Smoke : SimpleProjectile
    {

        private Timer smokeTimer;
        public override CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.PARTICLE };

        public Smoke(ISprite sprite, Vector2 startPos, Room room) : base(sprite, startPos, false, room)
        {
            smokeTimer = new Timer(0.5);
            damage = CharacterConstants.NO_DMG;
        }

        public override void Create()
        {
            smokeTimer.Start();
            base.Create();
        }

        public override void Update(GameTime gameTime)
        {
            smokeTimer.Update(gameTime);

            base.Update(gameTime);

            if (smokeTimer.JustEnded)
            {
                Delete();
            }
        }
    }
}
