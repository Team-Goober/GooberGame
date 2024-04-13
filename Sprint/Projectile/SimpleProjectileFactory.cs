using Microsoft.Xna.Framework;
using Sprint.Functions.SecondaryItem;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory
    {
        private SpriteLoader spriteLoader;
        private Room room;

        Vector2 position;
        Vector2 direction;
        bool isEnemy;

        private float distanceOut;

        private const string ANIMS_FILE = "projectileAnims";


        public SimpleProjectileFactory(SpriteLoader spriteLoader, float distanceOut, bool isEnemy, Room room)
        {
            this.distanceOut = distanceOut;
            this.spriteLoader = spriteLoader;
            this.room = room;
            this.isEnemy = isEnemy;
        }

        // Change the room that projectiles are placed in
        public void SetRoom(Room room)
        {
            this.room = room;
        }

        public IProjectile CreateFromString(string name)
        {
            IProjectile ret = null;
            switch (name)
            {
                case "smoke":
                    ret = CreateSmoke();
                    break;
                case "arrow":
                    ret = CreateArrow();
                    break;
                case "blueArrow":
                    ret = CreateBlueArrow();
                    break;
                case "bomb":
                    ret = CreateBomb();
                    break;
                case "boomerang":
                    ret = CreateBoomerang();
                    break;
                case "blueBoomerang":
                    ret = CreateBlueBoomerang();
                    break;
                case "fireBall":
                    ret = CreateFireBall();
                    break;
            }
            return ret;
        }

        public Smoke CreateSmoke()
        {
            return new Smoke(
                spriteLoader.BuildSprite(ANIMS_FILE, "smoke"),
                getSpawnPosition(), room);
        }

        public Arrow CreateArrow()
        {
            Arrow proj = new Arrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "arrow"),
                getSpawnPosition(), direction, isEnemy, room);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public BlueArrow CreateBlueArrow()
        {
            BlueArrow proj = new BlueArrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "bluearrow"),
                getSpawnPosition(), direction, isEnemy, room);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public Bomb CreateBomb()
        {
            Bomb proj = new Bomb(
                 spriteLoader.BuildSprite(ANIMS_FILE, "bomb"),
                 getSpawnPosition(), direction, isEnemy, room);
            return proj;
        }
        
        public Boomerang CreateBoomerang()
        {
            Boomerang proj = new Boomerang(
                spriteLoader.BuildSprite(ANIMS_FILE, "boomerang"),
                getSpawnPosition(), direction, isEnemy, room);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public BlueBoomerang CreateBlueBoomerang()
        {
            BlueBoomerang proj = new BlueBoomerang(
                spriteLoader.BuildSprite(ANIMS_FILE, "blueboomerang"),
                getSpawnPosition(), direction, isEnemy, room);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public FireBall CreateFireBall()
        {
            FireBall proj = new FireBall(
                spriteLoader.BuildSprite(ANIMS_FILE, "fireball"),
                getSpawnPosition(), direction, isEnemy, room);
            return proj;
        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }


        public Vector2 GetDirection()
        {
            return direction;
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
