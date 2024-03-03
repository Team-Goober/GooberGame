using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Sprite;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory
    {
        private SpriteLoader spriteLoader;

        Vector2 position;
        Vector2 direction;

        private float distanceOut;

        private const string ANIMS_FILE = "projectileAnims";


        public SimpleProjectileFactory(SpriteLoader spriteLoader, float distanceOut)
        {
            this.distanceOut = distanceOut;
            this.spriteLoader = spriteLoader;
        }

        public Arrow CreateArrow()
        {
            return new Arrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "arrow"), 
                spriteLoader.BuildSprite(ANIMS_FILE, "smoke"), 
                getSpawnPosition(), direction);
        }

        public BlueArrow CreateBlueArrow()
        {
            return new BlueArrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "bluearrow"), 
                spriteLoader.BuildSprite(ANIMS_FILE, "smoke"), 
                getSpawnPosition(), direction);
        }

        public Bomb CreateBomb()
        {
            return new Bomb(spriteLoader.BuildSprite(ANIMS_FILE, "bomb"), getSpawnPosition()); 
        }
        
        public Boomarang CreateBoomarang()
        { 
            return new Boomarang(spriteLoader.BuildSprite(ANIMS_FILE, "boomerang"), getSpawnPosition(), direction);
        }

        public BlueBoomerang CreateBlueBoomerang()
        {
            return new BlueBoomerang(spriteLoader.BuildSprite(ANIMS_FILE, "blueboomerang"), getSpawnPosition(), direction);
        }

        public FireBall CreateFireBall()
        {
            return new FireBall(spriteLoader.BuildSprite(ANIMS_FILE, "fireball"), getSpawnPosition(), direction);
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public void SetStartPosition(Vector2 pos)
        {
            position = pos;
        }

        private Vector2 getSpawnPosition()
        {
            return position + Vector2.Normalize(direction) * distanceOut;
        }
    }
}
