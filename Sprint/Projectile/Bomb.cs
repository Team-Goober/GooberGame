using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;
using Sprint.Music.Sfx;
using Sprint.Characters;

namespace Sprint.Projectile
{
    internal class Bomb : DissipatingProjectile
    {

        Timer explosionTimer;
        private SfxFactory sfxFactory;

        public override CollisionTypes[] CollisionType
        {
            get
            {
                CollisionTypes[] types = new CollisionTypes[2];
                types[0] = explosionTimer.Ended ? CollisionTypes.BOMB : CollisionTypes.EXPLOSION;
                types[1] = isEnemy ? CollisionTypes.ENEMY_PROJECTILE : CollisionTypes.PROJECTILE;
                return types;
            }
        }


        public override Rectangle BoundingBox
        {
            get
            {
                int sideLength = CharacterConstants.PROJECTILE_SIDE_LENGTH * CharacterConstants.COLLIDER_SCALE;
                // Double side length during explosion
                if (!explosionTimer.Ended)
                {
                    sideLength *= 4;
                }
                return new((int)(position.X - sideLength / 2), (int)(position.Y - sideLength / 2), sideLength, sideLength);
            }
        }

        private const int SPEED = 150;
        private const int TRAVEL = 50;

        public Bomb(ISprite sprite, Vector2 startPos, Vector2 direction, bool isEnemy, Room room) :
            base(sprite, startPos, direction, SPEED, TRAVEL, isEnemy, room)
        {
            explosionTimer = new Timer(1);
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.PlaySoundEffect("Bomb Placement");
            damage = CharacterConstants.HIGH_DMG;
        }

        public override void Dissipate()
        {
            if (explosionTimer.Ended)
            {
                sfxFactory.PlaySoundEffect("Bomb Explosion");
                sprite.SetAnimation("explosion");
                explosionTimer.Start();
                velocity = Vector2.Zero;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Handle timers for detonation
            explosionTimer.Update(gameTime);
            if (explosionTimer.JustEnded)
            {
                room.GetScene().Remove(this);
            }

            base.Update(gameTime);

        }
    }
}
