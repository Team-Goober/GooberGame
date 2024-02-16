using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint.Projectile
{
    internal class SimpleProjectileFactory
    {
        private Texture2D itemSheet;
        private Texture2D bomb;
        private Texture2D fireBall;
        private Texture2D smoke;

        Vector2 position;
        Vector2 direction;

        private float distanceOut;


        public SimpleProjectileFactory(float distanceOut)
        {
            this.distanceOut = distanceOut;
        }

        public void LoadAllTextures(ContentManager content)
        {
            itemSheet = content.Load<Texture2D>("zelda_items");
            bomb = content.Load<Texture2D>("Items/Bomb");
            fireBall = content.Load<Texture2D>("Items/FireBall");
            smoke = content.Load<Texture2D>("Items/EndArrow");

        }

        public Arrow CreateArrow()
        {
            return new Arrow(itemSheet, smoke, getSpawnPosition(), direction);
        }

        public BlueArrow CreateBlueArrow()
        {
            return new BlueArrow(itemSheet, smoke, getSpawnPosition(), direction);
        }

        public Bomb CreateBomb()
        {
            return new Bomb(bomb, getSpawnPosition(), direction); 
        }
        
        public Boomarang CreateBoomarang()
        { 
            return new Boomarang(itemSheet, getSpawnPosition(), direction);
        }

        public FireBall CreateFireBall()
        {
            return new FireBall(fireBall, getSpawnPosition(), direction);
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
