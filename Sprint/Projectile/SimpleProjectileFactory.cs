﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Functions.SecondaryItem;
using Sprint.Levels;
using Sprint.Sprite;
using System.Net.Http.Headers;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory
    {
        private SpriteLoader spriteLoader;
        private GameObjectManager objManager;

        Vector2 position;
        Vector2 direction;

        private float distanceOut;

        private const string ANIMS_FILE = "projectileAnims";


        public SimpleProjectileFactory(SpriteLoader spriteLoader, float distanceOut, GameObjectManager objManager)
        {
            this.distanceOut = distanceOut;
            this.spriteLoader = spriteLoader;
            this.objManager = objManager;
        }

        public Smoke CreateSmoke()
        {
            return new Smoke(
                spriteLoader.BuildSprite(ANIMS_FILE, "smoke"),
                getSpawnPosition(), objManager);
        }

        public Arrow CreateArrow()
        {
            Arrow proj = new Arrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "arrow"),
                getSpawnPosition(), direction, objManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public BlueArrow CreateBlueArrow()
        {
            BlueArrow proj = new BlueArrow(
                spriteLoader.BuildSprite(ANIMS_FILE, "bluearrow"),
                getSpawnPosition(), direction, objManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public Bomb CreateBomb()
        {
            Bomb proj = new Bomb(
                 spriteLoader.BuildSprite(ANIMS_FILE, "bomb"),
                 getSpawnPosition(), direction, objManager);
            return proj;
        }
        
        public Boomerang CreateBoomarang()
        {
            Boomerang proj = new Boomerang(
                spriteLoader.BuildSprite(ANIMS_FILE, "boomerang"),
                getSpawnPosition(), direction, objManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public BlueBoomerang CreateBlueBoomerang()
        {
            BlueBoomerang proj = new BlueBoomerang(
                spriteLoader.BuildSprite(ANIMS_FILE, "blueboomerang"),
                getSpawnPosition(), direction, objManager);
            proj.SetSmokeCommand(new PlaceSmoke(proj, CreateSmoke()));
            return proj;
        }

        public FireBall CreateFireBall()
        {
            FireBall proj = new FireBall(
                spriteLoader.BuildSprite(ANIMS_FILE, "fireball"),
                getSpawnPosition(), direction, objManager);
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
