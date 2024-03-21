using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Functions.SecondaryItem;
using Sprint.Levels;
using Sprint.Sprite;
using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory
    {
        private SpriteLoader spriteLoader;
        private SceneObjectManager objectManager;

        Vector2 position;
        Vector2 direction;
        bool isEnemy;

        private float distanceOut;

        private const string ANIMS_FILE = "projectileAnims";


        public SimpleProjectileFactory(SpriteLoader spriteLoader, float distanceOut, bool isEnemy, SceneObjectManager objectManager)
        {
            this.distanceOut = distanceOut;
            this.spriteLoader = spriteLoader;
            this.objectManager = objectManager;
            this.isEnemy = isEnemy;
        }

        public void SetScene(SceneObjectManager scene)
        {
            objectManager = scene;
        }

        public Smoke CreateSmoke()
        {
            return new Smoke(
                spriteLoader.BuildSprite(ANIMS_FILE, "smoke"),
                getSpawnPosition(), objectManager);
        }

        public Arrow CreateArrow()
        {
            Arrow proj = new Arrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "arrow"),
                getSpawnPosition(), direction, isEnemy, objectManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public BlueArrow CreateBlueArrow()
        {
            BlueArrow proj = new BlueArrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "bluearrow"),
                getSpawnPosition(), direction, isEnemy, objectManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public Bomb CreateBomb()
        {
            Bomb proj = new Bomb(
                 spriteLoader.BuildSprite(ANIMS_FILE, "bomb"),
                 getSpawnPosition(), direction, isEnemy, objectManager);
            return proj;
        }
        
        public Boomerang CreateBoomarang()
        {
            Boomerang proj = new Boomerang(
                spriteLoader.BuildSprite(ANIMS_FILE, "boomerang"),
                getSpawnPosition(), direction, isEnemy, objectManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public BlueBoomerang CreateBlueBoomerang()
        {
            BlueBoomerang proj = new BlueBoomerang(
                spriteLoader.BuildSprite(ANIMS_FILE, "blueboomerang"),
                getSpawnPosition(), direction, isEnemy, objectManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public FireBall CreateFireBall()
        {
            FireBall proj = new FireBall(
                spriteLoader.BuildSprite(ANIMS_FILE, "fireball"),
                getSpawnPosition(), direction, isEnemy, objectManager);
            return proj;
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
