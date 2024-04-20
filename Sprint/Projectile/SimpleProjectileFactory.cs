using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Functions.SecondaryItem;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System.Collections.Generic;

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

        private Dictionary<string, bool> upgraded; // Dictionary of whether each projectile has been upgraded

        public SimpleProjectileFactory(SpriteLoader spriteLoader, float distanceOut, bool isEnemy, Room room)
        {
            this.distanceOut = distanceOut;
            this.spriteLoader = spriteLoader;
            this.room = room;
            this.isEnemy = isEnemy;
            upgraded = new()
            {
                { "arrow", false },
                { "boomerang", false }
            };
        }

        // Change the room that projectiles are placed in
        public void SetRoom(Room room)
        {
            this.room = room;
        }

        // Create projectile based on string identifier
        public IProjectile CreateFromString(string name, Character shooter = null)
        {
            IProjectile ret = null;
            // Choose alternate string if upgraded
            bool isUpgraded = upgraded.ContainsKey(name) && upgraded[name];
            switch (name)
            {
                case "smoke":
                    ret = CreateSmoke();
                    break;
                case "arrow":
                    ret = isUpgraded ? CreateBlueArrow() : CreateArrow();
                    break;
                case "bomb":
                    ret = CreateBomb();
                    break;
                case "boomerang":
                    ret = isUpgraded ? CreateBlueBoomerang() : CreateBoomerang();
                    break;
                case "fireBall":
                    ret = CreateFireBall();
                    break;
                case "enderPearl":
                    ret = CreateEnderPearl(shooter);
                    break;
                case "hook":
                    ret = CreateHook(shooter);
                    break;
                case "lavabucket":
                    ret = CreateLava(shooter);
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

        public LavaBucket CreateLava(Character shooter)
        {
            LavaBucket proj = new LavaBucket(
                spriteLoader.BuildSprite(ANIMS_FILE, "lavabucket"),
                getSpawnPosition(), direction, isEnemy, room, shooter);
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

        public EnderPearl CreateEnderPearl(Character shooter)
        {
            EnderPearl proj = new EnderPearl(
                spriteLoader.BuildSprite(ANIMS_FILE, "enderPearl"),
                getSpawnPosition(), direction, isEnemy, room, shooter);
            return proj;
        }

        public Hook CreateHook(Character shooter)
        {
            Hook proj = new Hook(
                spriteLoader.BuildSprite(ANIMS_FILE, "bluearrow"),
                getSpawnPosition(), direction, isEnemy, room, shooter);
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

        public void SetUpgraded(string name, bool up)
        {
            upgraded[name] = up;
        }

        public bool GetUpgraded(string name)
        {
            return upgraded[name];
        }

        private Vector2 getSpawnPosition()
        {
            return position + Vector2.Normalize(direction) * distanceOut;
        }
    }
}
